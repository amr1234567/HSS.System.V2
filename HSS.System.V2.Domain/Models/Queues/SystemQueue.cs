using HSS.System.V2.Domain.Models.Appointments;
using HSS.System.V2.Domain.Models.Common;

using System.ComponentModel.DataAnnotations.Schema;

namespace HSS.System.V2.Domain.Models.Queues;

public class SystemQueue : BaseClass
{
    public TimeSpan PeriodPerAppointment { get; set; }
    //public string? DepartmentId { set; get; }
    public string? ClinicId { set; get; }
    public string? RadiologyCeneterId { set; get; }
    public string? MedicalLabId { set; get; }

    [NotMapped]
    public IEnumerable<Appointment> Appointments => [];

    public string? DepartmentId
    {
        get
        {
            if (ClinicId != null)
                return ClinicId;
            if (RadiologyCeneterId != null)
                return RadiologyCeneterId;
            if (MedicalLabId != null)
                return MedicalLabId;
            return null;
        }
    }

}