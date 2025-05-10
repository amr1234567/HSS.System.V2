using System.ComponentModel.DataAnnotations.Schema;
using HSS.System.V2.Domain.Enums;
using HSS.System.V2.Domain.Facilities;
using HSS.System.V2.Domain.People;
using HSS.System.V2.Domain.Medical;
using HSS.System.V2.Domain.Queues;
using HSS.System.V2.Domain.Common;

namespace HSS.System.V2.Domain.Appointments;

public class MedicalLabAppointment : Appointment, IAppointmentModel<MedicalLabQueue>
{
    public string Result { get; set; }
    public MedicalLabAppointmentState MedicalLabAppointmentState { get; set; }
    public DateTime ReceiveResultDate { get; set; }
    public string? ClinicAppointmentId { get; set; }
    [ForeignKey(nameof(ClinicAppointmentId))]
    public virtual ClinicAppointment ClinicAppointment { get; set; }
    public string MedicalLabId { get; set; }
    [ForeignKey(nameof(MedicalLabId))]
    public virtual MedicalLab MedicalLab { get; set; }
    public string? QueueId { get; set; }
    [ForeignKey(nameof(QueueId))]
    public virtual MedicalLabQueue Queue { get; set; }
    public string TesterId { get; set; }
    [ForeignKey(nameof(TesterId))]
    public virtual MedicalLabTester Tester { get; set; }
    public string TestId { get; set; }
    [ForeignKey(nameof(TesterId))]
    public virtual MedicalLabTest Test { get; set; }

    public void SetQueue(MedicalLabQueue? queue)
    {
        Queue = queue;
        QueueId = queue is null ? null : queue.Id;
    }

    public MedicalLabQueue? GetQueue()
    {
        return Queue;
    }
} 