using HSS.System.V2.Domain.Enums;
using HSS.System.V2.Domain.Helpers.Models;
using HSS.System.V2.Presentation.Controllers.Base;
using HSS.System.V2.Services.Contracts;
using HSS.System.V2.Services.DTOs.AuthDTOs;
using HSS.System.V2.Services.Services;

using Microsoft.AspNetCore.Mvc;

using System.Security.Claims;

namespace HSS.System.V2.Presentation.Controllers
{
    [ApiExplorerSettings(GroupName = "PatientAPI")]
    public class AuthPatientController : CustomBaseController
    {
        private readonly IAuthService _authServiceRepo;

        public AuthPatientController(IAuthService authServiceRepo)
        {
            _authServiceRepo = authServiceRepo;
        }

        [ProducesResponseType(typeof(ApiResponse<bool>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<string>), StatusCodes.Status400BadRequest)]
        [HttpPost(ApiRoutes.Patient.Register)]
        public async Task<IActionResult> RegisterPatient([FromBody] PatientDto dto)
        {
            var result = await _authServiceRepo.RegisterPatient(dto);

            return GetResponseWithoutType(result);
        }

        [ProducesResponseType(typeof(ApiResponse<UserDetails>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<UserDetails>), StatusCodes.Status400BadRequest)]
        [HttpPost(ApiRoutes.Patient.login)]
        public async Task<IActionResult> LoginPatient([FromBody] LoginModelDto model)
        {
            var result = await _authServiceRepo.LoginPatient(model);

            return GetResponse(result);
        }

        [ProducesResponseType(typeof(ApiResponse<ConfirmPatientAccountDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<ConfirmPatientAccountDto>), StatusCodes.Status400BadRequest)]
        [HttpPost(ApiRoutes.Patient.ConfirmAccount)]
        [NonAction]
        public async Task<IActionResult> ConfirmPatientAccount([FromRoute] string nationalId)
        {
            var result = await _authServiceRepo.ConfirmPatientAccount(nationalId);
            return GetResponse(result);
        }
    }
}
