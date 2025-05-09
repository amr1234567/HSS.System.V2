using HSS.System.V2.Domain.Common;
using HSS.System.V2.Domain.Appointments;

namespace HSS.System.V2.Domain.Medical;

public class Test : BaseClass
{
    public string Name { get; set; }
    public string Description { get; set; }
    public double TestPrice { get; set; }
    public double EstimatedDurationInMinutes { get; set; }

    public virtual ICollection<TestRequired> TestsRequired { get; set; }
} 