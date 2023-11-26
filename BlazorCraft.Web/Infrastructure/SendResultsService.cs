using System.Text;
using System.Text.Json;
using BlazorCraft.Web.Pages._11_Exam;
using Dropbox.Api;
using Dropbox.Api.Files;

namespace BlazorCraft.Web.Infrastructure;

public interface ISendResultsService
{
    Task SendResults(Questionnaire.QuestionnaireModel model);
}

public class SendResultsService : ISendResultsService
{
    private ITestLoggingRepository _loggingRepository;

    public SendResultsService(ITestLoggingRepository loggingRepository)
    {
        _loggingRepository = loggingRepository;
    }

    public async Task SendResults(Questionnaire.QuestionnaireModel model)
    {
        var config = new DropboxClientConfig("diplomamunka-npz")
        {
            HttpClient = new HttpClient()
        };

        using var client = new DropboxClient("sl.Bqd1WJDpZBKoddvatFx-CG7XPC9jEPKnA0upf-b1Kz4pYKVqnvGEVYdPHt7nFM5qjeDR5nszg9TgnwxEgAZysFjLqbs_MaLgTLyKfFyAbrLE0Rvn39v5ofdO1aDAmJKEiEK-wZByc60t5jkB-51U", config);

        var dictionary = await _loggingRepository.GetResults();
        var result = new
        {
            Feedback = model,
            TestResults = dictionary
        };
        var serialize = JsonSerializer.Serialize(result);

        using (var mem = new MemoryStream(Encoding.UTF8.GetBytes(serialize)))
        {
            var updated = await client.Files.UploadAsync(
                path: $"/{Constants.VERSION}/{FormatStringForFilename(model.Name)}{DateTime.Now:yyyyMMddhhmmssff}.json",
                body: mem
            );
        }
    }
    private string FormatStringForFilename(string input)
    {
        if (string.IsNullOrWhiteSpace(input))
        {
            return string.Empty;
        }

        // Replace invalid characters with empty strings.
        string result = string.Join("", input.Split(Path.GetInvalidFileNameChars()));

        // Remove spaces and capitalize the first character of the word
        result = string.Join("", result.Split(' ').Select(word => char.ToUpper(word[0]) + (word.Length > 1 ? word.Substring(1) : "")));

        return result;
    }
}