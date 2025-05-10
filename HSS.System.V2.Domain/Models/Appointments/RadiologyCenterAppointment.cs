using System.ComponentModel.DataAnnotations.Schema;
using HSS.System.V2.Domain.Models.People;
using HSS.System.V2.Domain.Models.Queues;
using HSS.System.V2.Domain.Models.Facilities;
using HSS.System.V2.Domain.Models.Common;
using HSS.System.V2.Domain.Models.Medical;

namespace HSS.System.V2.Domain.Models.Appointments;

public class RadiologyCeneterAppointment : Appointment, IAppointmentModel<RadiologyCenterQueue>
{
    public string Result { get; set; }
    public string? ClinicAppointmentId { get; set; }
    [ForeignKey(nameof(ClinicAppointmentId))]
    public virtual ClinicAppointment? ClinicAppointment { get; set; }
    public string RadiologyCeneterId { get; set; }
    public string RadiologyCeneterName { get; set; }
    [ForeignKey(nameof(RadiologyCeneterId))]
    public virtual RadiologyCenter RadiologyCeneter { get; set; }
    public string TesterId { get; set; }
    public string TesterName { get; set; }
    [ForeignKey(nameof(TesterId))]
    public virtual RadiologyTester Tester { get; set; }
    public string TestId { get; set; }
    [ForeignKey(nameof(TestId))]
    public virtual RadiologyTest Test { get; set; }
    public string? QueueId { get; set; }
    [ForeignKey(nameof(QueueId))]
    public virtual RadiologyCenterQueue Queue { get; set; }

    public void SetQueue(RadiologyCenterQueue? queue)
    {
        Queue = queue;
        QueueId = queue is null ? null : queue.Id;
    }

    public RadiologyCenterQueue? GetQueue()
    {
        return Queue;
    }

    public string EmployeeName => TesterName;

    public string DepartmentName => RadiologyCeneterName;
} 