using HSS.System.V2.Domain.Models.Common;
using HSS.System.V2.Domain.Models.Facilities;

using System.ComponentModel.DataAnnotations.Schema;

namespace HSS.System.V2.Domain.Models.Medical;

public class MedicinePharmacy : BaseClass
{
    public string MedicineId { get; set; }
    public string MedicineName { get; set; }
    [ForeignKey(nameof(MedicineId))]
    public virtual Medicine Medicine { get; set; }

    public string PharmacyId { get; set; }
    [ForeignKey(nameof(PharmacyId))]
    public virtual Pharmacy Pharmacy { get; set; }

    public int Quantity { get; set; }
    public decimal Price { get; set; }
}