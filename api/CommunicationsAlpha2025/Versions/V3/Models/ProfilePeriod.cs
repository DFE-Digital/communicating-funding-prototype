namespace CommunicationsAlpha2025.Versions.V3.Models;

public record ProfilePeriod
{
    public int Year { get; set; }

    public required string TypeValue { get; set; }
    
    public decimal Value { get; set; }
}