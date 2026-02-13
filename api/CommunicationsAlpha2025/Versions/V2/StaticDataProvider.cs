using System.Text.Json;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;

namespace CommunicationsAlpha2025.Versions.V2;

/// <summary>
/// Provides example static data to power the prototype.
/// </summary>
public interface IStaticDataProvider
{
    /// <summary>
    /// Gets the JSON element of a specific funding stream by its ID.
    /// </summary>
    JsonElement GetPublishedProviderFundingStreamById(string fundingStreamId);

    /// <summary>
    /// Gets all published provider funding streams as an enumerable of JSON elements.
    /// </summary>
    IEnumerable<JsonElement> GetAllPublishedProviderFundingStreams();

    /// <summary>
    /// Gets prototype calc result as the raw json string.
    /// </summary>
    string GetPrototypeCalcResult();
}

/// <inheritdoc cref="IStaticDataProvider"/>
public class StaticDataProvider : IStaticDataProvider
{
    private JsonDocument? _publishedProviderFunding;
    private string? _publishedCalcsResults;

    private readonly IConfiguration _configuration;

    public StaticDataProvider(
        IConfiguration configuration)
    {
        _configuration = configuration;

        FetchPublishedFundingData();
    }

    private void FetchPublishedFundingData()
    {
        string storageUri = _configuration["DefaultStorageAccountUri"]
                            ?? throw new InvalidOperationException("DefaultStorageAccountUri not set");
        string container = _configuration["DefaultStorageAccountDataContainer"]
                           ?? throw new InvalidOperationException("DefaultStorageAccountDataContainer not set");
        string filePathProviderData = _configuration["DefaultStorageAccountPublishedFundingPath"]
                          ?? throw new InvalidOperationException("DefaultStorageAccountPublishedFundingPath not set");
        string filePathCalcsData = _configuration["DefaultStorageAccountPublishedCalcsResultsPath"]
                  ?? throw new InvalidOperationException("DefaultStorageAccountPublishedCalcsResultsPath not set");

        GetBlobData(storageUri, container, filePathProviderData, ref _publishedProviderFunding);
        GetJsonString(storageUri, container, filePathCalcsData, ref _publishedCalcsResults);
    }

    private static void GetBlobData(string storageUri, string container, string filePathProviderData, ref JsonDocument? jsonData)
    {
        var blobUri = new Uri($"{storageUri}/{container}/{filePathProviderData}");
        var anonymousBlobClient = new BlobClient(blobUri);

        Console.WriteLine("Attempting to download published funding blob...");
        Azure.Response<BlobDownloadResult> downloadResponse = anonymousBlobClient.DownloadContent();

        Console.WriteLine("Downloaded. Attempting to get value stream...");
        using var downloadResponseStream = downloadResponse.Value.Content.ToStream();

        jsonData = JsonDocument.Parse(downloadResponseStream,
            new JsonDocumentOptions
            {
                AllowTrailingCommas = true,
                CommentHandling = JsonCommentHandling.Skip
            });

        Console.Write("Successfully fetched json document.");
    }

    private static void GetJsonString(string storageUri, string container, string filePathProviderData, ref string? jsonstring)
    {
        var blobUri = new Uri($"{storageUri}/{container}/{filePathProviderData}");
        var anonymousBlobClient = new BlobClient(blobUri);

        Console.WriteLine("Attempting to download published funding blob...");
        Azure.Response<BlobDownloadResult> downloadResponse = anonymousBlobClient.DownloadContent();
        jsonstring = downloadResponse.Value.Content.ToString();

        Console.Write("Successfully fetched json string.");
    }

    public JsonElement GetPublishedProviderFundingStreamById(string fundingStreamId)
    {
        if (_publishedProviderFunding == null)
            throw new InvalidOperationException("Published provider funding streams have not been fetched.");

        if (!_publishedProviderFunding!.RootElement.TryGetProperty(fundingStreamId, out JsonElement providerFunding))
            throw new NotSupportedException($"Funding stream {fundingStreamId} is not supported.");

        return providerFunding;
    }

    public IEnumerable<JsonElement> GetAllPublishedProviderFundingStreams()
    {
        if (_publishedProviderFunding == null)
            throw new InvalidOperationException("Published provider funding streams have not been fetched.");

        return _publishedProviderFunding.RootElement.EnumerateObject()
            .Select(property => property.Value);
    }

    public string GetPrototypeCalcResult()
    {
        if (string.IsNullOrWhiteSpace(_publishedCalcsResults))
            throw new InvalidOperationException("Published calc results have not been fetched.");

        return _publishedCalcsResults;
    }
}