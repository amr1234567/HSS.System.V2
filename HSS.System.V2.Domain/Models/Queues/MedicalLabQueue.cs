using System.ComponentModel.DataAnnotations.Schema;
using System.Linq.Expressions;
using HSS.System.V2.Domain.Models.Facilities;
using HSS.System.V2.Domain.Models.Common;
using HSS.System.V2.Domain.Models.Appointments;

namespace HSS.System.V2.Domain.Models.Queues;

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