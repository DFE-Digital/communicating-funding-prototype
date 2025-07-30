using System.Text.Json;

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
    private readonly JsonDocument _statementsDocument;

    public StaticDataProvider(IConfiguration configuration)
    {
        string path = configuration["DataPaths:PublishedFundingStreams"]!
                      ?? throw new InvalidOperationException(
                          "DataPaths:PublishedFundingStreams has not been populated");
        
        using FileStream fs = File.OpenRead(path);
        
        _statementsDocument = JsonDocument.Parse(fs,
            new JsonDocumentOptions
            {
                AllowTrailingCommas = true,
                CommentHandling = JsonCommentHandling.Skip
            });
    }

    public JsonElement GetPublishedProviderFundingStreamById(string fundingStreamId)
    {
        if (!_statementsDocument.RootElement.TryGetProperty(fundingStreamId, out JsonElement statement))
            throw new NotSupportedException($"Funding stream {fundingStreamId} is not supported.");

        return statement;
    }
}