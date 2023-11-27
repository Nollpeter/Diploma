﻿using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using Azure;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using BlazorCraft.Web.Pages._11_Exam;
using Dropbox.Api;

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
        // Serialize the results
        var dictionary = await _loggingRepository.GetResults();
        var result = new
        {
            Feedback = model,
            TestResults = dictionary
        };
        var serialize = JsonSerializer.Serialize(result);

        // Convert serialized data to byte array
        byte[] byteArray = Encoding.UTF8.GetBytes(serialize);
        
        // Generate filename for the blob
        string fileName = $"{FormatStringForFilename(model.Name)}{DateTime.Now:yyyyMMddhhmmssff}.json";

        using var memoryStream = new MemoryStream(byteArray);
        // Upload to Azure Blob Storage
        await UploadToAzureBlob(memoryStream, fileName);
    }

    private async Task UploadToAzureBlob(MemoryStream fileData, string fileName)
    {
        var blobServiceClient = new BlobServiceClient(new Uri("https://diplomamunkanpz.blob.core.windows.net"), new AzureSasCredential("si=everything&sv=2022-11-02&sr=c&sig=RlH3%2FjrzZYoH3MtQfbNLOA5N1wwqO8j8st164Hma3Kw%3D"));
        var blobContainerClient = blobServiceClient.GetBlobContainerClient("results");
        var response = await blobContainerClient.UploadBlobAsync(fileName, fileData);
    }

    private string FormatStringForFilename(string input)
    {
        if (string.IsNullOrWhiteSpace(input))
        {
            return string.Empty;
        }

        // Your existing filename formatting logic
        string result = string.Join("", input.Split(Path.GetInvalidFileNameChars()));
        result = string.Join("", result.Split(' ').Select(word => char.ToUpper(word[0]) + (word.Length > 1 ? word.Substring(1) : "")));

        return result;
    }
}
