using HSS.System.V2.Domain.Common;
using HSS.System.V2.Domain.Appointments;

namespace HSS.System.V2.Domain.Medical;

public class TestRequired : BaseClass
{
    public string TestId { get; set; }
    public Test Test { get; set; }
    public string TestName { get; set; }
    public string ClinicAppointmentId { get; set; }
    public virtual ClinicAppointment ClinicAppointment { get; set; }
    public string PatientNationalId { get; set; }
    public bool Used { get; set; }
} 