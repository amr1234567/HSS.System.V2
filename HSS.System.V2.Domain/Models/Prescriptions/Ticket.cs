using HSS.System.V2.Domain.Enums;
using System.ComponentModel.DataAnnotations.Schema;
using HSS.System.V2.Domain.Models.People;
using HSS.System.V2.Domain.Models.Common;
using HSS.System.V2.Domain.Models.Appointments;
using HSS.System.V2.Domain.Models.Facilities;

namespace HSS.System.V2.Domain.Models.Prescriptions;

public class Ticket : BaseClass
{
    public string PatientNationalId { get; set; }
    public string PatientName { get; set; }
    public TicketState State { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? ClosedAt { get; set; }
    public string? FirstClinicAppointmentId { get; set; }
    public virtual ClinicAppointment FirstClinicAppointment { get; set; }
    public string PatientId { get; set; }
    [ForeignKey(nameof(PatientId))]
    public virtual Patient Patient { get; set; }

    [ForeignKey(nameof(HospitalCreatedInId))]
    public virtual Hospital HospitalCreatedIn { get; set; }
    public string HospitalCreatedInId { get; set; }

    [InverseProperty(nameof(Appointment.Ticket))]
    public virtual ICollection<Appointment> Appointments { get; set; }
} 