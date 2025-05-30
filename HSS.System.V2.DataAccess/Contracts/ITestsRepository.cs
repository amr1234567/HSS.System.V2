using FluentResults;

using HSS.System.V2.Domain.Helpers.Models;
using HSS.System.V2.Domain.Models.Common;
using HSS.System.V2.Domain.Models.Facilities;
using HSS.System.V2.Domain.Models.Medical;

namespace HSS.System.V2.DataAccess.Contracts
{
    public interface ITestsRepository
    {
        Task<Result> CreateTestAsync(Test model);
        Task<Result> UpdateTestAsync(Test model);
        Task<Result> DeleteTestAsync(string id);
        Task<Result<PagedResult<T>>> GetAllTestsAsync<T>(PaginationRequest pagination) where T: Test;
        Task<Result<Test>> GetTestByIdAsync(string id);
        Task<Result> IsTestInHospital<TDept, ITest>(string testId, string hospitalId)
            where ITest : Test 
            where TDept : BaseClass, IHospitalDepartmentItem, ITestableDepartment<TDept, ITest>;
        Task<Result<PagedResult<TTest>>> GetAllTestsInHospitalAsync<TTest>(string hospitalId, int size = 10, int page = 1) where TTest : Test;
        Task<Result<IEnumerable<Hospital>>> GetAllHospitalsDoTest<TTest>(string testId) where TTest : Test;
        Task<Result<bool>> IsRadiologyCenterDoTestAsync(string radiologyCenterId, string testId);
        Result<bool> IsRadiologyCenterDoTest(string radiologyCenterId, string testId);
        Task<Result<bool>> IsMedicalLabDoTestAsync(string labId, string testId);
        Result<bool> IsMedicalLabDoTest(string labId, string testId);
        Task<Result<PagedResult<T>>> GetAllTestsInHospital<T>(string hospitalId, int page, int size) where T : Test;
    }
}
