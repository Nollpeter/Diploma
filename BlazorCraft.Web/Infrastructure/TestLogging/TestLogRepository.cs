using Blazored.LocalStorage;
using Newtonsoft.Json;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace BlazorCraft.Web.Infrastructure.TestLogging;


public interface ITestStateRepository<TResult>
{
    Task<TResult> LoadTestStates();
    
}
public interface ITestLoggingRepository
{
    Task SaveResults(TestRunSession results);
}

public interface IExamTestLoggingRepository : ITestLoggingRepository,ITestStateRepository<IDictionary<DateTime, List<KeyValuePair<string, string>>>> { }
public interface IAllTestLoggingRepository : ITestLoggingRepository, ITestStateRepository<IDictionary<string,TestRunState>?> { }

public class AllTestStateLoggingRepository : IAllTestLoggingRepository
{
    private string StorageKey => "LastSession";
    private readonly ILocalStorageService _localStorage;

    public AllTestStateLoggingRepository(ILocalStorageService localStorage)
    {
        _localStorage = localStorage;
    }

    public async Task SaveResults(TestRunSession results)
    {
       
        var serializedSession = JsonConvert.SerializeObject(results.Select(p => new KeyValuePair<string, TestRunState>(p.Key.Id, p.Value)).ToList());
        await _localStorage.SetItemAsync(StorageKey, serializedSession);
    }

    public async Task<IDictionary<string,TestRunState>?> LoadTestStates()
    {
        var serializedSession = await _localStorage.GetItemAsync<string>(StorageKey);

        if (string.IsNullOrEmpty(serializedSession))
        {
            return null;
        }
        
        IDictionary<string,TestRunState> result = JsonConvert.DeserializeObject<IEnumerable<KeyValuePair<string,TestRunState>>>(serializedSession)!.ToDictionary(p => p.Key, p => p.Value);
        return result;

    }

}

public class ExamTestLoggingRepository : IExamTestLoggingRepository
{
    protected virtual string StorageKey => "TestRunSessions";
    protected ILocalStorageService _localStorage;

    public ExamTestLoggingRepository(ILocalStorageService localStorage)
    {
        _localStorage = localStorage;
    }

    public virtual async Task SaveResults(TestRunSession results)
    {
        // Get existing sessions
        var existing = await _localStorage.GetItemAsync<Dictionary<DateTime, string>>(StorageKey) ?? new Dictionary<DateTime, string>();

        //int key = existing.Keys.Any() ? existing.Keys.Max() : 1;
        // Serialize our new session to a string so it can be stored

        var kvpList = results.Select(kvp =>
            new KeyValuePair<string, string>(kvp.Key.Id, kvp.Value.ToString()));

       
        var serialized = JsonSerializer.Serialize(kvpList);

        //var serialized = JsonSerializer.Serialize(results);

        // Add new session to existing
        existing[DateTime.Now] = serialized;

        // Store the updated sessions back into local storage
        await _localStorage.SetItemAsync(StorageKey, existing);
    }

    public async Task<IDictionary<DateTime, List<KeyValuePair<string, string>>>> LoadTestStates()
    {
        // Get sessions from local storage
        var sessions = await _localStorage.GetItemAsync<Dictionary<DateTime, string>>(StorageKey) ?? new Dictionary<DateTime, string>();

        // Deserialize sessions back into our session object
        var result = sessions.ToDictionary(
            kvp => kvp.Key,
            kvp => JsonSerializer.Deserialize<List<KeyValuePair<string, string>>>(kvp.Value)
        );

        return result!;
    }
}