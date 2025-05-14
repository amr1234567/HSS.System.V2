using HSS.System.V2.Domain.Models.Appointments;

using System.Linq.Expressions;

namespace HSS.System.V2.Domain.Models.Common
{

    public interface IAppoitnmentIncludeStrategy
    {
        Expression<Func<Appointment, object>> GetIncludeDepartment();
        Expression<Func<Appointment, object>> GetIncludeEmployee();
    }
}
