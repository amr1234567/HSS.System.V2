using FluentResults;

using HSS.System.V2.Domain.Common;
using HSS.System.V2.Domain.Helpers.Models;
using HSS.System.V2.Domain.Medical;

namespace HSS.System.V2.DataAccess.Contracts
{
    public interface ITestsRepository
    {
        Task<Result> CreateTestAsync(Test model);
        Task<Result> UpdateRadiologyTestAsync(Test model);
        Task<Result> DeleteRadiologyTestAsync(string id);
        Task<Result<PagedResult<T>>> GetAllTestsAsync<T>(PaginationRequest pagination) where T: Test;
        Task<Result<Test>> GetTestByIdAsync(string id);
        Task<Result> IsTestInHospital<TDept, ITest>(string testId, string hospitalId)
            where ITest : Test 
            where TDept : BaseClass, IHospitalDepartmentItem, ITestableDepartment<TDept, ITest>;
        Task<Result<PagedResult<Test>>> GetAllTestsInHospitalAsync(string hospitalId, int size = 10, int page = 1);
    }
}
