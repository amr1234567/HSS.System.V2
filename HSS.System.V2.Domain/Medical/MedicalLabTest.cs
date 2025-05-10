using HSS.System.V2.Domain.Appointments;
using HSS.System.V2.Domain.Facilities;

namespace HSS.System.V2.Domain.Medical;

public class MedicalLabTest : Test
{
    public string SampleType { get; set; }

    public virtual ICollection<MedicalLabAppointment> MedicalLabAppointments { get; set; }
    public virtual ICollection<MedicalLab> MedicalLabs { get; set; }
} 