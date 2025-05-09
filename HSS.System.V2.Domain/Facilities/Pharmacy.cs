using System.ComponentModel.DataAnnotations.Schema;
using HSS.System.V2.Domain.Common;
using HSS.System.V2.Domain.People;
using HSS.System.V2.Domain.Enums;

namespace HSS.System.V2.Domain.Facilities;

public class Pharmacy : BaseClass, IHospitalDepartmentItem
{
    public string Name { get; set; }
    public int NumberOfShifts { get; set; }
    public TimeSpan StartAt { get; set; }
    public TimeSpan EndAt { get; set; }
    public PharmacyType PharmacyType { get; set; }
    public string HospitalId { get; set; }
    [ForeignKey(nameof(HospitalId))]
    public virtual Hospital Hospital { get; set; }

    public virtual ICollection<Pharmacist> Pharmacists { get; set; }
} 