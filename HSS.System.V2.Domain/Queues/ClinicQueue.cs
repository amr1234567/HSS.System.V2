using System.ComponentModel.DataAnnotations.Schema;
using HSS.System.V2.Domain.Facilities;
using HSS.System.V2.Domain.Appointments;
using HSS.System.V2.Domain.Common;
using System.Linq.Expressions;

namespace HSS.System.V2.Domain.Queues;

public class ClinicQueue : SystemQueue, IQueueModel, IQueueIncludeStrategy<ClinicQueue>
{
    public virtual ICollection<ClinicAppointment> ClinicAppointments { get; set; }
    
    public string ClinicId { get; set; }
    [ForeignKey(nameof(ClinicId))]
    public virtual Clinic Clinic { get; set; }

    public string DepartmentId => ClinicId;
    public TimeSpan DepartmentStartAt => Clinic.StartAt;
    public TimeSpan DepartmentEndAt => Clinic.EndAt;
    public IEnumerable<Appointment> Appointments => ClinicAppointments;

    public Expression<Func<ClinicQueue, object>> GetInclude()
    {
        return x => x.ClinicAppointments;
    }
} 