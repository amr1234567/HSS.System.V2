using HSS.System.V2.Domain.Models.Common;
using HSS.System.V2.Domain.Models.Facilities;
using HSS.System.V2.Domain.Models.People;

namespace HSS.System.V2.Domain.Models.Medical;

public class Specialization : BaseClass
{
    public string Name { get; set; }
    public string Description { get; set; }

    public virtual ICollection<Doctor> Doctors { get; set; }
    public virtual ICollection<Clinic> Clinics { get; set; }
} 