using System.ComponentModel.DataAnnotations.Schema;
using System.Linq.Expressions;
using HSS.System.V2.Domain.Models.Facilities;
using HSS.System.V2.Domain.Models.Common;
using HSS.System.V2.Domain.Models.Appointments;

namespace HSS.System.V2.Domain.Models.Queues;

public class ClinicQueue : SystemQueue, IQueueModel, IQueueIncludeStrategy<ClinicQueue>
{
    public virtual ICollection<ClinicAppointment> ClinicAppointments { get; set; }
    
    [ForeignKey(nameof(ClinicId))]
    public virtual Clinic Clinic { get; set; }

    public TimeSpan DepartmentStartAt => Clinic.StartAt;
    public TimeSpan DepartmentEndAt => Clinic.EndAt;
    public IEnumerable<Appointment> Appointments => ClinicAppointments;

    public Expression<Func<ClinicQueue, object>> GetInclude()
    {
        return x => x.ClinicAppointments;
    }
} 