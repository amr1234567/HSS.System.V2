using HSS.System.V2.Domain.Enums;
using HSS.System.V2.Domain.Helpers.Models;
using HSS.System.V2.Presentation.Controllers.Base;
using HSS.System.V2.Services.Contracts;
using HSS.System.V2.Services.DTOs;
using HSS.System.V2.Services.DTOs.AuthDTOs;

using Microsoft.AspNetCore.Mvc;

using System.Security.Claims;

namespace HSS.System.V2.Presentation.Controllers
{
    /// <summary>
    /// Controller for managing patient authentication operations.
    /// Handles patient registration, login, and account confirmation.
    /// </summary>
    [ApiExplorerSettings(GroupName = "PatientAPI")]
    public class AuthPatientController : CustomBaseController
    {
        private readonly IAuthService _authServiceRepo;

        /// <summary>
        /// Initializes a new instance of the AuthPatientController.
        /// </summary>
        /// <param name="authServiceRepo">The authentication service for handling business logic.</param>
        public AuthPatientController(IAuthService authServiceRepo)
        {
            _authServiceRepo = authServiceRepo;
        }

        /// <summary>
        /// Registers a new patient in the system.
        /// </summary>
        /// <remarks>
        /// Sample request:
        /// <code>
        /// POST /register
        /// {
        ///     "name": "string",
        ///     "email": "user@example.com",
        ///     "nationalId": "string",
        ///     "password": "string",
        ///     "confirmPassword": "string",
        ///     "gender": 0
        /// }
        /// </code>
        /// </remarks>
        /// <param name="dto">The patient registration details</param>
        /// <returns>Success status of the registration</returns>
        /// <response code="200">Successfully registered</response>
        /// <response code="400">Invalid registration data or account already exists</response>
        [ProducesResponseType(typeof(ApiResponse<bool>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<string>), StatusCodes.Status400BadRequest)]
        [HttpPost(ApiRoutes.Patient.Register)]
        public async Task<IActionResult> RegisterPatient([FromBody] PatientDto dto)
        {
            var result = await _authServiceRepo.RegisterPatient(dto);
            return GetResponseWithoutType(result);
        }

        /// <summary>
        /// Authenticates a patient and generates an access token.
        /// </summary>
        /// <remarks>
        /// Sample request:
        /// <code>
        /// POST /login
        /// {
        ///     "nationalId": "string",
        ///     "password": "string"
        /// }
        /// </code>
        /// </remarks>
        /// <param name="model">The login credentials</param>
        /// <returns>Authentication token and user details</returns>
        /// <response code="200">Successfully authenticated</response>
        /// <response code="400">Invalid credentials or account is locked/disabled</response>
        [ProducesResponseType(typeof(ApiResponse<UserDetails>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<UserDetails>), StatusCodes.Status400BadRequest)]
        [HttpPost(ApiRoutes.Patient.login)]
        public async Task<IActionResult> LoginPatient([FromBody] LoginModelDto model)
        {
            var result = await _authServiceRepo.LoginPatient(model);
            return GetResponse(result);
        }

        /// <summary>
        /// Confirms a patient's account using their national ID.
        /// </summary>
        /// <remarks>
        /// Sample request:
        /// <code>
        /// POST /confirm-account/{nationalId}
        /// </code>
        /// </remarks>
        /// <param name="nationalId">The national ID of the patient</param>
        /// <returns>Account confirmation details</returns>
        /// <response code="200">Account successfully confirmed</response>
        /// <response code="400">Invalid national ID or account already confirmed</response>
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
