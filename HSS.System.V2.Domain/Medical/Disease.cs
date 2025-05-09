using HSS.System.V2.Domain.Common;
using HSS.System.V2.Domain.Appointments;

namespace HSS.System.V2.Domain.Medical;

public class Disease : BaseClass
{
    public string Name { get; set; }
    public string Description { get; set; }

    public virtual ICollection<ClinicAppointment> ClinicAppointments { get; set; }
} 