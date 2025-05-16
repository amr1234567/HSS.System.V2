using HSS.System.V2.Domain.Models.Appointments;
using HSS.System.V2.Domain.Models.Common;

namespace HSS.System.V2.Domain.Models.Queues;

public class SystemQueue : BaseClass, IQueueModel
{
    public TimeSpan PeriodPerAppointment { get; set; }
    public string? DepartmentId { set; get; }

    public IEnumerable<Appointment> Appointments => [];

    public TimeSpan DepartmentStartAt => TimeSpan.MinValue;
    public TimeSpan DepartmentEndAt => TimeSpan.MaxValue;

}