using HSS.System.V2.Domain.Models.Appointments;

using System.Linq.Expressions;

namespace HSS.System.V2.Domain.Models.Common;

public interface IQueueModel
{
    TimeSpan DepartmentStartAt { get; }
    TimeSpan DepartmentEndAt { get; }

    IEnumerable<Appointment> Appointments { get; }
}
