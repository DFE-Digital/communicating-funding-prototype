namespace CommunicationsAlpha2025.Versions.V2;

/// <summary>
/// Provides augmentations to ESFA's existing dataset to power
/// the prototype.
/// </summary>
public static class Augmentations
{
    /// <summary>
    /// Mapping from funding stream IDs to their names.
    /// This data was manually created by using the CFS production
    /// environment's specification list found here: https://calculate-funding.education.gov.uk/app/SpecificationsList
    /// </summary>
    public static readonly Dictionary<string, string> FundingStreamIdToName = new()
    {
        { "1619", "16-19" },
        { "DADA", "Dance and Drama Award" },
        { "ASF", "Adult skills fund" },
        { "DSG", "Dedicated Schools Grant" },
        { "PSG", "PE and Sport Premium Grant" },
        { "GAG", "Academies General Annual Grant" },
        { "GAGADJ", "GAG Adv & Abt/Adj & Transfer deficits" },
        { "ILPREC", "ILP Reconciliation" },
        { "NFF", "Non Formula Funded Activity" },
        { "NLG", "Non Learning Grants" },
        { "NMSS", "Non maintained special schools revenue" },
        { "RPA", "Risk Protection Arrangements" },
        { "1416", "14-16" },
        { "LAREC", "LA Recoupment" },
        { "PNA", "Pupil Number Adjustments (Main PNA)" },
        { "PP", "Pupil Premium" },
        { "UIFSM", "Universal infant free school meals" },
        { "1619AO", "16 to 19 Allocation Outturn" },
        { "1619Recon", "1619 Reconciliation" },
        { "ALL", "Advanced learner loans" },
        { "CCF", "College Collaboration Fund" },
        { "ASFC", "Contracting - Adult Skills Fund Contract" },
        { "ASFG", "Contracting - Adult Skills Fund Grant" },
        { "LOANS", "Contracting - Advanced Learner Loans" }
    };

    /// <summary>
    /// Mapping from funding period IDs to their names.
    /// </summary>
    public static readonly Dictionary<string, string?> FundingPeriodIdAcronymToName = new()
    {
        { "AY", "Academic year (AY)" },
        { "FY", "Financial year (FY)" },
        { "AS", "Academy and school academic year (AS)" },
        { "AC", "Academy academic year (AC)" }
    };
}