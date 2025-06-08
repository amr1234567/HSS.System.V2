using FluentResults;

using HSS.System.V2.Domain.Models.Medical;

namespace HSS.System.V2.DataAccess.Contracts;

public interface IDiseaseRepository
{
    Task<Result<IEnumerable<Disease>>> GetAllDiseases(string? querySearch);
    Task<Result<Disease>> GetDiseaseById(string? diseaseId);
}