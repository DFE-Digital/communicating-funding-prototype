using System.Text.Json;

namespace CommunicationsAlpha2025.Versions.V2.Models;

public class Statement
{
    public required string FundingStreamId { get; set; }

    public required string FundingStreamName { get; set; }

    public required string FundingPeriodId { get; set; }

    public required string FundingPeriodName { get; set; }

    public decimal TotalValue { get; set; }

    public IEnumerable<FundingLine> FundingLines { get; set; } = [];

    public required Provider Provider { get; set; }

    public static Statement FromCfsDataDocument(JsonElement element)
    {
        JsonElement currentNode = element.GetProperty("content").GetProperty("current");
        // Get 'content.current' nodes
        string fundingStreamId = currentNode.GetProperty("fundingStreamId").GetString()!;
        string fundingStreamName = Augmentations.FundingStreamIdToName.GetValueOrDefault(fundingStreamId)
                                   ?? fundingStreamId;
        string fundingPeriodId = currentNode.GetProperty("fundingPeriodId").GetString()!;
        string fundingPeriodName = Transformations.FormatFundingPeriodIdAsName(fundingPeriodId);
        decimal totalFunding = currentNode.GetProperty("totalFunding").GetDecimal();
        // Get 'content.current.provider' nodes
        JsonElement providerNode = currentNode.GetProperty("provider");
        string providerName = providerNode.GetProperty("name").GetString()!;
        string providerUkprn = providerNode.GetProperty("ukprn").GetString()!;

        return new Statement
        {
            FundingStreamId = fundingStreamId,
            FundingStreamName = fundingStreamName,
            FundingPeriodId = fundingPeriodId,
            FundingPeriodName = fundingPeriodName,
            TotalValue = totalFunding,
            Provider = new Provider
            {
                Name = providerName,
                Ukprn = providerUkprn
            }
        };
    }
}