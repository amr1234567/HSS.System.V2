using System.ComponentModel.DataAnnotations.Schema;
using System.Linq.Expressions;
using HSS.System.V2.Domain.Models.Facilities;
using HSS.System.V2.Domain.Models.Common;
using HSS.System.V2.Domain.Models.Appointments;

namespace HSS.System.V2.Domain.Models.Queues;

public class ClinicQueue : SystemQueue
{
    public virtual ICollection<ClinicAppointment> ClinicAppointments { get; set; }
    
    [ForeignKey(nameof(ClinicId))]
    public virtual Clinic Clinic { get; set; }

    public override TimeSpan DepartmentStartAt => Clinic.StartAt;
    public override TimeSpan DepartmentEndAt => Clinic.EndAt;
    [NotMapped]
    public override IEnumerable<Appointment> Appointments 
    {
        get
        {
            return ClinicAppointments;
        }
        set
        {
            ClinicAppointments = new HashSet<ClinicAppointment>(value.Cast<ClinicAppointment>());
        }
    }
} 