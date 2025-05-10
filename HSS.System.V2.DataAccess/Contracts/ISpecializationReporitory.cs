using FluentResults;

using HSS.System.V2.Domain.Helpers.Models;
using HSS.System.V2.Domain.Models.Medical;

namespace HSS.System.V2.DataAccess.Contracts
{
    public interface ISpecializationReporitory
    {
        Task<Result<IEnumerable<Specialization>>> GetAllAsync();
        Task<Result<PagedResult<Specialization>>> GetAllAsync(PaginationRequest pagination);
        Task<Result<IEnumerable<Specialization>>> GetAllInHospitalAsync(string hospitalId);
    }
}
