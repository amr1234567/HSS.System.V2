using FluentResults;

using HSS.System.V2.Domain.Models.Medical;

namespace HSS.System.V2.DataAccess.Contracts
{
    public interface ITestRequiredRepository
    {
        Task<Result> CreateTestRequiredAsync(TestRequired model);
        Task<Result> UpdateTestRequiredAsync(TestRequired model);
        Task<Result> DeleteTestRequiredAsync(string id);
        Task<Result<IEnumerable<TestRequired>>> GetNotUsedTestsRequiredAvailableInTicket(string ticketId);
        Task<Result<IEnumerable<TestRequired>>> GetDoneTestsRequiredInTicket(string ticketId);
        Task<Result<IEnumerable<TestRequired>>> GetAllTestsRequiredAvailableInTicket(string ticketId);
        Task<Result<IEnumerable<TestRequired>>> GetAllTestsRequiredAvailableForUser(string userId);
    }
}
