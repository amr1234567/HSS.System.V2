using System.ComponentModel.DataAnnotations.Schema;
using HSS.System.V2.Domain.Enums;
using System.Diagnostics.CodeAnalysis;
using HSS.System.V2.Domain.Models.People;
using HSS.System.V2.Domain.Models.Queues;
using HSS.System.V2.Domain.Models.Facilities;
using HSS.System.V2.Domain.Models.Common;
using HSS.System.V2.Domain.Models.Medical;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;

namespace HSS.System.V2.Domain.Models.Appointments;

public class MedicalLabAppointment : Appointment, 
    IAppointmentModel<MedicalLabQueue>
{
    [AllowNull]
    public string Result { get; set; }
    public MedicalLabAppointmentState MedicalLabAppointmentState { get; set; }
    public DateTime ReceiveResultDate { get; set; }
    public string? ClinicAppointmentId { get; set; }
    [ForeignKey(nameof(ClinicAppointmentId))]
    public virtual ClinicAppointment ClinicAppointment { get; set; }
    public string MedicalLabId { get; set; }
    public string MedicalLabName { get; set; }
    [ForeignKey(nameof(MedicalLabId))]
    public virtual MedicalLab MedicalLab { get; set; }
    public string? QueueId { get; set; }
    [ForeignKey(nameof(QueueId))]
    public virtual MedicalLabQueue Queue { get; set; }
    public string TesterId { get; set; }
    public string TesterName { get; set; }  
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

    public string EmployeeName => TesterName;

    public string DepartmentName => MedicalLabName;


    public override Func<Appointment, object> GetIncludeDepartment()
    {
        return x => ((MedicalLabAppointment)x).MedicalLab;
    }

    public override Func<Appointment, object> GetIncludeEmployee()
    {
        return x => ((MedicalLabAppointment)x).Tester;
    }
} 