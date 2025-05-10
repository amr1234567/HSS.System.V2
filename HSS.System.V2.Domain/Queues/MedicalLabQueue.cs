using System.ComponentModel.DataAnnotations.Schema;
using HSS.System.V2.Domain.Facilities;
using HSS.System.V2.Domain.Appointments;
using HSS.System.V2.Domain.Common;
using System.Linq.Expressions;

namespace HSS.System.V2.Domain.Queues;

public class MedicalLabQueue : SystemQueue, IQueueModel, IQueueIncludeStrategy<MedicalLabQueue>
{
    public virtual ICollection<MedicalLabAppointment> MedicalLabAppointments { get; set; }

    public string MedicalLabId { get; set; }
    [ForeignKey(nameof(MedicalLabId))]
    public virtual MedicalLab MedicalLab { get; set; }

    public string DepartmentId => MedicalLabId;
    public IEnumerable<Appointment> Appointments => MedicalLabAppointments;

    public TimeSpan DepartmentStartAt => MedicalLab.StartAt;
    public TimeSpan DepartmentEndAt => MedicalLab.EndAt;


    public Expression<Func<MedicalLabQueue, object>> GetInclude()
    {
        return x => x.MedicalLabAppointments;
    }
} 