
namespace CommunicationsAlpha2025.Versions.V3.Models;

public class Statement
{
    public required string FundingStreamId { get; set; }

    public required string FundingStreamName { get; set; }

    public required string FundingPeriodId { get; set; }

    public required string FundingPeriodName { get; set; }

    public decimal TotalValue { get; set; }

    public IEnumerable<FundingLine> FundingLines { get; set; } = [];

    public required Provider Provider { get; set; }
    
    public required DateTime UpdatedAt { get; set; }
    
    public int Version { get; set; }
}