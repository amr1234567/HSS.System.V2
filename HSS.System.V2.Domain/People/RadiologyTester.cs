using System.ComponentModel.DataAnnotations.Schema;
using HSS.System.V2.Domain.Facilities;
using HSS.System.V2.Domain.Appointments;

namespace HSS.System.V2.Domain.People;

public class RadiologyTester : Employee
{
    public string RadiologyCenterName { get; set; }
    public string RadiologyCenterId { get; set; }
    [ForeignKey(nameof(RadiologyCenterId))]
    public virtual RadiologyCenter RadiologyCenter { get; set; }

    public virtual ICollection<RadiologyCeneterAppointment> RadiologyCeneterAppointments { get; set; }
} 