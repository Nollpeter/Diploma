using System.Reflection;
using System.Text.RegularExpressions;
using BlazorCraft.Web.Infrastructure.Attributes;
using BlazorCraft.Web.Infrastructure.TestLogging;
using BlazorCraft.Web.Pages._11_Exam;
using BlazorCraft.Web.Tests;
using Bunit;

namespace BlazorCraft.Web.Infrastructure;

public interface ITestRunnerService
{
    Task RunAll();
    Task RunAllInPage(Type pageType);
    Task RunAllInTestClass(Type testClass);
    Task RunTest(TestDescriptor testDescriptor, TestRunSession session);
    Dictionary<TestDescriptor, TestRunResult?> GetTestRunResultMethods(Type testClass);
    TestRunSession GetSessionForPage(Type pageType);
    event EventHandler<TestRunStateEventArgs> TestStateChanged;
    TestRunSession GetSessionForTestClass(Type testClass);

    bool ExamCompleted();
    TestRunSession TestsSession { get; }

	Task Initialize();
}

public enum TestRunState
{
    NotStarted,
    Running,
    Successful,
    Error,
    AtLeastOneNotStarted,
}

public class TestRunSession : Dictionary<TestDescriptor, TestRunState>
{
    public TestRunSession(IDictionary<TestDescriptor, TestRunState> dictionary) : base(dictionary)
    {
    }

    public TestRunState GetSessionStateInAll()
    {
        return GetSessionState(this);
    }

    public TestRunState GetSessionStateInPage(Type pageType)
    {
        return GetSessionState(this.Where(p => p.Key.PageClass == pageType));
    }

    public TestRunState GetSessionStateInTestClass(Type testClass)
    {
        return GetSessionState(this.Where(p => p.Key.TestClass == testClass));
    }

    public TestRunState GetSessionState(IEnumerable<KeyValuePair<TestDescriptor, TestRunState>> values)
    {
        
        if (values.Any(p => p.Value == TestRunState.Running))
        {
            return TestRunState.Running;
        }

        if (values.Any(p => p.Value == TestRunState.Error))
        {
            return TestRunState.Error;
        }

        if (values.All(p => p.Value == TestRunState.NotStarted))
        {
            return TestRunState.NotStarted;
        }
        
        if (values.Any(p => p.Value == TestRunState.NotStarted || p.Value == TestRunState.AtLeastOneNotStarted))
        {
            return TestRunState.AtLeastOneNotStarted;
        }
        

        return TestRunState.Successful;
    }

}

public record TestRunStateEventArgs(TestDescriptor TestDescriptor, TestRunResult? TestRunResult, TestRunSession Session,
    TestRunState TestRunState);

public record TestDescriptor(Func<Task> Method, string Title, string Description, string? Hint,
							 Type PageClass, Type TestClass, bool IsPrecondition, ComponentTestBase testClassInstance, string Id)
{
	public bool IsExamTest => TestClass.Name.Contains("Exam");
	public override int GetHashCode()
    {
        return Id.GetHashCode();
    }

    public virtual bool Equals(TestDescriptor? other)
    {
        if (ReferenceEquals(null, other)) return false;
        if (ReferenceEquals(this, other)) return true;
        return Id == other.Id;
    }

    public string ToShortString()
	{
		return $"Title: {Title}, PageClass: {PageClass.Name}, TestClass: {TestClass.Name}, IsPrecondition: {IsPrecondition}";
	}
}

public class TestRunnerService : ITestRunnerService
{
	public TestRunSession TestsSession { get; private set; } = null!;
    private Dictionary<TestDescriptor, TestRunResult?> _testRunResults;
    private IServiceProvider _serviceProvider;
    private readonly ITestLoggerService _testLoggerService;
	private readonly IAllTestLoggingRepository _allTestStateLoggingRepository;
    public TestRunnerService(IServiceProvider serviceProvider, IAllTestLoggingRepository allTestStateLoggingRepository)
    {
        _serviceProvider = serviceProvider;
		_allTestStateLoggingRepository = allTestStateLoggingRepository;
		_testLoggerService = serviceProvider.GetRequiredService<ITestLoggerService>();
        _testRunResults = GetEveryTest();
    }

