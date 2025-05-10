using FluentResults;

using HSS.System.V2.DataAccess.Contracts;
using HSS.System.V2.Domain.Helpers.Models;
using HSS.System.V2.Domain.Models.Medical;

namespace HSS.System.V2.DataAccess.Repositories
{
    public class SpecializationReporitory : ISpecializationReporitory
    {
        public Task<Result<IEnumerable<Specialization>>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public Task<Result<PagedResult<Specialization>>> GetAllAsync(PaginationRequest pagination)
        {
            throw new NotImplementedException();
        }

        public Task<Result<IEnumerable<Specialization>>> GetAllInHospitalAsync(string hospitalId)
        {
            throw new NotImplementedException();
        }
    }
}
