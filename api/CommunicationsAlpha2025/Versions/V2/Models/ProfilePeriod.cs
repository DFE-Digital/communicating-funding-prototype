namespace CommunicationsAlpha2025.Versions.V2.Models;

public record ProfilePeriod
{
    public int Year { get; set; }
    
    public required string TypeValue { get; set; }
}