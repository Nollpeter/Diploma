using System.Text.Json;
using Blazored.LocalStorage;

namespace BlazorCraft.Web.Infrastructure;

public class TestLoggingSession : IAsyncDisposable
{
    private readonly TestRunnerService _service;
    private readonly IEnumerable<TestDescriptor> _tests;
    private TestRunSession _testResults;
    private readonly ITestLoggingRepository _testLoggingRepository;

    public TestLoggingSession(TestRunnerService service, IEnumerable<TestDescriptor> tests, ITestLoggingRepository testLoggingRepository)
    {
        _service = service;
        _tests = tests;
        _testLoggingRepository = testLoggingRepository;
        _testResults = new TestRunSession(_tests.ToDictionary(p => p, p => TestRunState.NotStarted));
        _service.TestStateChanged += LogTest;
        
    }

    private void LogTest(object? sender, TestRunStateEventArgs e)
    {
        if(!e.TestDescriptor.IsExamTest) return;
        _testResults[e.TestDescriptor] = e.TestRunState;
    }

    public async ValueTask DisposeAsync()
    {
        await _testLoggingRepository.SaveResults(_testResults);
        _service.TestStateChanged -= LogTest;
    }
}

public interface ITestLoggingRepository
{
    Task SaveResults(TestRunSession results);
    Task<IDictionary<DateTime, List<KeyValuePair<string, string>>>> GetResults();
}

public class TestLoggingRepository : ITestLoggingRepository
{
    const string StorageKey = "TestRunSessions";
    private ILocalStorageService _localStorage;

    public TestLoggingRepository(ILocalStorageService localStorage)
    {
        _localStorage = localStorage;
    }

    public async Task SaveResults(TestRunSession results)
    {
        // Get existing sessions
        var existing = await _localStorage.GetItemAsync<Dictionary<DateTime, string>>(StorageKey) ?? new Dictionary<DateTime, string>();

        //int key = existing.Keys.Any() ? existing.Keys.Max() : 1;
        // Serialize our new session to a string so it can be stored
        
        var kvpList = results.Select(kvp => 
            new KeyValuePair<string, string>(kvp.Key.ToShortString(), kvp.Value.ToString()));

        var serialized = JsonSerializer.Serialize(kvpList);
        
        //var serialized = JsonSerializer.Serialize(results);

        // Add new session to existing
        existing[DateTime.Now] = serialized;
        
        // Store the updated sessions back into local storage
        await _localStorage.SetItemAsync(StorageKey, existing);
    }

    public async Task<IDictionary<DateTime, List<KeyValuePair<string,string>>>> GetResults()
    {
        // Get sessions from local storage
        var sessions = await _localStorage.GetItemAsync<Dictionary<DateTime, string>>(StorageKey) ?? new Dictionary<DateTime, string>();

        // Deserialize sessions back into our session object
        var result = sessions.ToDictionary(
            kvp => kvp.Key, 
            kvp => JsonSerializer.Deserialize<List<KeyValuePair<string,string>>>(kvp.Value)
        );

        return result!;
    }
}

public interface ITestLoggerService
{
    TestLoggingSession CreateLoggingSession(TestRunnerService _service, IEnumerable<TestDescriptor> array);

}
public class TestLoggerService : ITestLoggerService
{
    private readonly ITestLoggingRepository _repository;

    public TestLoggerService(ITestLoggingRepository repository)
    {
        _repository = repository;
    }

    public TestLoggingSession CreateLoggingSession(TestRunnerService _service, IEnumerable<TestDescriptor> array)
    {
        return new TestLoggingSession(_service, array, _repository);
    }

}