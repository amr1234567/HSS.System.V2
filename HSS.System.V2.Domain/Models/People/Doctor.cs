using System.ComponentModel.DataAnnotations.Schema;
using HSS.System.V2.Domain.Models.Facilities;
using HSS.System.V2.Domain.Models.Appointments;
using HSS.System.V2.Domain.Models.Medical;

namespace HSS.System.V2.Domain.Models.People;

public class Doctor : Employee
{
    public string SpecializationId { get; set; }
    [ForeignKey(nameof(SpecializationId))]
    public virtual Specialization Specialization { get; set; }
    public string SpecializationName { get; set; }

    public string ClinicId { get; set; }
    [ForeignKey(nameof(ClinicId))]
    public virtual Clinic Clinic { get; set; }


    public virtual ICollection<ClinicAppointment> ClinicAppointments { get; set; }
} 