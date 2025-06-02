using System.ComponentModel.DataAnnotations.Schema;
using System.Linq.Expressions;
using HSS.System.V2.Domain.Models.Facilities;
using HSS.System.V2.Domain.Models.Common;
using HSS.System.V2.Domain.Models.Appointments;

namespace HSS.System.V2.Domain.Models.Queues;

public class RadiologyCenterQueue : SystemQueue
{
    public virtual ICollection<RadiologyCeneterAppointment> RadiologyCeneterAppointments { get; set; }
    [ForeignKey(nameof(RadiologyCeneterId))]
    public virtual RadiologyCenter RadiologyCenter { get; set; }
    [NotMapped]
    public override IEnumerable<Appointment> Appointments
    {
        get
        {
            return RadiologyCeneterAppointments;
        }
        set
        {
            RadiologyCeneterAppointments = new HashSet<RadiologyCeneterAppointment>(value.Cast<RadiologyCeneterAppointment>());
        }
    }

    public override TimeSpan DepartmentStartAt => RadiologyCenter.StartAt;
    public override TimeSpan DepartmentEndAt => RadiologyCenter.EndAt;
} 