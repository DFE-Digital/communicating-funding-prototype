using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CommunicationsAlpha2025.Versions.V2.Data.Models;

[Table("fundingStreams",Schema = "calc")]
public class FundingStream
{
    [Key]
    public long Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public long SpecificationId { get; set; }
    public Specification_GQL Specification {get; set;}
}