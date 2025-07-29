namespace CommunicationsAlpha2025.Versions.V2.Models;

public record Provider
{
    public required string Name { get; set; }
    
    public required string Ukprn { get; set; }
}