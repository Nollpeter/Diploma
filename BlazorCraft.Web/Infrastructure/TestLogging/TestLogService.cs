namespace BlazorCraft.Web.Infrastructure.TestLogging;


public interface ITestLoggerService
{
    ITestLoggingSession CreateLoggingSession(ITestRunnerService testRunnerService);
}

public class TestLoggerService : ITestLoggerService
{
    private readonly IExamTestLoggingRepository _examTestLoggingRepository;
    private readonly IAllTestLoggingRepository _allTestStateLoggingRepository;

    public TestLoggerService(IExamTestLoggingRepository examTestLoggingRepository, IAllTestLoggingRepository allTestStateLoggingRepository)
    {
        _examTestLoggingRepository = examTestLoggingRepository;
        _allTestStateLoggingRepository = allTestStateLoggingRepository;
    }

    public ITestLoggingSession CreateLoggingSession(ITestRunnerService testRunnerService)
    {
        return new CompositeTestSession(new TestLoggingSession(testRunnerService, _allTestStateLoggingRepository), new ExamTestLoggingSession(testRunnerService, _examTestLoggingRepository));
    }
}