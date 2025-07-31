namespace CommunicationsAlpha2025.Versions.V2;

/// <summary>
/// Provides transformations applied on ESFA's existing dataset to power
/// the prototype.
/// </summary>
public static class Transformations
{
    /// <summary>
    /// Converts funding periods to human-readable names.
    /// For e.g. FY-2425 -> Financial year (FY) 2024 to 2025
    /// </summary>
    public static string FormatFundingPeriodIdAsName(string fundingPeriodId)
    {
        string[] parts = fundingPeriodId.Split('-');
        string fundingPeriodIdAcronym = parts[0];
        string periodSpan = parts[1];
        
        if (!Augmentations.FundingPeriodIdAcronymToName.TryGetValue(fundingPeriodIdAcronym, out string? fundingPeriodName))
        {
            throw new NotSupportedException($"Funding period ID acronym '{fundingPeriodIdAcronym}' is not supported");
        }
        
        int yearStart = int.Parse(periodSpan[..2]);
        int yearEnd = int.Parse(periodSpan[^2..]);
        
        return $"{fundingPeriodName!} 20{yearStart} to 20{yearEnd}";
    }
}