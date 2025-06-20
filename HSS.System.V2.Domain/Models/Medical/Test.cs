using HSS.System.V2.Domain.Models.Common;

using System.ComponentModel.DataAnnotations;

namespace HSS.System.V2.Domain.Models.Medical;

public class Test : BaseClass
{
    public string Name { get; set; }
    public string Description { get; set; }
    public double TestPrice { get; set; }
    public double EstimatedDurationInMinutes { get; set; }

    [DataType(DataType.Html)]
    public string? Icon { get; set; }

    public virtual ICollection<TestRequired> TestsRequired { get; set; }
}
