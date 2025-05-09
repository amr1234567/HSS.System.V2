using System.ComponentModel.DataAnnotations.Schema;
using HSS.System.V2.Domain.Facilities;
using HSS.System.V2.Domain.Appointments;

namespace HSS.System.V2.Domain.Queues;

public class ClinicQueue : SystemQueue
{
    public virtual ICollection<ClinicAppointment> ClinicAppointments { get; set; }
    
    public string ClinicId { get; set; }
    [ForeignKey(nameof(ClinicId))]
    public virtual Clinic Clinic { get; set; }
} 