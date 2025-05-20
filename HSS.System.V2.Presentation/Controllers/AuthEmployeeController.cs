using HSS.System.V2.Domain.Attributes;
using HSS.System.V2.Domain.Enums;
using HSS.System.V2.Domain.Helpers.Models;
using HSS.System.V2.Presentation.Controllers.Base;
using HSS.System.V2.Services.Contracts;
using HSS.System.V2.Services.DTOs.AuthDTOs;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HSS.System.V2.Presentation.Controllers
{
    /// <summary>
    /// Controller for managing employee authentication operations.
    /// Handles employee login and logout functionality.
    /// </summary>
    [ApiExplorerSettings(GroupName = "EmployeeAuthAPI")]
    public class AuthEmployeeController : CustomBaseController
    {
        private readonly IAuthService _authServiceRepo;

        /// <summary>
        /// Initializes a new instance of the AuthEmployeeController.
        /// </summary>
        /// <param name="authServiceRepo">The authentication service for handling business logic.</param>
        public AuthEmployeeController(IAuthService authServiceRepo)
        {
            _authServiceRepo = authServiceRepo;
        }

        /// <summary>
        /// Authenticates an employee and generates an access token.
        /// </summary>
        /// <remarks>
        /// Sample request:
        /// <code>
        /// POST /login-employee
        /// {
        ///     "username": "string",
        ///     "password": "string"
        /// }
        /// </code>
        /// </remarks>
        /// <param name="model">The login credentials</param>
        /// <returns>Authentication token and user details</returns>
        /// <response code="200">Successfully authenticated</response>
        /// <response code="400">Invalid credentials or account is locked/disabled</response>
        [ProducesResponseType(typeof(ApiResponse<TokenModel>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<TokenModel>), StatusCodes.Status400BadRequest)]
        [HttpPost("login-employee")]
        public async Task<IActionResult> LoginEmployee([FromBody] LoginModelDto model)
        {
            var result = await _authServiceRepo.LoginEmployee(model);
            return GetResponse(result);
        }

        /// <summary>
        /// Logs out the currently authenticated employee.
        /// </summary>
        /// <remarks>
        /// Sample request:
        /// <code>
        /// POST /logout
        /// </code>
        /// </remarks>
        /// <returns>Success status of the logout operation</returns>
        /// <response code="200">Successfully logged out</response>
        /// <response code="400">Error during logout process</response>
        [ProducesResponseType(typeof(ApiResponse<TokenModel>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<TokenModel>), StatusCodes.Status400BadRequest)]
        [HttpPost("logout")]
        [AuthorizeByEnum(UserRole.MedicalLabTester, UserRole.Receptionist, UserRole.RadiologyTester, UserRole.Pharmacist, UserRole.Doctor)]
        public async Task<IActionResult> Logout()
        {
            var result = await _authServiceRepo.LogoutEmployee();
            return GetResponseWithoutType(result);
        }
    }
}
