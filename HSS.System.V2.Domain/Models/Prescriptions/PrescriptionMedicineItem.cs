using System.ComponentModel.DataAnnotations.Schema;

using HSS.System.V2.Domain.Models.Common;
using HSS.System.V2.Domain.Models.Medical;

namespace HSS.System.V2.Domain.Models.Prescriptions;

public class PrescriptionMedicineItem : BaseClass
{
    public string MedicineName { get; set; }
    public string? Instructions { get; set; }
    public int Quantity { get; set; }
    public int TimesPerDay { get; set; }
    public int DurationInDays { get; set; }
    public string MedicineId { get; set; }
    public virtual Medicine Medicine { get; set; }
    public string PrescriptionId { get; set; }
    public virtual Prescription Prescription { get; set; }
} 