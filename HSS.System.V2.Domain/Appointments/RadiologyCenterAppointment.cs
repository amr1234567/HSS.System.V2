using System.ComponentModel.DataAnnotations.Schema;
using HSS.System.V2.Domain.Facilities;
using HSS.System.V2.Domain.People;
using HSS.System.V2.Domain.Medical;
using HSS.System.V2.Domain.Queues;
using HSS.System.V2.Domain.Common;

namespace HSS.System.V2.Domain.Appointments;

public class RadiologyCeneterAppointment : Appointment, IAppointmentModel<RadiologyCenterQueue>
{
    public string Result { get; set; }
    public string? ClinicAppointmentId { get; set; }
    [ForeignKey(nameof(ClinicAppointmentId))]
    public virtual ClinicAppointment? ClinicAppointment { get; set; }
    public string RadiologyCeneterId { get; set; }
    [ForeignKey(nameof(RadiologyCeneterId))]
    public virtual RadiologyCenter RadiologyCeneter { get; set; }
    public string TesterId { get; set; }
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
} 