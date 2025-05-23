using HSS.System.V2.Domain.Models.Appointments;
using HSS.System.V2.Domain.Models.Common;

namespace HSS.System.V2.Domain.Models.Medical;

public class TestRequired : BaseClass
{
    public string TestId { get; set; }
    public virtual Test Test { get; set; }
    public string TestName { get; set; }
    public string ClinicAppointmentId { get; set; }
    public virtual ClinicAppointment ClinicAppointment { get; set; }
    public string PatientNationalId { get; set; }
    public bool Used { get; set; }
} 