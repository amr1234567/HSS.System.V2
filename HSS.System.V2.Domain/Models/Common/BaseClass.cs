using System.ComponentModel.DataAnnotations;

namespace HSS.System.V2.Domain.Models.Common;

public class BaseClass
{
    [Key]
    public string Id { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}
