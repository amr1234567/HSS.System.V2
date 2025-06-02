using System.ComponentModel.DataAnnotations.Schema;
using System.Linq.Expressions;
using HSS.System.V2.Domain.Models.Facilities;
using HSS.System.V2.Domain.Models.Common;
using HSS.System.V2.Domain.Models.Appointments;

namespace HSS.System.V2.Domain.Models.Queues;

public class MedicalLabQueue : SystemQueue
{
    public virtual ICollection<MedicalLabAppointment> MedicalLabAppointments { get; set; }

    [ForeignKey(nameof(MedicalLabId))]
    public virtual MedicalLab MedicalLab { get; set; }

    [NotMapped]
    public override IEnumerable<Appointment> Appointments
    {
        get
        {
            return MedicalLabAppointments;
        }
        set
        {
            MedicalLabAppointments = new HashSet<MedicalLabAppointment>(value.Cast<MedicalLabAppointment>());
        }
    }

    public override TimeSpan DepartmentStartAt => MedicalLab.StartAt;
    public override TimeSpan DepartmentEndAt => MedicalLab.EndAt;

} 