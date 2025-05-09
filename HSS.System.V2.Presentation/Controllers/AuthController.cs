using HSS.System.V2.Domain.Enums;
using HSS.System.V2.Domain.Helpers.Models;
using HSS.System.V2.Presentation.Controllers.Base;
using HSS.System.V2.Services.Contracts;
using HSS.System.V2.Services.DTOs.AuthDTOs;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using System.Security.Claims;

namespace HSS.System.V2.Presentation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : CustomBaseController
    {
        private readonly IAuthService _authServiceRepo;

        public AuthController(IAuthService authServiceRepo)
        {
            _authServiceRepo = authServiceRepo;
        }

        [ProducesResponseType(typeof(ApiResponse<bool>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<string>), StatusCodes.Status400BadRequest)]
        [HttpPost("register-patient")]
        public async Task<IActionResult> RegisterPatient(PatientDto dto)
        {
            var result = await _authServiceRepo.RegisterPatient(dto);

            return GetResponseWithoutType(result);
        }

        [ProducesResponseType(typeof(ApiResponse<TokenModel>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<TokenModel>), StatusCodes.Status400BadRequest)]
        [HttpPost("login-patient")]
        public async Task<IActionResult> LoginPatient([FromBody]LoginModelDto model)
        {
            var result = await _authServiceRepo.LoginPatient(model);

            return GetResponse(result);
        }

        [ProducesResponseType(typeof(ApiResponse<TokenModel>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<TokenModel>), StatusCodes.Status400BadRequest)]
        [HttpPost("login-employee")]
        public async Task<IActionResult> LoginEmployee([FromBody]LoginModelDto model)
        {
            var result = await _authServiceRepo.LoginEmployee(model);

            return GetResponse(result);
        }

        [ProducesResponseType(typeof(ApiResponse<TokenModel>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<TokenModel>), StatusCodes.Status400BadRequest)]
        [HttpPost("refresh-token-patient")]
        public async Task<IActionResult> RefreshTokenForPatient([FromBody] RefreshTokenModelDto model)
        {
            var result = await _authServiceRepo.RefreshTheTokenForPatient(model.RefreshToken, model.AccessToken);

            return GetResponse(result);
        }

        [ProducesResponseType(typeof(ApiResponse<TokenModel>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<TokenModel>), StatusCodes.Status400BadRequest)]
        [HttpPost("refresh-token-employee")]
        public async Task<IActionResult> RefreshTokenForEmployee([FromBody] RefreshTokenModelDto model)
        {
            var result = await _authServiceRepo.RefreshTheTokenForEmployee(model.RefreshToken, model.AccessToken);

            return GetResponse(result);
        }

        [ProducesResponseType(typeof(ApiResponse<TokenModel>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<TokenModel>), StatusCodes.Status400BadRequest)]
        [HttpPost("logout")]
        [Authorize]
        public async Task<IActionResult> Logout([FromBody] LogoutModelDto model)
        {
            var roleInString = User.FindAll(ClaimTypes.Role).Select(c => c.Value).FirstOrDefault();
            var role = Enum.Parse<UserRole>(roleInString);
            if (role == UserRole.Patient)
            {
                var result = await _authServiceRepo.LogoutPatient(model.RefreshToken);
                return GetResponseWithoutType(result);
            }
            else
            {
                var result = await _authServiceRepo.LogoutEmployee(model.RefreshToken);
                return GetResponseWithoutType(result);
            }
        }
    }
}
