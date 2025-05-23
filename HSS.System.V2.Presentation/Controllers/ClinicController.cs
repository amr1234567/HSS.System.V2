using HSS.System.V2.Domain.Attributes;
using HSS.System.V2.Domain.Enums;
using HSS.System.V2.Domain.Helpers.Models;
using HSS.System.V2.Presentation.Controllers.Base;
using HSS.System.V2.Services.Contracts;
using HSS.System.V2.Services.DTOs.ClinicDTOs;

using Microsoft.AspNetCore.Mvc;

namespace HSS.System.V2.Presentation.Controllers
{
    [AuthorizeByEnum(UserRole.Doctor)]
    [ApiExplorerSettings(GroupName = "ClinicAPI")]
    public class ClinicController : CustomBaseController
    {
        private readonly IClinicServices _clinicServices;

        public ClinicController(IClinicServices clinicServices)
        {
            _clinicServices = clinicServices;
        }

        /// <summary>
        /// Retrieves the details of a specific appointment by its ID.
        /// </summary>
        /// <remarks>
        /// Sample request:
        /// <code>
        /// GET /api/Clinic/appointments/{appointmentId}
        /// </code>
        /// </remarks>
        /// <param name="appointmentId">The ID of the appointment to retrieve.</param>
        /// <returns>The appointment details.</returns>
        [ProducesResponseType(typeof(ApiResponse<ClinicAppointmentDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<ClinicAppointmentDto>), StatusCodes.Status400BadRequest)]
        [HttpGet("clinic/appointments/{appointmentId}")]
        public async Task<IActionResult> GetAppointmentDetails(string appointmentId)
        {
            var result = await _clinicServices.GetAppointmentDetailsById(appointmentId);
            return GetResponse(result);
        }

        /// <summary>
        /// Retrieves the details of a specific ticket by its ID.
        /// </summary>
        /// <remarks>
        /// Sample request:
        /// <code>
        /// GET /api/Clinic/tickets/{ticketId}
        /// </code>
        /// </remarks>
        /// <param name="ticketId">The ID of the ticket to retrieve.</param>
        /// <returns>The ticket details with associated appointments.</returns>
        [ProducesResponseType(typeof(ApiResponse<AppointmentTicketDetailsDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<AppointmentTicketDetailsDto>), StatusCodes.Status400BadRequest)]
        [HttpGet("clinic/tickets/{ticketId}")]
        public async Task<IActionResult> GetCurrentTicketDetails(string ticketId)
        {
            var result = await _clinicServices.GetCurrentTicketDetails(ticketId);
            return GetResponse(result);
        }

        /// <summary>
        /// Retrieves paginated medical histories for a specific patient.
        /// </summary>
        /// <remarks>
        /// Sample request:
        /// <code>
        /// GET /api/Clinic/patients/{patientId}/medical-histories?page=1&amp;size=10
        /// </code>
        /// </remarks>
        /// <param name="patientId">The ID of the patient.</param>
        /// <param name="page">The page number (default: 1).</param>
        /// <param name="size">The number of items per page (default: 10).</param>
        /// <returns>Paginated list of medical histories.</returns>
        [ProducesResponseType(typeof(ApiResponse<PagedResult<MinMedicalHistoryDto>>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<PagedResult<MinMedicalHistoryDto>>), StatusCodes.Status400BadRequest)]
        [HttpGet("clinic/patients/{patientId}/medical-histories")]
        public async Task<IActionResult> GetMedicalHistoriesForPatient(string patientId, [FromQuery] int page = 1, [FromQuery] int size = 10)
        {
            var pagination = new PaginationRequest { Page = page, Size = size };
            var result = await _clinicServices.GetMedicalHistoriesForPatient(patientId, pagination);
            return GetResponse(result);
        }

        /// <summary>
        /// Retrieves all medical histories for a specific patient.
        /// </summary>
        /// <remarks>
        /// Sample request:
        /// <code>
        /// GET /api/Clinic/patients/{patientId}/medical-histories/all
        /// </code>
        /// </remarks>
        /// <param name="patientId">The ID of the patient.</param>
        /// <returns>List of all medical histories.</returns>
        [ProducesResponseType(typeof(ApiResponse<IEnumerable<MinMedicalHistoryDto>>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<IEnumerable<MinMedicalHistoryDto>>), StatusCodes.Status400BadRequest)]
        [HttpGet("clinic/patients/{patientId}/medical-histories/all")]
        public async Task<IActionResult> GetAllMedicalHistoriesForPatient(string patientId)
        {
            var result = await _clinicServices.GetMedicalHistoriesForPatient(patientId);
            return GetResponse(result);
        }

        /// <summary>
        /// Retrieves the details of a specific medical history by its ID.
        /// </summary>
        /// <remarks>
        /// Sample request:
        /// <code>
        /// GET /api/Clinic/medical-histories/{medicalHistoryId}
        /// </code>
        /// </remarks>
        /// <param name="medicalHistoryId">The ID of the medical history.</param>
        /// <returns>The medical history details.</returns>
        [ProducesResponseType(typeof(ApiResponse<MidecalHistoryDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<MidecalHistoryDto>), StatusCodes.Status400BadRequest)]
        [HttpGet("clinic/medical-histories/{medicalHistoryId}")]
        public async Task<IActionResult> GetMedicalHistoryDetails(string medicalHistoryId)
        {
            var result = await _clinicServices.GetMedicalHistoryDetails(medicalHistoryId);
            return GetResponse(result);
        }

        /// <summary>
        /// Submits a clinic result for an appointment.
        /// </summary>
        /// <remarks>
        /// Sample request:
        /// <code>
        /// POST /api/Clinic/results
        /// {
        ///   "appointmentId": "123",
        ///   "diseaseId": "456",
        ///   "diagnosis": "Flu",
        ///   "reExaminationNeeded": true,
        ///   "testsRequired": [
        ///     {
        ///       "testId": "789"
        ///     }
        ///   ],
        ///   "prescription": {
        ///     "items": [
        ///       {
        ///         "medicineId": "101",
        ///         "dosage": "1 tablet daily"
        ///       }
        ///     ]
        ///   }
        /// }
        /// </code>
        /// </remarks>
        /// <param name="request">The clinic result details.</param>
        /// <returns>Confirmation of submission.</returns>
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
        [HttpPost("clinic/results")]
        public async Task<IActionResult> SubmitClinicResult([FromBody] ClinicResultRequestDto request)
        {
            var result = await _clinicServices.SubmitClinicResultAsync(request);
            return GetResponseWithoutType(result);
        }

        /// <summary>
        /// Ends an appointment by updating its status to completed.
        /// </summary>
        /// <remarks>
        /// Sample request:
        /// <code>
        /// PUT /api/Clinic/appointments/{appointmentId}/end
        /// </code>
        /// </remarks>
        /// <param name="appointmentId">The ID of the appointment to end.</param>
        /// <returns>Confirmation of appointment completion.</returns>
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
        [HttpPut("clinic/appointments/{appointmentId}/end")]
        public async Task<IActionResult> EndAppointment(string appointmentId)
        {
            var result = await _clinicServices.EndAppointmentAsync(appointmentId);
            return GetResponseWithoutType(result);
        }
    }
}
