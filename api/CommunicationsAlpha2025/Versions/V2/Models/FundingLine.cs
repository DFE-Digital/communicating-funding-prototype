namespace CommunicationsAlpha2025.Versions.V2.Models;

public record FundingLine
{
    public required string Name { get; set; }
    public decimal Value { get; set; }
}