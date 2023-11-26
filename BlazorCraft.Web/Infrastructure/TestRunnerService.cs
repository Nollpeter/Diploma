using System.Reflection;
using BlazorCraft.Web.Infrastructure.Attributes;
using BlazorCraft.Web.Tests.Introduction;

namespace BlazorCraft.Web.Infrastructure;

public interface ITestRunnerService
{
    Task RunAll();
    Task RunAllInPage(Type pageType);
    Task RunAllInTestClass(Type testClass);
    Task RunTest(TestDescriptor testDescriptor);
    Dictionary<TestDescriptor, TestRunResult?> GetTestRunResultMethods(Type testClass);
    event EventHandler<TestRunStateEventArgs> TestStateChanged;
}

public enum TestRunState
{
    NotStarted,
    Running,
    Successful,
    Error
}

public record TestRunStateEventArgs(TestDescriptor TestDescriptor, TestRunResult? TestRunResult, TestRunState TestRunState);

public record TestDescriptor(Func<Task<TestRunResult>> Method, string Title, string Description, string? Hint, Type PageClass, Type TestClass)
{
    public override int GetHashCode() => Title.GetHashCode();
}

public class TestRunnerService : ITestRunnerService
{
    public async Task RunAll()
    {
        var tests = GetEveryTest();
        foreach (var test in tests)
        {
            await RunTest(test.Key);
        }
    }

    public async Task RunAllInPage(Type pageType)
    {
        var keyValuePairs = GetEveryTest().Where(p => p.Key.PageClass == pageType);
        Console.WriteLine(pageType);
        foreach (var kvp in keyValuePairs)
        {
            await RunTest(kvp.Key);
        }
    }

    public async Task RunAllInTestClass(Type testClass)
    {
        var testRunResultMethods = GetTestRunResultMethods(testClass);
        foreach (var testRunResult in testRunResultMethods)
        {
            await RunTest(testRunResult.Key);
        }
    }

    public async Task RunTest(TestDescriptor testDescriptor)
    {
        TestStateChanged?.Invoke(this, new TestRunStateEventArgs(testDescriptor, null, TestRunState.Running));
        try
        {
            var contextIdMethod = await testDescriptor.Method();
            TestStateChanged?.Invoke(this, new TestRunStateEventArgs(testDescriptor, contextIdMethod, contextIdMethod.IsSuccessful ? TestRunState.Successful : TestRunState.Error));
        }
        catch (Exception e)
        {
            TestStateChanged?.Invoke(this, new TestRunStateEventArgs(testDescriptor, new TestRunResult(false, e.Message), TestRunState.Error));
        }

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
                    var func = (Func<Task<TestRunResult>>)Delegate.CreateDelegate(typeof(Func<Task<TestRunResult>>), null, methodInfo);
                    resultList.Add(
                        new(func, titleAttribute?.Title, descriptionAttribute?.Description, hintAttribute?.Hint,
                            testForPageAttribute.Page, type), null);
                }


            }
        }

        Console.WriteLine(resultList.Count);
        return resultList;
    }

    public Dictionary<TestDescriptor, TestRunResult?> GetTestRunResultMethods(Type testClass)
    {
        return GetEveryTest().Where(p => p.Key.TestClass == testClass).ToDictionary(p => p.Key, p => p.Value);
    }

    public event EventHandler<TestRunStateEventArgs>? TestStateChanged;
}