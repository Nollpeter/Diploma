using System.Text;
using System.Text.Json;
using Dropbox.Api;
using Dropbox.Api.Files;

namespace BlazorCraft.Web.Infrastructure;

public interface ISendResultsService
{
    Task SendResults();
}

public class SendResultsService : ISendResultsService
{
    private ITestLoggingRepository _loggingRepository;

    public SendResultsService(ITestLoggingRepository loggingRepository)
    {
        _loggingRepository = loggingRepository;
    }

    public async Task SendResults()
    {
        var config = new DropboxClientConfig("diplomamunka-npz")
        {
            HttpClient = new HttpClient()
        };

        using var client = new DropboxClient("sl.Bqd1WJDpZBKoddvatFx-CG7XPC9jEPKnA0upf-b1Kz4pYKVqnvGEVYdPHt7nFM5qjeDR5nszg9TgnwxEgAZysFjLqbs_MaLgTLyKfFyAbrLE0Rvn39v5ofdO1aDAmJKEiEK-wZByc60t5jkB-51U", config);

        var dictionary = await _loggingRepository.GetResults();
        var serialize = JsonSerializer.Serialize(dictionary);

        using (var mem = new MemoryStream(Encoding.UTF8.GetBytes(serialize)))
        {
            var updated = await client.Files.UploadAsync(
                path: $"/{Constants.VERSION}/test.json",
                body: mem
            );
        }
    }
}