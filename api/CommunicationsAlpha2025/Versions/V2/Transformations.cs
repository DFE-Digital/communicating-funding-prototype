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
        string yearType = parts[0];
        string periodSpan = parts[1];

        string yearTypeFormatted = yearType switch
        {
            "AY" => "academic year (AY)",
            "FY" => "financial year (FY)",
            _ => throw new NotSupportedException($"Year type '{yearType}' is not supported")
        };
        
        int yearStart = int.Parse(periodSpan[..2]);
        int yearEnd = int.Parse(periodSpan[^2..]);
        
        return $"{yearTypeFormatted} 20{yearStart} to 20{yearEnd}";
    }
}