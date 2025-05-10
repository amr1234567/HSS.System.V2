using HSS.System.V2.Domain.Common;
using HSS.System.V2.Domain.Enums;
using HSS.System.V2.Domain.People;
using HSS.System.V2.Domain.Appointments;
using System.ComponentModel.DataAnnotations.Schema;

namespace HSS.System.V2.Domain.Prescriptions;

public class Ticket : BaseClass
{
    public string PatientNationalId { get; set; }
    public string PatientName { get; set; }
    public TicketState State { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? EndedAt { get; set; }
    public string? FirstClinicAppointmentId { get; set; }
    public virtual ClinicAppointment FirstClinicAppointment { get; set; }
    public string PatientId { get; set; }
    public virtual Patient Patient { get; set; }

    [InverseProperty(nameof(Appointment.Ticket))]
    public virtual ICollection<Appointment> Appointments { get; set; }
} 