using Hangfire;

using HSS.System.V2.Domain.Helpers.Models;
using HSS.System.V2.Domain.Models.Medical;
using HSS.System.V2.Presentation.Controllers.Base;
using HSS.System.V2.Services.Contracts;
using HSS.System.V2.Services.DTOs.GeeneralDTOs;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HSS.System.V2.Presentation.Controllers
{
    public class GeneralController : CustomBaseController
    {
        private readonly IGeneralServices _generalServices;

        public GeneralController(IGeneralServices generalServices)
        {
            _generalServices = generalServices;
        }

        /// <summary>
        /// Retrieves a paginated list of radiology tests.
        /// </summary>
        /// <remarks>
        /// Sample request:
        /// <code>
        /// GET /radiology-tests?page=1&amp;size=10
        /// </code>
        /// Sample response:
        /// <code>
        /// {
        ///   "success": true,
        ///   "data": {
        ///     "items": [
        ///       {
        ///         "testId": "123",
        ///         "testName": "X-Ray",
        ///         "details": "Chest X-Ray"
        ///       }
        ///     ],
        ///     "totalCount": 1,
        ///     "currentPage": 1,
        ///     "pageSize": 10
        ///   },
        ///   "errors": []
        /// }
        /// </code>
        /// </remarks>
        /// <param name="page">The page number (default: 1).</param>
        /// <param name="size">The number of items per page (default: 10).</param>
        /// <returns>Paginated list of radiology tests.</returns>
        [ProducesResponseType(typeof(ApiResponse<PagedResult<TestDto<RadiologyTest>>>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<PagedResult<TestDto<RadiologyTest>>>), StatusCodes.Status400BadRequest)]
        [HttpGet(ApiRoutes.General.RadiologyTests)]
        public async Task<IActionResult> GetRadiologyTests([FromQuery] int page = 1, [FromQuery] int size = 10)
        {
            var pagination = new PaginationRequest { Page = page, Size = size };
            var result = await _generalServices.GetRadiologyTests(pagination);
            return GetResponse(result);
        }

        /// <summary>
        /// Retrieves a paginated list of medical lab tests.
        /// </summary>
        /// <remarks>
        /// Sample request:
        /// <code>
        /// GET /medical-lab-tests?page=1&amp;size=10
        /// </code>
        /// Sample response:
        /// <code>
        /// {
        ///   "success": true,
        ///   "data": {
        ///     "items": [
        ///       {
        ///         "testId": "456",
        ///         "testName": "Blood Test",
        ///         "details": "Complete Blood Count"
        ///       }
        ///     ],
        ///     "totalCount": 1,
        ///     "currentPage": 1,
        ///     "pageSize": 10
        ///   },
        ///   "errors": []
        /// }
        /// </code>
        /// </remarks>
        /// <param name="page">The page number (default: 1).</param>
        /// <param name="size">The number of items per page (default: 10).</param>
        /// <returns>Paginated list of medical lab tests.</returns>
        [ProducesResponseType(typeof(ApiResponse<PagedResult<TestDto<MedicalLabTest>>>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<PagedResult<TestDto<MedicalLabTest>>>), StatusCodes.Status400BadRequest)]
        [HttpGet(ApiRoutes.General.MedicalLabTests)]
        public async Task<IActionResult> GetMedicalLabTests([FromQuery] int page = 1, [FromQuery] int size = 10)
        {
            var pagination = new PaginationRequest { Page = page, Size = size };
            var result = await _generalServices.GetMedicalLabTests(pagination);
            return GetResponse(result);
        }
    }
}
