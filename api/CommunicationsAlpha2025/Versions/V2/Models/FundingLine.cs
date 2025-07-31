using System.Text.Json.Serialization;

namespace CommunicationsAlpha2025.Versions.V2.Models;

public record FundingLine
{
    public required string Name { get; set; }
    
    public decimal Value { get; set; }
    
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public IEnumerable<DistributionPeriod>? DistributionPeriods { get; set; } = null;
}