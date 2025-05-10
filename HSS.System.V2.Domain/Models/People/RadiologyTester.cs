using System.ComponentModel.DataAnnotations.Schema;

using HSS.System.V2.Domain.Models.Appointments;
using HSS.System.V2.Domain.Models.Facilities;

namespace HSS.System.V2.Domain.Models.People;

public class RadiologyTester : Employee
{
    public string RadiologyCenterName { get; set; }
    public string RadiologyCenterId { get; set; }
    [ForeignKey(nameof(RadiologyCenterId))]
    public virtual RadiologyCenter RadiologyCenter { get; set; }

    public virtual ICollection<RadiologyCeneterAppointment> RadiologyCeneterAppointments { get; set; }
} 