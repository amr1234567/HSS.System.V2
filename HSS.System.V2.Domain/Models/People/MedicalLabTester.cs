using System.ComponentModel.DataAnnotations.Schema;

using HSS.System.V2.Domain.Models.Appointments;
using HSS.System.V2.Domain.Models.Facilities;

namespace HSS.System.V2.Domain.Models.People;

public class MedicalLabTester : Employee
{
    public string MedicalLabName { get; set; }
    public string MedicalLabId { get; set; }
    [ForeignKey(nameof(MedicalLabId))]
    public virtual MedicalLab MedicalLab { get; set; }

    public virtual ICollection<MedicalLabAppointment> MedicalLabAppointments { get; set; }
} 