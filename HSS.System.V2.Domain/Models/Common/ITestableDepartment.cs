using HSS.System.V2.Domain.Models.Medical;

using System.Linq.Expressions;

namespace HSS.System.V2.Domain.Models.Common
{
    public interface ITestableDepartment<TDept, ITest> where ITest : Test where TDept: IHospitalDepartmentItem
    {
        Expression<Func<TDept, IEnumerable<ITest>>> GetInclude();

        IEnumerable<ITest> DepartmentTests { get; }
    }
}
