using HSS.System.V2.Domain.Common;
using HSS.System.V2.Domain.People;
using HSS.System.V2.Domain.Facilities;

namespace HSS.System.V2.Domain.Medical;

public class Specialization : BaseClass
{
    public string Name { get; set; }
    public string Description { get; set; }

    public virtual ICollection<Doctor> Doctors { get; set; }
    public virtual ICollection<Clinic> Clinics { get; set; }
} 