using System.ComponentModel.DataAnnotations.Schema;

using HSS.System.V2.Domain.Models.Facilities;

namespace HSS.System.V2.Domain.Models.People;

public class Pharmacist : Employee
{
    public string PharmacyId { get; set; }
    [ForeignKey(nameof(PharmacyId))]
    public virtual Pharmacy Pharmacy { get; set; }
} 