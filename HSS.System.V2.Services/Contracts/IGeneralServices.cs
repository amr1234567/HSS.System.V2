using FluentResults;

using HSS.System.V2.Domain.Helpers.Models;
using HSS.System.V2.Domain.Models.Medical;
using HSS.System.V2.Services.DTOs.GeeneralDTOs;

namespace HSS.System.V2.Services.Contracts
{
    public interface IGeneralServices
    {
        Task<Result<PagedResult<TestDto<MedicalLabTest>>>> GetMedicalLabTests(PaginationRequest request);
        Task<Result<PagedResult<TestDto<RadiologyTest>>>> GetRadiologyTests(PaginationRequest request);
    }
}