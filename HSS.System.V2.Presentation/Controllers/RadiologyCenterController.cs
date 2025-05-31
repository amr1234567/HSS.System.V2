using HSS.System.V2.Domain.Attributes;
using HSS.System.V2.Domain.Enums;
using HSS.System.V2.Domain.Helpers.Models;
using HSS.System.V2.Presentation.Controllers.Base;
using HSS.System.V2.Presentation.Models;
using HSS.System.V2.Services.Contracts;
using HSS.System.V2.Services.DTOs.RadiologyCenterDTOs;
using HSS.System.V2.Services.DTOs.ReceptionDTOs;

using Microsoft.AspNetCore.Mvc;

namespace HSS.System.V2.Presentation.Controllers
{
    [AuthorizeByEnum(UserRole.RadiologyTester)]
    [ApiExplorerSettings(GroupName = "RadiologyAPI")]
    public class RadiologyCenterController : CustomBaseController
    {
        private readonly IRadiologyCenterServices _radiologyCenterServices;

        public RadiologyCenterController(IRadiologyCenterServices radiologyCenterServices)
        {
            _radiologyCenterServices = radiologyCenterServices;
        }

        /// <summary>
        /// Retrieves the queue for a specific radiology center.
        /// </summary>
        /// <remarks>
        /// Sample request:
        /// <code>
        /// GET /api/radiology/queue/{radiologyCenterId}?page=1&amp;pageSize=10
        /// </code>
        /// </remarks>
        /// <param name="radiologyCenterId">The ID of the radiology center.</param>
        /// <param name="page">The page number (default: 1).</param>
        /// <param name="pageSize">The number of items per page (default: 10).</param>
        /// <returns>Paginated list of appointments in the queue.</returns>
        [ProducesResponseType(typeof(ApiResponse<PagedResult<AppointmentDto>>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<PagedResult<AppointmentDto>>), StatusCodes.Status400BadRequest)]
        [HttpGet(ApiRoutes.RadiologyCenter.GetQueueForRadiologyCenter)]
        public async Task<IActionResult> GetQueueForRadiologyCenter([FromRoute] string radiologyCenterId, [FromQuery] int page = 1, [FromQuery] int pageSize = 10)
        {
            var result = await _radiologyCenterServices.GetQueueForRadiologyCenter(radiologyCenterId, page, pageSize);
            return GetResponse(result);
        }

        /// <summary>
        /// Retrieves the current radiology appointment details.
        /// </summary>
        /// <remarks>
        /// Sample request:
        /// <code>
        /// GET /api/radiology/appointments/{appointmentId}
        /// </code>
        /// </remarks>
        /// <param name="appointmentId">The ID of the appointment.</param>
        /// <returns>The radiology appointment details.</returns>
        [ProducesResponseType(typeof(ApiResponse<RadiologyAppointmentModel>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<RadiologyAppointmentModel>), StatusCodes.Status400BadRequest)]
        [HttpGet(ApiRoutes.RadiologyCenter.CurrentRadiologyAppointment)]
        public async Task<IActionResult> GetCurrentRadiologyAppointment([FromRoute] string appointmentId)
        {
            var result = await _radiologyCenterServices.GetCurrentRadiologyAppointment(appointmentId);
            return GetResponse(result);
        }

        /// <summary>
        /// Adds radiology images to an appointment.
        /// </summary>
        /// <remarks>
        /// Sample request:
        /// <code>
        /// POST /api/radiology/appointments/{appointmentId}/result
        /// </code>
        /// </remarks>
        /// <param name="appointmentId">The ID of the appointment.</param>
        /// <param name="model">The radiology appointment result model containing images.</param>
        /// <returns>Confirmation of image upload.</returns>
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
        [HttpPost(ApiRoutes.RadiologyCenter.RadiologyAppointmentResult)]
        public async Task<IActionResult> AddRadiologyImagesToAppointment([FromRoute] string appointmentId, [FromForm] RadiologyAppointmentResultModel model)
        {
            var result = await _radiologyCenterServices.AddRadiologyImagesToAppointment(appointmentId, model.Result);
            return GetResponseWithoutType(result);
        }

        /// <summary>
        /// Ends a radiology appointment by updating its status to completed.
        /// </summary>
        /// <remarks>
        /// Sample request:
        /// <code>
        /// PUT /api/radiology/appointments/{appointmentId}/end
        /// </code>
        /// </remarks>
        /// <param name="appointmentId">The ID of the appointment to end.</param>
        /// <returns>Confirmation of appointment completion.</returns>
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
        [HttpPut(ApiRoutes.RadiologyCenter.EndAppointment)]
        public async Task<IActionResult> EndAppointment([FromRoute] string appointmentId)
        {
            var result = await _radiologyCenterServices.EndAppointmentAsync(appointmentId);
            return GetResponseWithoutType(result);
        }
    }
}
