using System.Text.Json;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;

namespace CommunicationsAlpha2025.Data;

/// <summary>
/// Provides example static data to power the prototype.
/// </summary>
public interface IStaticDataProvider
{
    JsonElement GetPublishedProviderFundingStreamById(string fundingStreamId);
}

/// <inheritdoc cref="IStaticDataProvider"/>
public class StaticDataProvider : IStaticDataProvider
{ 
    private JsonDocument? _publishedProviderFunding;
    
    private readonly IConfiguration _configuration;
    private readonly BlobServiceClient _blobServiceClient;
    
    public StaticDataProvider(
        IConfiguration configuration,
        BlobServiceClient blobServiceClient)
    {
        _configuration = configuration;
        _blobServiceClient = blobServiceClient;
        
        FetchPublishedProviderFundingStreams();
    }

    private void FetchPublishedProviderFundingStreams()
    {
        string container = _configuration["DefaultStorageAccountDataContainer"]
                           ?? throw new InvalidOperationException("DefaultStorageAccountDataContainer not set");
        string filePath = _configuration["DefaultStorageAccountPublishedFundingPath"]
                          ?? throw new InvalidOperationException("DefaultStorageAccountPublishedFundingPath not set");

        BlobContainerClient containerClient = _blobServiceClient.GetBlobContainerClient(container);
        BlobClient blobClient = containerClient.GetBlobClient(filePath);
        
        Console.WriteLine("Attempting to download published funding blob...");
        Azure.Response<BlobDownloadResult> downloadResponse = blobClient.DownloadContent();

        Console.WriteLine("Downloaded. Attempting to get value stream...");
        using var downloadResponseStream = downloadResponse.Value.Content.ToStream();
        
        _publishedProviderFunding = JsonDocument.Parse(downloadResponseStream,
            new JsonDocumentOptions
            {
                AllowTrailingCommas = true,
                CommentHandling = JsonCommentHandling.Skip
            });
        
        Console.Write("Successfully fetched published provider funding stream.");
    }

    public JsonElement GetPublishedProviderFundingStreamById(string fundingStreamId)
    {
        if (!_publishedProviderFunding!.RootElement.TryGetProperty(fundingStreamId, out JsonElement providerFunding))
            throw new NotSupportedException($"Funding stream {fundingStreamId} is not supported.");

        return providerFunding;
    }
}