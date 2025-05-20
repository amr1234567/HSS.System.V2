using System.ComponentModel.DataAnnotations.Schema;

using HSS.System.V2.Domain.Models.Appointments;
using HSS.System.V2.Domain.Models.Common;

namespace HSS.System.V2.Domain.Models.Prescriptions;

public class Prescription : BaseClass
{
    public string? Notes { get; set; }
    public string ClinicAppointmentId { get; set; }
    [ForeignKey(nameof(ClinicAppointmentId))]
    public virtual ClinicAppointment ClinicAppointment { get; set; }

    public virtual ICollection<PrescriptionMedicineItem> Items { get; set; }
} 