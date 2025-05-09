using System.ComponentModel.DataAnnotations.Schema;
using HSS.System.V2.Domain.Facilities;
using HSS.System.V2.Domain.Appointments;

namespace HSS.System.V2.Domain.Queues;

public class MedicalLabQueue : SystemQueue
{
    public virtual ICollection<MedicalLabAppointment> MedicalLabAppointments { get; set; }

    public string MedicalLabId { get; set; }
    [ForeignKey(nameof(MedicalLabId))]
    public virtual MedicalLab MedicalLab { get; set; }
} 