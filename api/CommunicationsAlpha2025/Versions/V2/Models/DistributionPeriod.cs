namespace CommunicationsAlpha2025.Versions.V2.Models;

public record DistributionPeriod
{
    public required decimal Value { get; set; }

    public IEnumerable<ProfilePeriod> ProfilePeriods { get; set; } = [];
}