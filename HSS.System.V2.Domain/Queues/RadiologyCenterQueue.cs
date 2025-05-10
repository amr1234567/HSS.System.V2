using System.ComponentModel.DataAnnotations.Schema;
using HSS.System.V2.Domain.Facilities;
using HSS.System.V2.Domain.Appointments;
using HSS.System.V2.Domain.Common;
using System.Linq.Expressions;

namespace HSS.System.V2.Domain.Queues;

public class RadiologyCenterQueue : SystemQueue, IQueueModel, IQueueIncludeStrategy<RadiologyCenterQueue>
{
    public virtual ICollection<RadiologyCeneterAppointment> RadiologyCeneterAppointments { get; set; }
    public string RadiologyCenterId { get; set; }
    [ForeignKey(nameof(RadiologyCenterId))]
    public virtual RadiologyCenter RadiologyCenter { get; set; }
    public string DepartmentId => RadiologyCenterId;
    public IEnumerable<Appointment> Appointments => RadiologyCeneterAppointments;

    public TimeSpan DepartmentStartAt => RadiologyCenter.StartAt;
    public TimeSpan DepartmentEndAt => RadiologyCenter.EndAt;


    public Expression<Func<RadiologyCenterQueue, object>> GetInclude()
    {
        return x => x.RadiologyCeneterAppointments;
    }
} 