	public async Task Initialize()
    {
        var loadLastSession = await _allTestStateLoggingRepository.LoadTestStates();
        if (loadLastSession != null)
        {
            TestsSession = InitTestDescriptorMethods(loadLastSession);
            //TODO: Save and load Test results as well
            _testRunResults = GetEveryTest();
        }
        else
        {
            var testRunResults = GetEveryTest();
            TestsSession = new TestRunSession(testRunResults.ToDictionary(p => p.Key, p => TestRunState.NotStarted));
            _testRunResults = testRunResults;
        }
    }
    
    
    public async Task RunTests(IEnumerable<KeyValuePair<TestDescriptor,TestRunResult?>> tests)
    {
        await using (_testLoggerService.CreateLoggingSession(this))
        {
            foreach (var test in tests)
            {
                OnTestStateChanged(new TestRunStateEventArgs(test.Key, null, TestsSession, TestRunState.Running));
            }

            //Let the UI update
            await Task.Delay(100);

            foreach (var test in tests)
            {
                await RunTest(test.Key, TestsSession);

                //Let the UI update
                await Task.Delay(1);
            }
        }
    }
    
    public async Task RunAll()
    {
        await RunTests(GetEveryTest());
    }

    public async Task RunAllInPage(Type pageType)
    {
        await RunTests(GetEveryTest().Where(p => p.Key.PageClass == pageType));
    }

    public async Task RunAllInTestClass(Type testClass)
    {
        await RunTests(GetTestRunResultMethods(testClass));
    }

    public async Task RunTest(TestDescriptor testDescriptor, TestRunSession session)
    {
        try
        {
            if (!testDescriptor.IsPrecondition)
            {
                await testDescriptor.testClassInstance.CheckPreconditions();
            }

            await testDescriptor.Method();
            OnTestStateChanged(new TestRunStateEventArgs(testDescriptor, TestRunResult.Success, session,
                TestRunState.Successful));
        }
        catch (TestRunException e)
        {
            OnTestStateChanged(new TestRunStateEventArgs(testDescriptor, new TestRunResult(false, e.Message), session,
                TestRunState.Error));
        }
        catch (HtmlEqualException e)
        {
            string actualHtmlPattern = @"Actual HTML:\s*(.+?)\s*Expected HTML:";
            string expectedHtmlPattern = @"Expected HTML:\s*(.+)";

            Match actualHtmlMatch = Regex.Match(e.Message, actualHtmlPattern, RegexOptions.Singleline);
            Match expectedHtmlMatch = Regex.Match(e.Message, expectedHtmlPattern, RegexOptions.Singleline);
            string actualHtml = actualHtmlMatch.Groups[1].Value.Trim();
            string expectedHtml = expectedHtmlMatch.Groups[1].Value.Trim();
            OnTestStateChanged(
                new TestRunStateEventArgs(testDescriptor,
                    new HtmlMarkupMismatchTestRunResult("The rendered HTML markup of the component is not as expected!",
                        expectedHtml, actualHtml), session, TestRunState.Error));
        }
        catch (PreconditionsFailedException e)
        {
            OnTestStateChanged(new TestRunStateEventArgs(testDescriptor, new PreconditionsNotMetTestRunResult(e),
                session, TestRunState.Error));
        }
        catch (CollectionsNotEquivalentException e)
        {
            OnTestStateChanged(new TestRunStateEventArgs(testDescriptor,
                new CollectionsNotEquivalentTestRunResult(e.Expected, e.Actual, e.Message), session,
                TestRunState.Error));
        }
        catch (Exception e)
        {
            OnTestStateChanged(new TestRunStateEventArgs(testDescriptor,
                new TestRunResult(false, String.Join(Environment.NewLine, e.GetType().Name, e.Message, e.StackTrace)),
                session,
                TestRunState.Error));
        }
    }

