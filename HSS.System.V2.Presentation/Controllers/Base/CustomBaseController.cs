using FluentResults;

using HSS.System.V2.Domain.Constants;
using HSS.System.V2.Domain.Helpers.Models;
using HSS.System.V2.Domain.ResultHelpers.Errors;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

using System.Security.Claims;

namespace HSS.System.V2.Presentation.Controllers.Base
{
    /// <summary>
    /// Base controller providing common response handling.
    /// </summary>
    public abstract class CustomBaseController : ControllerBase
    {
        /// <summary>
        /// Generates an appropriate IActionResult based on the result.
        /// </summary>
        /// <typeparam name="T">The type of the result value.</typeparam>
        /// <param name="result">The result object.</param>
        /// <returns>An IActionResult representing the result.</returns>
        [NonAction]
        protected IActionResult GetResponse<T>(Result<T> result)
        {
            if (result.IsFailed)
            {
                return GenerateErrorResponse<T>(result);
            }
            return Ok(ApiResponse<T>.Success(result.Value));
        }

        /// <summary>
        /// Generates an appropriate IActionResult based on the result.
        /// </summary>
        /// <param name="result">The result object.</param>
        /// <returns>An IActionResult representing the result.</returns>
        [NonAction]
        protected IActionResult GetResponseWithoutType(Result result)
        {
            if (result.IsFailed)
                return GenerateErrorResponse<object>(result);
            return Ok(ApiResponse<object?>.Success(null, "تمت العملية بنجاح ✅"));
        }

        /// <summary>
        /// Generates an appropriate IActionResult based on the result.
        /// </summary>
        /// <param name="result">The result object.</param>
        /// <returns>An IActionResult representing the result.</returns>
        private IActionResult GenerateErrorResponse<T>(ResultBase result)
        {
            var errorMessage = string.Join(", ", result.Errors.Select(e => e.Message));

            if (result.HasError<UnAuthorizedAccessError>())
                return Ok(ApiResponse<T>.Error(errorMessage, 401));

            if (result.HasError<UnSupportedBehaviorError>())
                return Ok(ApiResponse<T>.Error(errorMessage, 403));

            if (result.HasError<BadArgumentsError>() ||
                result.HasError<BadRequestError>() ||
                result.HasError<EntityNotExistsError>() ||
                result.HasError<EntityAlreadyExistsError>())
                return Ok(ApiResponse<T>.Error(errorMessage, 400));

            if (result.HasError<HttpRequestResponseError>() ||
                result.HasError<UnKnownError>() ||
                result.HasError<UnExpectedResponseError>() ||
                result.HasError<UnSupportedTypeError>())
                return Ok(ApiResponse<T>.Error(errorMessage, 500));

            if (result.HasError<EntityNotExistsError>())
                return Ok(ApiResponse<T>.Error(errorMessage, 404));

            return Ok(ApiResponse<T>.Error(errorMessage, 500));
        }

        protected string GetUserId()
        {
            return User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? string.Empty;
        }

        protected string GetHospitalId() => User.FindFirst(CustomClaimTypes.HospitalId)?.Value ?? string.Empty;

        protected string GetDepartmentId() => User.FindFirst(CustomClaimTypes.DepartmentItemId)?.Value ?? string.Empty;
    }
}
