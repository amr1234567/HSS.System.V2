using HSS.System.V2.Domain.Common;
using HSS.System.V2.Domain.Prescriptions;

namespace HSS.System.V2.Domain.Medical;

public class Medicine : BaseClass
{
    public string Name { get; set; }
    public string ActiveIngredient { get; set; }

    public virtual ICollection<PrescriptionMedicineItem> Items { get; set; }
} 