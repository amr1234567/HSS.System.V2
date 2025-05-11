using HSS.System.V2.Domain.Models.Common;
using HSS.System.V2.Domain.Models.Facilities;
using HSS.System.V2.Domain.Models.People;

using System.ComponentModel.DataAnnotations;

namespace HSS.System.V2.Domain.Models.Medical;

public class Specialization : BaseClass
{
    public string Name { get; set; }
    public string Description { get; set; }

    [DataType(DataType.Html)]
    public string? Icon { get; set; }

    public virtual ICollection<Doctor> Doctors { get; set; }
    public virtual ICollection<Clinic> Clinics { get; set; }
} 