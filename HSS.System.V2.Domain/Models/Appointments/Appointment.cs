using System.ComponentModel.DataAnnotations.Schema;

using HSS.System.V2.Domain.Enums;
using HSS.System.V2.Domain.Models.Common;
using HSS.System.V2.Domain.Models.Facilities;
using HSS.System.V2.Domain.Models.Medical;
using HSS.System.V2.Domain.Models.People;
using HSS.System.V2.Domain.Models.Prescriptions;

namespace HSS.System.V2.Domain.Models.Appointments;

public class Appointment : BaseClass
{
    public string PatientId {  get; set; }
    public virtual Patient Patient { get; set; }
    public string PatientNationalId { get; set; }
    public string PatientName { get; set; }
    public AppointmentState State { get; set; }
    public DateTime SchaudleStartAt { get; set; }
    public DateTime? ActualStartAt { get; set; }
    public TimeSpan? ActualDuration { get; set; }
    public TimeSpan ExpectedDuration { get; set; }
    public string TicketId { get; set; }
    [InverseProperty(nameof(Ticket.Appointments))]
    [ForeignKey(nameof(TicketId))]
    public virtual Ticket Ticket { get; set; }
    public virtual Hospital Hospital { get; set; }
    public string HospitalId { get; set; }
    public string HospitalName { get; set; }

    public string? EmployeeName { get; set; }
    public string DepartmentName { get; set; }

    public string? QueueId {  get; set; }

    public string? MedicalHistoryId {  get; set; }
    [InverseProperty(nameof(Medical.MedicalHistory.Appointments))]
    [ForeignKey(nameof(MedicalHistoryId))]
    public virtual MedicalHistory MedicalHistory {  get; set; }


    public virtual Func<Appointment, object> GetIncludeDepartment()
    {
        return x => Hospital;
    }

    public virtual Func<Appointment, object> GetIncludeEmployee()
    {
        return x => Hospital;
    }
} 