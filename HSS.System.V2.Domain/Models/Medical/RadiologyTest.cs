using HSS.System.V2.Domain.Models.Appointments;
using HSS.System.V2.Domain.Models.Facilities;

namespace HSS.System.V2.Domain.Models.Medical;

public class RadiologyTest : Test
{
    public string BodyPart { get; set; }
    public string RequiresContrast { get; set; }
    public string PreparationInstructions { get; set; }

    public virtual ICollection<RadiologyCeneterAppointment> RadiologyCeneterAppointments { get; set; }
    public virtual ICollection<RadiologyCenter> RadiologyCenters { get; set; }
} 