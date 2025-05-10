using HSS.System.V2.Domain.Appointments;
using HSS.System.V2.Domain.Queues;

using System.Linq.Expressions;

namespace HSS.System.V2.Domain.Common;

public interface IQueueModel
{
    string DepartmentId { get; }
    TimeSpan DepartmentStartAt { get; }
    TimeSpan DepartmentEndAt { get; }

    IEnumerable<Appointment> Appointments { get; }
}
