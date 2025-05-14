using HSS.System.V2.Domain.Helpers.Models;
using HSS.System.V2.Presentation.Controllers.Base;
using HSS.System.V2.Services.Contracts;
using HSS.System.V2.Services.DTOs.AuthDTOs;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HSS.System.V2.Presentation.Controllers
{
    [ApiExplorerSettings(GroupName = "EmployeeAuthAPI")]
    public class AuthEmployeeController : CustomBaseController
    {
        private readonly IAuthService _authServiceRepo;

        public AuthEmployeeController(IAuthService authServiceRepo)
        {
            _authServiceRepo = authServiceRepo;
        }

        [ProducesResponseType(typeof(ApiResponse<TokenModel>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<TokenModel>), StatusCodes.Status400BadRequest)]
        [HttpPost("login-employee")]
        public async Task<IActionResult> LoginEmployee([FromBody] LoginModelDto model)
        {
            var result = await _authServiceRepo.LoginEmployee(model);

            return GetResponse(result);
        }

        [ProducesResponseType(typeof(ApiResponse<TokenModel>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<TokenModel>), StatusCodes.Status400BadRequest)]
        [HttpPost("logout")]
        [Authorize]
        public async Task<IActionResult> Logout()
        {
            var result = await _authServiceRepo.LogoutEmployee();
            return GetResponseWithoutType(result);
        }
    }
}