    private void OnTestStateChanged(TestRunStateEventArgs eventArgs)
    {
        
        eventArgs.Session[eventArgs.TestDescriptor] = eventArgs.TestRunState;
        _testRunResults[eventArgs.TestDescriptor] = eventArgs.TestRunResult;
        TestStateChanged?.Invoke(this, eventArgs);
    }
    public static Type? GetTypeByName(string typeName)
    {
        foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies())
        {
            foreach (var type in assembly.GetTypes())
            {
                if (type.Name == typeName)
                    return type;
            }
        }
        return null;
    }
    public TestRunSession InitTestDescriptorMethods(IDictionary<string, TestRunState> session)
    {
        TestRunSession initedSession = new TestRunSession(new Dictionary<TestDescriptor, TestRunState>());
        foreach (var grouping in session.GroupBy(p => p.Key.Split(".")[0]))
        {
            var testClass = GetTypeByName(grouping.Key) ?? throw new TypeLoadException(grouping.Key);
            var testClassInstance = _serviceProvider.GetRequiredService(testClass);
            foreach (var kvp in grouping)
            {
                var strings = kvp.Key.Split(".");
                var methodName = strings[1];
                var methodInfo = testClass.GetMethod(methodName,BindingFlags.Instance | BindingFlags.Public);
                if (methodInfo == null)
                {
                    continue;
                }
                
                var title = methodInfo.GetCustomAttribute<TitleAttribute>()?.Title ?? string.Empty;
                var description = methodInfo.GetCustomAttribute<DescriptionAttribute>()?.Description ?? string.Empty;
                var hint = methodInfo.GetCustomAttribute<HintAttribute>()?.Hint;
                var isPrecondition = methodInfo.GetCustomAttribute<PreconditionAttribute>() != null;
                var pageClass = testClass.GetCustomAttribute<TestForPageAttribute>()!.Page;
                var func = (Func<Task>)Delegate.CreateDelegate(typeof(Func<Task>),
                    testClassInstance, methodInfo);
                TestDescriptor testDescriptor = new TestDescriptor(func, title, description, hint, pageClass, testClass, isPrecondition, (ComponentTestBase)testClassInstance, kvp.Key);
                initedSession[testDescriptor] = kvp.Value;
            }
        }

        return initedSession;
    }
    
    private Dictionary<TestDescriptor, TestRunResult?> GetEveryTest()
    {
        Dictionary<TestDescriptor, TestRunResult?> resultList = new();

        var types = Assembly.GetExecutingAssembly().GetTypes();
        foreach (var type in types)
        {
            var testForPageAttribute = type.GetCustomAttribute<TestForPageAttribute>();
            if (testForPageAttribute != null)
            {
                foreach (var methodInfo in type.GetMethods(BindingFlags.Instance | BindingFlags.Public))
                {
                    var titleAttribute = methodInfo.GetCustomAttribute<TitleAttribute>();
                    if (titleAttribute == null)
                    {
                        continue;
                    }
                    var descriptionAttribute = methodInfo.GetCustomAttribute<DescriptionAttribute>();
                    var hintAttribute = methodInfo.GetCustomAttribute<HintAttribute>();
                    var isPrecondition = methodInfo.GetCustomAttribute<PreconditionAttribute>() != null;

                    var testClassInstance = _serviceProvider.GetRequiredService(type);
                    
                    var func = (Func<Task>)Delegate.CreateDelegate(typeof(Func<Task>),
                        testClassInstance, methodInfo);
                    string id = type.Name+"."+methodInfo.Name;
                    resultList.Add(
                        new(func, titleAttribute?.Title ?? string.Empty, descriptionAttribute?.Description ?? string.Empty, hintAttribute?.Hint,
                            testForPageAttribute.Page, type, isPrecondition, (ComponentTestBase)testClassInstance, id), null);
                }
            }
        }
        return resultList;
    }

    public Dictionary<TestDescriptor, TestRunResult?> GetTestRunResultMethods(Type testClass)
    {
        return _testRunResults.Where(p => p.Key.TestClass == testClass).ToDictionary(p => p.Key, p => p.Value);
    }

    public TestRunSession GetSessionForPage(Type pageType)
    {
        return new(TestsSession.Where(p => p.Key.PageClass == pageType).ToDictionary(p => p.Key, p => p.Value));
    }

    public TestRunSession GetSessionForTestClass(Type testClass)
    {
		var sessionForTestClass = new TestRunSession(TestsSession.Where(p => p.Key.TestClass == testClass).ToDictionary(p => p.Key, p => p.Value));
        
		return sessionForTestClass;
    }

    public bool ExamCompleted()
    {
        return TestsSession.Where(p => p.Key.PageClass == typeof(Exam)).All(p => p.Value == TestRunState.Successful);
    }

    public event EventHandler<TestRunStateEventArgs>? TestStateChanged;
}