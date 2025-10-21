using System.Text.Json;
using CommunicationsAlpha2025.Versions.V2;
using CommunicationsAlpha2025.Versions.V3.Models;

namespace CommunicationsAlpha2025.Versions.V3;

public interface IStatementFactory
{
    Statement FromCfsDataDocument(JsonElement element);
}

public class StatementFactory : IStatementFactory
{
    public Statement FromCfsDataDocument(JsonElement element)
    {
        JsonElement currentNode = element.GetProperty("content").GetProperty("current");

        // Get root notes
        bool hasUpdatedAt = element.GetProperty("updatedAt").TryGetDateTime(out DateTime updatedAt);
        bool hasCreatedAt = element.GetProperty("createdAt").TryGetDateTime(out DateTime createdAt);
        if (!hasCreatedAt)
            throw new InvalidOperationException("Statement does not have updatedAt or createdAt attribute.");

        // Get 'content.current' nodes
        string fundingStreamId = currentNode.GetProperty("fundingStreamId").GetString()!;
        string fundingStreamName = Augmentations.FundingStreamIdToName.GetValueOrDefault(fundingStreamId)
                                   ?? fundingStreamId;
        string fundingPeriodId = currentNode.GetProperty("fundingPeriodId").GetString()!;
        string fundingPeriodName = Transformations.FormatFundingPeriodIdAsName(fundingPeriodId);
        decimal totalFunding = currentNode.GetProperty("totalFunding").GetDecimal();
        int version = currentNode.GetProperty("version").GetInt32()!;

        // Get 'content.current.provider' nodes
        JsonElement providerNode = currentNode.GetProperty("provider");
        string providerName = providerNode.GetProperty("name").GetString()!;
        string providerUkprn = providerNode.GetProperty("ukprn").GetString()!;

        // Get 'content.current.fundingLines' nodes
        JsonElement fundingLineNode = currentNode.GetProperty("fundingLines");
        JsonElement.ArrayEnumerator fundingLineNodeEnumerator = fundingLineNode.EnumerateArray();
        IEnumerable<FundingLine> fundingLines = fundingLineNodeEnumerator
            .Where(fl =>
                // Don't show anything that doesn't have external name on CFS
                DoesFundingLineHaveExternalName(fl) &&
                // Don't show anything that doesn't have a value, as the provider
                // isn't entitled to the funding line
                DoesFundingLineHaveValue(fl))
            .Select(fl =>
            {
                // Get 'content.current.fundingLines.distributionPeriods' nodes
                IEnumerable<DistributionPeriod>? distributionPeriods = null;
                // Not all funding lines have distribution periods, so handle that
                if (fl.TryGetProperty("distributionPeriods", out JsonElement distributionPeriodsNode)
                    && distributionPeriodsNode.ValueKind != JsonValueKind.Null)
                {
                    JsonElement.ArrayEnumerator distributionPeriodsEnumerator =
                        distributionPeriodsNode.EnumerateArray();

                    distributionPeriods = distributionPeriodsEnumerator
                        .Select(dp =>
                        {
                            // Get 'content.current.fundingLines.distributionPeriods.profilePeriods' nodes
                            JsonElement profilePeriodsNode = dp.GetProperty("profilePeriods");
                            JsonElement.ArrayEnumerator profilePeriodsEnumerator = profilePeriodsNode.EnumerateArray();
                            IEnumerable<ProfilePeriod> profilePeriods = profilePeriodsEnumerator
                                .Select(pp => new ProfilePeriod
                                {
                                    Year = pp.GetProperty("year").GetInt32(),
                                    TypeValue = pp.GetProperty("typeValue").GetString()!,
                                    Value = pp.GetProperty("profiledValue").GetDecimal()
                                });

                            return new DistributionPeriod
                            {
                                Value = dp.GetProperty("value").GetDecimal(),
                                ProfilePeriods = profilePeriods
                            };
                        });
                }

                return new FundingLine
                {
                    Name = fl.GetProperty("name").GetString()!,
                    ExternalName = fl.GetProperty("externalName").GetString()!,
                    Value = fl.GetProperty("value").GetDecimal()!,
                    DistributionPeriods = distributionPeriods
                };
            });

        return new Statement
        {
            FundingStreamId = fundingStreamId,
            FundingStreamName = fundingStreamName,
            FundingPeriodId = fundingPeriodId,
            FundingPeriodName = fundingPeriodName,
            TotalValue = totalFunding,
            FundingLines = fundingLines,
            UpdatedAt = hasUpdatedAt ? updatedAt : createdAt,
            Version = version,
            Provider = new Provider
            {
                Name = providerName,
                Ukprn = providerUkprn
            }
        };
    }

    private static bool DoesFundingLineHaveExternalName(JsonElement fl)
    {
        return fl.TryGetProperty("externalName", out JsonElement externalNameElement) &&
            externalNameElement.ValueKind == JsonValueKind.String;
    }

    private static bool DoesFundingLineHaveValue(JsonElement fl)
    {
        return fl.TryGetProperty("value", out JsonElement valueElement) &&
            valueElement.ValueKind == JsonValueKind.Number;
    }
}