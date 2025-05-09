using System.ComponentModel.DataAnnotations.Schema;
using HSS.System.V2.Domain.Common;
using HSS.System.V2.Domain.Enums;
using HSS.System.V2.Domain.Prescriptions;

namespace HSS.System.V2.Domain.Appointments;

public class Appointment : BaseClass
{
    public string PatientNationalId { get; set; }
    public AppointmentState State { get; set; }
    public DateTime SchaudleStartAt { get; set; }
    public DateTime? ActualStartAt { get; set; }
    public TimeSpan? ActualDuration { get; set; }
    public TimeSpan ExpectedDuration { get; set; }
    public string TicketId { get; set; }
    [InverseProperty(nameof(Ticket.Appointments))]
    [ForeignKey(nameof(TicketId))]
    public virtual Ticket Ticket { get; set; }
} 