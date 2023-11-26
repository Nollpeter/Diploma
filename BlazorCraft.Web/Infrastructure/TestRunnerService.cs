using System.Reflection;
using System.Text.RegularExpressions;
using BlazorCraft.Web.Infrastructure.Attributes;
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
    Type PageClass, Type TestClass, bool IsPrecondition, ComponentTestBase testClassInstance)
{
    public bool IsExamTest => TestClass.Name.Contains("Exam");
    public override int GetHashCode() => Title.GetHashCode();

    public string ToShortString()
    {
        return $"Title: {Title}, PageClass: {PageClass.Name}, TestClass: {TestClass.Name}, IsPrecondition: {IsPrecondition}";
    }
}

public class TestRunnerService : ITestRunnerService
{

    private TestRunSession _testsSession;
    private Dictionary<TestDescriptor, TestRunResult?> _testRunResults;
    private IServiceProvider _serviceProvider;
    private readonly ITestLoggerService _testLoggerService;
    public TestRunnerService(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
        _testLoggerService = serviceProvider.GetRequiredService<ITestLoggerService>();
        _testsSession = new TestRunSession(GetEveryTest().ToDictionary(p => p.Key, p => TestRunState.NotStarted));
        _testRunResults = GetEveryTest();
    }
    
    
    public async Task RunTests(IEnumerable<KeyValuePair<TestDescriptor,TestRunResult?>> tests)
    {
        await using (_testLoggerService.CreateLoggingSession(this, tests.Select(p => p.Key).ToArray()))
        {
            foreach (var test in tests)
            {
                OnTestStateChanged(new TestRunStateEventArgs(test.Key, null, _testsSession, TestRunState.Running));
            }

            //Let the UI update
            await Task.Delay(100);

            foreach (var test in tests)
            {
                await RunTest(test.Key, _testsSession);

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
                    resultList.Add(
                        new(func, titleAttribute?.Title, descriptionAttribute?.Description, hintAttribute?.Hint,
                            testForPageAttribute.Page, type, isPrecondition, (ComponentTestBase)testClassInstance), null);
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
        return new(_testsSession.Where(p => p.Key.PageClass == pageType).ToDictionary(p => p.Key, p => p.Value));
    }

    public TestRunSession GetSessionForTestClass(Type testClass)
    {
        return new(_testsSession.Where(p => p.Key.TestClass == testClass).ToDictionary(p => p.Key, p => p.Value));
    }

    public bool ExamCompleted()
    {
        return _testsSession.Where(p => p.Key.PageClass == typeof(Exam)).All(p => p.Value == TestRunState.Successful);
    }

    public event EventHandler<TestRunStateEventArgs>? TestStateChanged;
}