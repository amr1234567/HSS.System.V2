using HSS.System.V2.Domain.Models.Appointments;
using HSS.System.V2.Domain.Models.Common;

namespace HSS.System.V2.Domain.Models.Medical;

public class Disease : BaseClass
{
    public string Name { get; set; }
    public string Description { get; set; }

    public virtual ICollection<ClinicAppointment> ClinicAppointments { get; set; }
} 