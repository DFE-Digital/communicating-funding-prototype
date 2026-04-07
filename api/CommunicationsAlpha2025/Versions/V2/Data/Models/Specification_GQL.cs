using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CommunicationsAlpha2025.Versions.V2.Data.Models;

[Table("Specifications", Schema = "calc")]
public class Specification_GQL
{
    [Key]
    public long Id { get; set; }
    public string Name { get; set; } =  string.Empty;
    public string SpecificationId  {get; set;} = string.Empty;
    public ICollection<FundingStream> FundingStreams {get; set;}
}