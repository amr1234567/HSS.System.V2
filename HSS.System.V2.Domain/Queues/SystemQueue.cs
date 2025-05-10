using HSS.System.V2.Domain.Appointments;
using HSS.System.V2.Domain.Common;
using HSS.System.V2.Domain.Facilities;

namespace HSS.System.V2.Domain.Queues;

public class SystemQueue : BaseClass, IQueueModel
{
    public TimeSpan PeriodPerAppointment { get; set; }
    public string DepartmentId => string.Empty;
    public IEnumerable<Appointment> Appointments => [];

    public TimeSpan DepartmentStartAt => TimeSpan.MinValue;
    public TimeSpan DepartmentEndAt => TimeSpan.MaxValue;

}