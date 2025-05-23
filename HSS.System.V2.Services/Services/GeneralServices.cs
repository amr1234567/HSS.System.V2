using FluentResults;

using HSS.System.V2.DataAccess.Contracts;
using HSS.System.V2.Domain.Helpers.Models;
using HSS.System.V2.Domain.Models.Medical;
using HSS.System.V2.Services.Contracts;
using HSS.System.V2.Services.DTOs.GeeneralDTOs;

namespace HSS.System.V2.Services.Services
{
    public class GeneralServices : IGeneralServices
    {
        private readonly ITestsRepository _testsRepository;

        public GeneralServices(ITestsRepository testsRepository)
        {
            _testsRepository = testsRepository;
        }

        public async Task<Result<PagedResult<TestDto<RadiologyTest>>>> GetRadiologyTests(PaginationRequest request)
        {
            var testsResult = await _testsRepository.GetAllTestsAsync<RadiologyTest>(request);
            if (testsResult.IsFailed || testsResult.Value == null)
                return Result.Fail(testsResult.Errors);

            var tests = testsResult.Value.Items
                .Select(t => new TestDto<RadiologyTest>().MapFromModel(t))
                .ToList();

            var pagedResult = new PagedResult<TestDto<RadiologyTest>>(tests, testsResult.Value.TotalCount, testsResult.Value.CurrentPage, testsResult.Value.PageSize);
            return Result.Ok(pagedResult);
        }

        public async Task<Result<PagedResult<TestDto<MedicalLabTest>>>> GetMedicalLabTests(PaginationRequest request)
        {
            var testsResult = await _testsRepository.GetAllTestsAsync<MedicalLabTest>(request);
            if (testsResult.IsFailed || testsResult.Value == null)
                return Result.Fail(testsResult.Errors);

            var tests = testsResult.Value.Items
                .Select(t => new TestDto<MedicalLabTest>().MapFromModel(t))
                .ToList();

            var pagedResult = new PagedResult<TestDto<MedicalLabTest>>(tests, testsResult.Value.TotalCount, testsResult.Value.CurrentPage, testsResult.Value.PageSize);
            return Result.Ok(pagedResult);
        }
    }
}
