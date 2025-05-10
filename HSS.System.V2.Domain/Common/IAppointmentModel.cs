using HSS.System.V2.Domain.Medical;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace HSS.System.V2.Domain.Common
{
    public interface IAppointmentModel<TQueue> where TQueue : IQueueModel
    {
        void SetQueue(TQueue? queue);
        TQueue? GetQueue();
    }

    public interface ITestableDepartment<TDept, ITest> where ITest : Test where TDept: IHospitalDepartmentItem
    {
        Expression<Func<TDept, IEnumerable<ITest>>> GetInclude();

        IEnumerable<ITest> DepartmentTests { get; }
    }
}
