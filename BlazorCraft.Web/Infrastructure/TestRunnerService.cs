using System.Reflection;
using System.Text.RegularExpressions;
using BlazorCraft.Web.Infrastructure.Attributes;
using BlazorCraft.Web.Tests;
using BlazorCraft.Web.Tests.Introduction;
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
}

public enum TestRunState
{
    NotStarted,
    Running,
    Successful,
    Error,
    AtLeastOneNotStarted,
}

//public record TestRunSession(IDictionary<TestDescriptor, TestRunResult?> TestsRun);
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

        if (values.Any(p => p.Value == TestRunState.NotStarted || p.Value == TestRunState.AtLeastOneNotStarted))
        {
            return TestRunState.AtLeastOneNotStarted;
        }

        return TestRunState.Successful;
    }

}

public record TestRunStateEventArgs(TestDescriptor TestDescriptor, TestRunResult? TestRunResult, TestRunSession Session,
    TestRunState TestRunState);

public record TestDescriptor(Func<Task<TestRunResult>> Method, string Title, string Description, string? Hint,
    Type PageClass, Type TestClass)
{
    public override int GetHashCode() => Title.GetHashCode();
}

public class TestRunnerService : ITestRunnerService
{
    public async Task RunTests(IEnumerable<KeyValuePair<TestDescriptor,TestRunResult?>> tests)
    {
        TestRunSession session = new(tests.ToDictionary(p => p.Key, p => TestRunState.Running));
        foreach (var test in tests)
        {
            OnTestStateChanged(new TestRunStateEventArgs(test.Key, null, session, TestRunState.Running));
        }
        await Task.Delay(100);
        foreach (var test in tests)
        {
            //await Task.Run(async () => await RunTest(test.Key));
            await RunTest(test.Key, session);
            await Task.Delay(1);
        }
    }
    
    public async Task RunAll()
    {
        await RunTests(GetEveryTest());
        //var tests = GetEveryTest();
        //TestRunSession session = new(tests.ToDictionary(p => p.Key, p => TestRunState.NotStarted));
        //foreach (var test in tests)
        //{
        //    //await Task.Run(async () => await RunTest(test.Key));
        //    await RunTest(test.Key, session);
        //    await Task.Delay(1);
        //}
    }

    public async Task RunAllInPage(Type pageType)
    {
        await RunTests(GetEveryTest().Where(p => p.Key.PageClass == pageType));
        //var tests = GetEveryTest()
        //    .Where(p => p.Key.PageClass == pageType)
        //    .ToDictionary(p => p.Key, p => p.Value);
        //TestRunSession session = new(tests.ToDictionary(p => p.Key, p => TestRunState.NotStarted));
        //foreach (var test in tests)
        //{
        //    //await Task.Run(async () => await RunTest(kvp.Key));
        //    await RunTest(test.Key, session);
        //    await Task.Delay(1);
        //}
    }

    public async Task RunAllInTestClass(Type testClass)
    {
        await RunTests(GetTestRunResultMethods(testClass));
        //var tests = GetTestRunResultMethods(testClass)
        //    .ToDictionary(p => p.Key, p => p.Value);
        //TestRunSession session = new(tests.ToDictionary(p => p.Key, p => TestRunState.NotStarted));
        //foreach (var testRunResult in tests)
        //{
        //    //await Task.Run(async () => await RunTest(testRunResult.Key));
        //    await RunTest(testRunResult.Key, session);
        //    await Task.Delay(1);
        //}
    }

    public async Task RunTest(TestDescriptor testDescriptor, TestRunSession session)
    {
        try
        {
            var contextIdMethod = await testDescriptor.Method();
            OnTestStateChanged(new TestRunStateEventArgs(testDescriptor, contextIdMethod, session,
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
                    if (methodInfo.ReturnType != typeof(Task<TestRunResult>))
                    {
                        continue;
                    }

                    var titleAttribute = methodInfo.GetCustomAttribute<TitleAttribute>();
                    var descriptionAttribute = methodInfo.GetCustomAttribute<DescriptionAttribute>();
                    var hintAttribute = methodInfo.GetCustomAttribute<HintAttribute>();
                    var func = (Func<Task<TestRunResult>>)Delegate.CreateDelegate(typeof(Func<Task<TestRunResult>>),
                        null, methodInfo);
                    resultList.Add(
                        new(func, titleAttribute?.Title, descriptionAttribute?.Description, hintAttribute?.Hint,
                            testForPageAttribute.Page, type), null);
                }
            }
        }
        return resultList;
    }

    public Dictionary<TestDescriptor, TestRunResult?> GetTestRunResultMethods(Type testClass)
    {
        return GetEveryTest().Where(p => p.Key.TestClass == testClass).ToDictionary(p => p.Key, p => p.Value);
    }

    public TestRunSession GetSessionForPage(Type pageType)
    {
        return new(GetEveryTest().Where(p => p.Key.PageClass == pageType)
            .ToDictionary(p => p.Key, p => TestRunState.NotStarted));
    }

    public TestRunSession GetSessionForTestClass(Type testClass)
    {
        return new(GetEveryTest().Where(p => p.Key.TestClass == testClass)
            .ToDictionary(p => p.Key, p => TestRunState.NotStarted));
    }

    public event EventHandler<TestRunStateEventArgs>? TestStateChanged;
}