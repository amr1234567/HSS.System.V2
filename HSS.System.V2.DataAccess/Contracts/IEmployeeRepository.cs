using FluentResults;

using HSS.System.V2.Domain.Enums;
using HSS.System.V2.Domain.Models.People;

namespace HSS.System.V2.DataAccess.Contracts
{
    public interface IEmployeeRepository
    {
        Task<Result<Employee?>> GetEmployeeByNationalId(string nationalId);
        Task<Result<Employee?>> GetEmployeeById(string id);
        Task<Result> CreateLoginActivity(Employee employee, ActivityType activityType = ActivityType.Login);
    }
}
