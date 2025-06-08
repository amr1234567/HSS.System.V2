using HSS.System.V2.Domain.Enums;
using HSS.System.V2.Domain.Models.Appointments;
using HSS.System.V2.Domain.Models.Common;
using HSS.System.V2.Domain.Models.Facilities;
using HSS.System.V2.Domain.Models.People;

using System.ComponentModel.DataAnnotations.Schema;

namespace HSS.System.V2.Domain.Models.Medical;

public class MedicalHistory : BaseClass
{
    public string PatientId { get; set; }
    public virtual Patient Patient { get; set; }
    public string PatientNationalId { get; set; }
    public string PatientName { get; set; }
    //public TicketState State { get; set; }
    public DateTime CreatedAt { get; set; }
    public string TicketId { get; set; }
    public string? FirstClinicAppointmentId { get; set; }
    [ForeignKey(nameof(FirstClinicAppointmentId))]
    public virtual ClinicAppointment FirstClinicAppointment { get; set; }

    [InverseProperty(nameof(Appointment.MedicalHistory))]
    public virtual ICollection<Appointment> Appointments { get; set; }
    public string? FinalDiagnosis { get; set; }
}