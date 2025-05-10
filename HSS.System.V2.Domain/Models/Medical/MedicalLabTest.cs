using HSS.System.V2.Domain.Models.Appointments;
using HSS.System.V2.Domain.Models.Facilities;

namespace HSS.System.V2.Domain.Models.Medical;

public class MedicalLabTest : Test
{
    public string SampleType { get; set; }

    public virtual ICollection<MedicalLabAppointment> MedicalLabAppointments { get; set; }
    public virtual ICollection<MedicalLab> MedicalLabs { get; set; }
} 