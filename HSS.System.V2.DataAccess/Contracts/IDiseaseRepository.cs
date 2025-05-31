using FluentResults;

using HSS.System.V2.Domain.Models.Medical;

namespace HSS.System.V2.DataAccess.Contracts
{
    public interface IDiseaseRepository
    {
        Task<Result<Disease>> GetDiseaseById(string diseaseId);
    }
}