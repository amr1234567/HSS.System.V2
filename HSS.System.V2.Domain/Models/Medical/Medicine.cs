using HSS.System.V2.Domain.Models.Common;
using HSS.System.V2.Domain.Models.Prescriptions;

namespace HSS.System.V2.Domain.Models.Medical;

public class Medicine : BaseClass
{
    public string Name { get; set; }
    public string ActiveIngredient { get; set; }
    public virtual ICollection<MedicinePharmacy> MedicinePharmacies { get; set; }
    public virtual ICollection<PrescriptionMedicineItem> Items { get; set; }
}
