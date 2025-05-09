using HSS.System.V2.Domain.Facilities;
using HSS.System.V2.Domain.Appointments;

namespace HSS.System.V2.Domain.Medical;

public class RadiologyTest : Test
{
    public string BodyPart { get; set; }
    public string RequiresContrast { get; set; }
    public string PreparationInstructions { get; set; }

    public virtual ICollection<RadiologyCeneterAppointment> RadiologyCeneterAppointments { get; set; }
    public virtual ICollection<RadiologyCenter> RadiologyCenters { get; set; }
} 