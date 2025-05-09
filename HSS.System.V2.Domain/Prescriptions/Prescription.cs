using System.ComponentModel.DataAnnotations.Schema;
using HSS.System.V2.Domain.Common;
using HSS.System.V2.Domain.Appointments;

namespace HSS.System.V2.Domain.Prescriptions;

public class Prescription : BaseClass
{
    public string? Notes { get; set; }
    public string AppointmentId { get; set; }

    public string ClinicAppointmentId { get; set; }
    [ForeignKey(nameof(ClinicAppointmentId))]
    public virtual ClinicAppointment ClinicAppointment { get; set; }

    public virtual ICollection<PrescriptionMedicineItem> Items { get; set; }
} 