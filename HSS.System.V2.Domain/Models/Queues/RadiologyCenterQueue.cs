using System.ComponentModel.DataAnnotations.Schema;
using System.Linq.Expressions;
using HSS.System.V2.Domain.Models.Facilities;
using HSS.System.V2.Domain.Models.Common;
using HSS.System.V2.Domain.Models.Appointments;

namespace HSS.System.V2.Domain.Models.Queues;

public class RadiologyCenterQueue : SystemQueue, IQueueModel, IQueueIncludeStrategy<RadiologyCenterQueue>
{
    public virtual ICollection<RadiologyCeneterAppointment> RadiologyCeneterAppointments { get; set; }
    [ForeignKey(nameof(RadiologyCeneterId))]
    public virtual RadiologyCenter RadiologyCenter { get; set; }
    public IEnumerable<Appointment> Appointments => RadiologyCeneterAppointments;

    public TimeSpan DepartmentStartAt => RadiologyCenter.StartAt;
    public TimeSpan DepartmentEndAt => RadiologyCenter.EndAt;


    public Expression<Func<RadiologyCenterQueue, object>> GetInclude()
    {
        return x => x.RadiologyCeneterAppointments;
    }
} 