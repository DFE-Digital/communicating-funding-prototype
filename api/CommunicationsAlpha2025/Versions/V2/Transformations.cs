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

    /// <summary>
    /// Cleans up funding stream names by removing underscores and
    /// capitalizing the first letter of each word.
    /// </summary>
    private static string CleanFundingStreamName(string fundingStreamName)
    {
        var words = fundingStreamName
            .Replace('_', ' ')
            .Split(' ');

        List<string> cleanedWords = [];
        foreach (var word in words)
        {
            string cleanedWord = word;
            // Lowercase anything that is not an acronym
            if (word.Length < 2 || (word.Length > 2 && !word.All(char.IsUpper)))
            {
                cleanedWord = word.ToLowerInvariant();
            }

            cleanedWords.Add(cleanedWord);
        }

        var sentence = string.Join(' ', cleanedWords)
            .Trim();
        // Capitalize the first letter of the sentence
        sentence = char.ToUpper(sentence[0]) + sentence[1..];
        return sentence;
    }

    /// <summary>
    /// Replaces specific funding stream names with more human-readable alternatives.
    /// </summary>
    private static string TransformToHumanReadableFundingStreamName(string name)
    {
        return name.ToLower() switch
        {
            "of which asf adult skills" => "Adult skills core",
            "free courses for jobs grant" => "Free courses for jobs",
            _ => name
        };
    }

    /// <summary>
    /// Formats funding stream names to be more human-readable.
    /// </summary>
    public static string FormatAsHumanReadableFundingStreamName(string fundingStreamName)
    {
        fundingStreamName = TransformToHumanReadableFundingStreamName(fundingStreamName);
        fundingStreamName = CleanFundingStreamName(fundingStreamName);
        return fundingStreamName;
    }
}