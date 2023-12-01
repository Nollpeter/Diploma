namespace BlazorCraft.Web.Infrastructure.TestLogging;

public interface ITestLoggingSession: IAsyncDisposable
{
    void LogTest(object? sender, TestRunStateEventArgs e);
}

public class TestLoggingSession : ITestLoggingSession
{
    private readonly ITestRunnerService _service;
    private readonly ITestLoggingRepository _allTestLoggingRepository;

    public TestLoggingSession(ITestRunnerService service, IAllTestLoggingRepository allTestLoggingRepository)
    {
        _service = service;
        _allTestLoggingRepository = allTestLoggingRepository;
    }

    public void LogTest(object? sender, TestRunStateEventArgs e)
    {
    }

    public async ValueTask DisposeAsync()
    {
        await _allTestLoggingRepository.SaveResults(_service.TestsSession);
    }
}

public class ExamTestLoggingSession : ITestLoggingSession
{
    private readonly ITestRunnerService _service;
    private TestRunSession _testResults;
    private readonly ITestLoggingRepository _testLoggingRepository;

    public ExamTestLoggingSession(ITestRunnerService service, IExamTestLoggingRepository testLoggingRepository)
    {
        _service = service;
        _testLoggingRepository = testLoggingRepository;
        _testResults = new TestRunSession(new Dictionary<TestDescriptor, TestRunState>());
        _service.TestStateChanged += LogTest;
    }

    public void LogTest(object? sender, TestRunStateEventArgs e)
    {
        if (!e.TestDescriptor.IsExamTest) return;
        _testResults[e.TestDescriptor] = e.TestRunState;
    }

    public async ValueTask DisposeAsync()
    {
        if (_testResults.Any())
        {
            await _testLoggingRepository.SaveResults(_testResults);
        }

        _service.TestStateChanged -= LogTest;
        
    }
}

public class CompositeTestSession : ITestLoggingSession
{
    private readonly TestLoggingSession _testLoggingSession;
    private readonly ExamTestLoggingSession _examTestLoggingSession;

    public CompositeTestSession(TestLoggingSession testLoggingSession, ExamTestLoggingSession examTestLoggingSession)
    {
        _testLoggingSession = testLoggingSession;
        _examTestLoggingSession = examTestLoggingSession;
    }

    public void LogTest(object? sender, TestRunStateEventArgs e)
    {
        _testLoggingSession.LogTest(sender, e);
        _examTestLoggingSession.LogTest(sender, e);
    }

    public async ValueTask DisposeAsync()
    {
        await _testLoggingSession.DisposeAsync();
        await _examTestLoggingSession.DisposeAsync();
    }
}