using HSS.System.V2.Presentation.Controllers.Base;
using HSS.System.V2.Services.Contracts;
using HSS.System.V2.Services.DTOs.ReceptionDTOs;
using HSS.System.V2.Domain.Models.Requests;
using HSS.System.V2.Domain.Constants;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using HSS.System.V2.Domain.Attributes;
using HSS.System.V2.Domain.Enums;
using System.Security.Claims;
using HSS.System.V2.Domain.Helpers.Models;
using HSS.System.V2.Domain.DTOs;
using HSS.System.V2.Services.DTOs.GeeneralDTOs;
using HSS.System.V2.Domain.Models.Medical;

namespace HSS.System.V2.Presentation.Controllers
{
    [AuthorizeByEnum(UserRole.Receptionist, UserRole.Doctor)]
    [ApiExplorerSettings(GroupName = "RecpetionAPI")]
    public class ReceptionController : CustomBaseController
    {
        private readonly IReceptionServices _receptionServices;

        public ReceptionController(IReceptionServices receptionServices)
        {
            _receptionServices = receptionServices;
        }

        /// <summary>
        /// Get Hospital departments details
        /// </summary>
        /// <remarks>
        /// Sample request:
        /// <code>
        /// GET /api/reception/hospital-departments
        /// </code>
        /// </remarks>
        /// <response code="200">Successfully retrieved departments</response>
        /// <response code="400">Bad request error</response>
        [ProducesResponseType(typeof(ApiResponse<HospitalDepartments>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<string>), StatusCodes.Status400BadRequest)]
        [HttpGet(ApiRoutes.Reception.GetAllHospitalDepartments)]
        public async Task<IActionResult> GetAllHospitalDepartmentsInHospital()
        {
            var hospitalId = GetHospitalIdFromClaims();
            var result = await _receptionServices.GetAllHospitalDepartmentsInHospital(hospitalId);
            return GetResponse(result);
        }

        /// <summary>
        /// Get all specializations in the hospital
        /// </summary>
        /// <remarks>
        /// Sample request:
        /// <code>
        /// GET /api/reception/specializations?page=1&amp;size=10
        /// </code>
        /// </remarks>
        /// <param name="pagination">Pagination parameters</param>
        /// <response code="200">Successfully retrieved specializations</response>
        /// <response code="400">Bad request error</response>
        [ProducesResponseType(typeof(ApiResponse<PagedResult<SpecializationDto>>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<string>), StatusCodes.Status400BadRequest)]
        [HttpGet(ApiRoutes.Reception.GetAllSpecializations)]
        public async Task<IActionResult> GetAllSpecializationsInHospital([FromQuery] PaginationRequest pagination)
        {
            var hospitalId = GetHospitalIdFromClaims();
            var result = await _receptionServices.GetAllSpecializationsInHospital(hospitalId, pagination.Page, pagination.Size);
            return GetResponse(result);
        }

        /// <summary>
        /// Get all clinics for a specific specialization
        /// </summary>
        /// <remarks>
        /// Sample request:
        /// <code>
        /// GET /api/reception/clinics/{specializationId}?page=1&amp;size=10
        /// </code>
        /// </remarks>
        /// <param name="specializationId">The specialization ID</param>
        /// <param name="pagination">Pagination parameters</param>
        /// <response code="200">Successfully retrieved clinics</response>
        /// <response code="400">Bad request error</response>
        [ProducesResponseType(typeof(ApiResponse<PagedResult<ClinicDto>>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<string>), StatusCodes.Status400BadRequest)]
        [HttpGet(ApiRoutes.Reception.GetAllClinics)]
        public async Task<IActionResult> GetAllClinics([FromRoute] string specializationId, [FromQuery] PaginationRequest pagination)
        {
            var hospitalId = GetHospitalIdFromClaims();
            var result = await _receptionServices.GetAllClinics(specializationId, hospitalId, pagination.Page, pagination.Size);
            return GetResponse(result);
        }


        /// <summary>
        /// Get all radiology tests available in the hospital
        /// </summary>
        /// <remarks>
        /// Sample request:
        /// <code>
        /// GET /api/reception/radiology/tests?page=1&amp;size=10
        /// </code>
        /// </remarks>
        /// <param name="pagination">Pagination parameters</param>
        [ProducesResponseType(typeof(ApiResponse<PagedResult<TestDto<RadiologyTest>>>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<string>), StatusCodes.Status400BadRequest)]
        [HttpGet(ApiRoutes.Reception.GetAllRadiologyTestsAvailableInHospital)]
        public async Task<IActionResult> GetAllRadiologyTestsInHospital([FromQuery] PaginationRequest pagination)
        {
            var hospitalId = GetHospitalIdFromClaims();
            var result = await _receptionServices.GetAllRadiologyTestsInHospital(hospitalId, pagination.Page, pagination.Size);
            return GetResponse(result);
        }

        /// <summary>
        /// Get all medical lab tests in the hospital
        /// </summary>
        /// <remarks>
        /// Sample request:
        /// <code>
        /// GET /api/reception/medical-lab/tests?page=1&amp;size=10
        /// </code>
        /// </remarks>
        /// <param name="pagination">Pagination parameters</param>
        [ProducesResponseType(typeof(ApiResponse<PagedResult<TestDto<MedicalLabTest>>>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<string>), StatusCodes.Status400BadRequest)]
        [HttpGet(ApiRoutes.Reception.GetAllMedicalLabTestsAvailableInHospital)]
        public async Task<IActionResult> GetAllMedicalLabTestsInHospital([FromQuery] PaginationRequest pagination)
        {
            var hospitalId = GetHospitalIdFromClaims();
            var result = await _receptionServices.GetAllMedicalLabTestsTestsInHospital(hospitalId, pagination.Page, pagination.Size);
            return GetResponse(result);
        }

        /// <summary>
        /// Get all radiology centers
        /// </summary>
        /// <remarks>
        /// Sample request:
        /// <code>
        /// GET /api/reception/radiology-centers?page=1&amp;size=10
        /// </code>
        /// </remarks>
        /// <param name="pagination">Pagination parameters</param>
        /// <response code="200">Successfully retrieved radiology centers</response>
        /// <response code="400">Bad request error</response>
        [ProducesResponseType(typeof(ApiResponse<PagedResult<RadiologyCenterDto>>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<string>), StatusCodes.Status400BadRequest)]
        [HttpGet(ApiRoutes.Reception.GetAllRadiologyCenters)]
        public async Task<IActionResult> GetAllRadiologyCenters([FromQuery] PaginationRequest pagination)
        {
            var hospitalId = GetHospitalIdFromClaims();
            var result = await _receptionServices.GetAllRadiologyCenters(hospitalId, pagination.Page, pagination.Size);
            return GetResponse(result);
        }

        /// <summary>
        /// Get all radiology centers that perform a specific test
        /// </summary>
        /// <remarks>
        /// Sample request:
        /// <code>
        /// GET /api/reception/radiology-centers/test/{radiologyTestId}?page=1&amp;size=10
        /// </code>
        /// </remarks>
        /// <param name="radiologyTestId">The radiology test ID</param>
        /// <param name="pagination">Pagination parameters</param>
        /// <response code="200">Successfully retrieved radiology centers</response>
        /// <response code="400">Bad request error</response>
        [ProducesResponseType(typeof(ApiResponse<PagedResult<RadiologyCenterDto>>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<string>), StatusCodes.Status400BadRequest)]
        [HttpGet(ApiRoutes.Reception.GetAllRadiologyCentersDoTest)]
        public async Task<IActionResult> GetAllRadiologyCentersDoTest([FromRoute] string radiologyTestId, [FromQuery] PaginationRequest pagination)
        {
            var hospitalId = GetHospitalIdFromClaims();
            var result = await _receptionServices.GetAllRadiologyCentersDoTest(hospitalId, radiologyTestId, pagination.Page, pagination.Size);
            return GetResponse(result);
        }

        /// <summary>
        /// Get all medical labs
        /// </summary>
        /// <remarks>
        /// Sample request:
        /// <code>
        /// GET /api/reception/medical-labs?page=1&amp;size=10
        /// </code>
        /// </remarks>
        /// <param name="pagination">Pagination parameters</param>
        /// <response code="200">Successfully retrieved medical labs</response>
        /// <response code="400">Bad request error</response>
        [ProducesResponseType(typeof(ApiResponse<PagedResult<MedicalLabDto>>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<string>), StatusCodes.Status400BadRequest)]
        [HttpGet(ApiRoutes.Reception.GetAllMedicalLabs)]
        public async Task<IActionResult> GetAllMedicalLabs([FromQuery] PaginationRequest pagination)
        {
            var hospitalId = GetHospitalIdFromClaims();
            var result = await _receptionServices.GetAllMedicalLabs(hospitalId, pagination.Page, pagination.Size);
            return GetResponse(result);
        }

        /// <summary>
        /// Get all medical labs that perform a specific test
        /// </summary>
        /// <remarks>
        /// Sample request:
        /// <code>
        /// GET /api/reception/medical-labs/test/{medicalTestId}?page=1&amp;size=10
        /// </code>
        /// </remarks>
        /// <param name="medicalTestId">The medical test ID</param>
        /// <param name="pagination">Pagination parameters</param>
        /// <response code="200">Successfully retrieved medical labs</response>
        /// <response code="400">Bad request error</response>
        [ProducesResponseType(typeof(ApiResponse<PagedResult<MedicalLabDto>>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<string>), StatusCodes.Status400BadRequest)]
        [HttpGet(ApiRoutes.Reception.GetAllMedicalLabsDoTest)]
        public async Task<IActionResult> GetAllMedicalLabsDoTest([FromRoute] string medicalTestId, [FromQuery] PaginationRequest pagination)
        {
            var hospitalId = GetHospitalIdFromClaims();
            var result = await _receptionServices.GetAllMedicalLabsDoTest(hospitalId, medicalTestId, pagination.Page, pagination.Size);
            return GetResponse(result);
        }

        /// <summary>
        /// Get all appointments for a specific clinic
        /// </summary>
        /// <remarks>
        /// Sample request:
        /// <code>
        /// GET /api/reception/appointments/clinic/{clinicId}?dateFrom=2024-03-20&amp;dateTo=2024-03-21&amp;page=1&amp;size=10
        /// </code>
        /// </remarks>
        /// <param name="clinicId">The clinic ID</param>
        /// <param name="filter">Date filter parameters</param>
        /// <param name="pagination">Pagination parameters</param>
        /// <response code="200">Successfully retrieved appointments</response>
        /// <response code="400">Bad request error</response>
        [ProducesResponseType(typeof(ApiResponse<PagedResult<AppointmentDto>>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<string>), StatusCodes.Status400BadRequest)]
        [HttpGet(ApiRoutes.Reception.GetAllAppointmentsForClinic)]
        public async Task<IActionResult> GetAllAppointmentsForClinic([FromRoute] string clinicId, [FromQuery] DateFilteration filter, [FromQuery] PaginationRequest pagination)
        {
            var result = await _receptionServices.GetAllAppointmentsForClinic(clinicId, filter.DateFrom, filter.DateTo, pagination.Page, pagination.Size);
            return GetResponse(result);
        }

        /// <summary>
        /// Get queue for a specific clinic
        /// </summary>
        /// <remarks>
        /// Sample request:
        /// <code>
        /// GET /api/reception/queue/clinic/{clinicId}?page=1&amp;size=10
        /// </code>
        /// </remarks>
        /// <param name="clinicId">The clinic ID</param>
        /// <param name="pagination">Pagination parameters</param>
        /// <response code="200">Successfully retrieved queue</response>
        /// <response code="400">Bad request error</response>
        [ProducesResponseType(typeof(ApiResponse<PagedResult<AppointmentDto>>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<string>), StatusCodes.Status400BadRequest)]
        [HttpGet(ApiRoutes.Reception.GetQueueForClinic)]
        public async Task<IActionResult> GetQueueForClinic([FromRoute] string clinicId, [FromQuery] PaginationRequest pagination)
        {
            var result = await _receptionServices.GetQueueForClinic(clinicId, pagination.Page, pagination.Size);
            return GetResponse(result);
        }

        /// <summary>
        /// Get all appointments for a specific radiology center
        /// </summary>
        /// <remarks>
        /// Sample request:
        /// <code>
        /// GET /api/reception/appointments/radiology/{radiologyCenterId}?dateFrom=2024-03-20&amp;dateTo=2024-03-21&amp;page=1&amp;size=10
        /// </code>
        /// </remarks>
        /// <param name="radiologyCenterId">The radiology center ID</param>
        /// <param name="filter">Date filter parameters</param>
        /// <param name="pagination">Pagination parameters</param>
        /// <response code="200">Successfully retrieved appointments</response>
        /// <response code="400">Bad request error</response>
        [ProducesResponseType(typeof(ApiResponse<PagedResult<AppointmentDto>>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<string>), StatusCodes.Status400BadRequest)]
        [HttpGet(ApiRoutes.Reception.GetAllAppointmentsForRadiologyCenter)]
        public async Task<IActionResult> GetAllAppointmentsForRadiologyCenter([FromRoute] string radiologyCenterId, [FromQuery] DateFilteration filter, [FromQuery] PaginationRequest pagination)
        {
            var result = await _receptionServices.GetAllAppointmentsForRadiologyCenter(radiologyCenterId, filter.DateFrom, filter.DateTo, pagination.Page, pagination.Size);
            return GetResponse(result);
        }

        /// <summary>
        /// Get queue for a specific radiology center
        /// </summary>
        /// <remarks>
        /// Sample request:
        /// <code>
        /// GET /api/reception/queue/radiology/{radiologyCenterId}?page=1&amp;size=10
        /// </code>
        /// </remarks>
        /// <param name="radiologyCenterId">The radiology center ID</param>
        /// <param name="pagination">Pagination parameters</param>
        /// <response code="200">Successfully retrieved queue</response>
        /// <response code="400">Bad request error</response>
        [ProducesResponseType(typeof(ApiResponse<PagedResult<AppointmentDto>>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<string>), StatusCodes.Status400BadRequest)]
        [HttpGet(ApiRoutes.Reception.GetQueueForRadiologyCenter)]
        public async Task<IActionResult> GetQueueForRadiologyCenter([FromRoute] string radiologyCenterId, [FromQuery] PaginationRequest pagination)
        {
            var result = await _receptionServices.GetQueueForRadiologyCenter(radiologyCenterId, pagination.Page, pagination.Size);
            return GetResponse(result);
        }

        /// <summary>
        /// Get all appointments for a specific medical lab
        /// </summary>
        /// <remarks>
        /// Sample request:
        /// <code>
        /// GET /api/reception/appointments/medical-lab/{medicalLabId}?dateFrom=2024-03-20&amp;dateTo=2024-03-21&amp;page=1&amp;size=10
        /// </code>
        /// </remarks>
        /// <param name="medicalLabId">The medical lab ID</param>
        /// <param name="filter">Date filter parameters</param>
        /// <param name="pagination">Pagination parameters</param>
        /// <response code="200">Successfully retrieved appointments</response>
        /// <response code="400">Bad request error</response>
        [ProducesResponseType(typeof(ApiResponse<PagedResult<AppointmentDto>>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<string>), StatusCodes.Status400BadRequest)]
        [HttpGet(ApiRoutes.Reception.GetAllAppointmentsForMedicalLab)]
        public async Task<IActionResult> GetAllAppointmentsForMedicalLab([FromRoute] string medicalLabId, [FromQuery] DateFilteration filter, [FromQuery] PaginationRequest pagination)
        {
            var result = await _receptionServices.GetAllAppointmentsForMedicalLab(medicalLabId, filter.DateFrom, filter.DateTo, pagination.Page, pagination.Size);
            return GetResponse(result);
        }

        /// <summary>
        /// Get queue for a specific medical lab
        /// </summary>
        /// <remarks>
        /// Sample request:
        /// <code>
        /// GET /api/reception/queue/medical-lab/{medicalLabId}?page=1&amp;size=10
        /// </code>
        /// </remarks>
        /// <param name="medicalLabId">The medical lab ID</param>
        /// <param name="pagination">Pagination parameters</param>
        /// <response code="200">Successfully retrieved queue</response>
        /// <response code="400">Bad request error</response>
        [ProducesResponseType(typeof(ApiResponse<PagedResult<AppointmentDto>>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<string>), StatusCodes.Status400BadRequest)]
        [HttpGet(ApiRoutes.Reception.GetQueueForMedicalLab)]
        public async Task<IActionResult> GetQueueForMedicalLab([FromRoute] string medicalLabId, [FromQuery] PaginationRequest pagination)
        {
            var result = await _receptionServices.GetQueueForMedicalLab(medicalLabId, pagination.Page, pagination.Size);
            return GetResponse(result);
        }

        /// <summary>
        /// Get open tickets for a patient by national ID
        /// </summary>
        /// <remarks>
        /// Sample request:
        /// <code>
        /// GET /api/reception/tickets/national-id/{nationalId}?page=1&amp;size=10
        /// </code>
        /// </remarks>
        /// <param name="nationalId">The patient's national ID</param>
        /// <param name="pagination">Pagination parameters</param>
        /// <response code="200">Successfully retrieved tickets</response>
        /// <response code="400">Bad request error</response>
        [ProducesResponseType(typeof(ApiResponse<PatientDetailsWithTicketsDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<string>), StatusCodes.Status400BadRequest)]
        [HttpGet(ApiRoutes.Reception.GetOpenTicketsByNationalId)]
        public async Task<IActionResult> GetOpenTicketsForPatientByNationalId([FromRoute] string nationalId, [FromQuery] PaginationRequest pagination)
        {
            var result = await _receptionServices.GetOpenTicketsForPatientByNationalId(nationalId, pagination.Page, pagination.Size);
            return GetResponse(result);
        }

        /// <summary>
        /// Get open tickets for a patient by patient ID
        /// </summary>
        /// <remarks>
        /// Sample request:
        /// <code>
        /// GET /api/reception/tickets/patient/{patientId}?page=1&amp;size=10
        /// </code>
        /// </remarks>
        /// <param name="patientId">The patient ID</param>
        /// <param name="pagination">Pagination parameters</param>
        /// <response code="200">Successfully retrieved tickets</response>
        /// <response code="400">Bad request error</response>
        [ProducesResponseType(typeof(ApiResponse<PagedResult<TicketDto>>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<string>), StatusCodes.Status400BadRequest)]
        [HttpGet(ApiRoutes.Reception.GetOpenTicketsByPatientId)]
        public async Task<IActionResult> GetOpenTicketsForPatientById([FromRoute] string patientId, [FromQuery] PaginationRequest pagination)
        {
            var result = await _receptionServices.GetOpenTicketsForPatientById(patientId, pagination.Page, pagination.Size);
            return GetResponse(result);
        }

        /// <summary>
        /// Create a new clinic appointment
        /// </summary>
        /// <remarks>
        /// Sample request:
        /// <code>
        /// POST /api/reception/appointments/clinic
        /// {
        ///     "clinicId": "string",
        ///     "ticketId": "string",
        ///     "expectedTimeForStart": "2024-03-20T10:00:00Z",
        ///     "nationalId": "string"
        /// }
        /// </code>
        /// </remarks>
        /// <param name="model">The appointment creation model</param>
        /// <response code="200">Successfully created appointment</response>
        /// <response code="400">Bad request error</response>
        [ProducesResponseType(typeof(ApiResponse<string>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<string>), StatusCodes.Status400BadRequest)]
        [HttpPost(ApiRoutes.Reception.CreateClinicAppointment)]
        public async Task<IActionResult> CreateClinicAppointment([FromBody] CreateClinicAppointmentModelForReception model)
        {
            var result = await _receptionServices.CreateAppointment(model);
            return GetResponseWithoutType(result);
        }

        /// <summary>
        /// Create a new radiology appointment
        /// </summary>
        /// <remarks>
        /// Sample request:
        /// <code>
        /// POST /api/reception/appointments/radiology
        /// {
        ///     "radiologyCenterId": "string",
        ///     "ticketId": "string",
        ///     "expectedTimeForStart": "2024-03-20T10:00:00Z",
        ///     "nationalId": "string"
        /// }
        /// </code>
        /// </remarks>
        /// <param name="model">The appointment creation model</param>
        /// <response code="200">Successfully created appointment</response>
        /// <response code="400">Bad request error</response>
        [ProducesResponseType(typeof(ApiResponse<string>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<string>), StatusCodes.Status400BadRequest)]
        [HttpPost(ApiRoutes.Reception.CreateRadiologyAppointment)]
        public async Task<IActionResult> CreateRadiologyAppointment([FromBody] CreateRadiologyAppointmentModelForReception model)
        {
            var result = await _receptionServices.CreateAppointment(model);
            return GetResponseWithoutType(result);
        }

        /// <summary>
        /// Create a new medical lab appointment
        /// </summary>
        /// <remarks>
        /// Sample request:
        /// <code>
        /// POST /api/reception/appointments/medical-lab
        /// {
        ///     "medicalLabId": "string",
        ///     "ticketId": "string",
        ///     "expectedTimeForStart": "2024-03-20T10:00:00Z",
        ///     "nationalId": "string"
        /// }
        /// </code>
        /// </remarks>
        /// <param name="model">The appointment creation model</param>
        /// <response code="200">Successfully created appointment</response>
        /// <response code="400">Bad request error</response>
        [ProducesResponseType(typeof(ApiResponse<string>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<string>), StatusCodes.Status400BadRequest)]
        [HttpPost(ApiRoutes.Reception.CreateMedicalLabAppointment)]
        public async Task<IActionResult> CreateMedicalLabAppointment([FromBody] CreateMedicalLabAppointmentModelForReception model)
        {
            var result = await _receptionServices.CreateAppointment(model);
            return GetResponseWithoutType(result);
        }

        /// <summary>
        /// Terminate an appointment
        /// </summary>
        /// <remarks>
        /// Sample request:
        /// <code>
        /// POST /api/reception/appointments/{appointmentId}/terminate
        /// </code>
        /// </remarks>
        /// <param name="appointmentId">The appointment ID</param>
        /// <response code="200">Successfully terminated appointment</response>
        /// <response code="400">Bad request error</response>
        [ProducesResponseType(typeof(ApiResponse<string>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<string>), StatusCodes.Status400BadRequest)]
        [HttpPost(ApiRoutes.Reception.TerminateAppointment)]
        public async Task<IActionResult> TerminateAppointment([FromRoute] string appointmentId)
        {
            var result = await _receptionServices.TerminateAppointment(appointmentId);
            return GetResponseWithoutType(result);
        }

        /// <summary>
        /// Swap two clinic appointments
        /// </summary>
        /// <remarks>
        /// Sample request:
        /// <code>
        /// POST /api/reception/appointments/clinic/swap/{appointmentId1}/{appointmentId2}
        /// </code>
        /// </remarks>
        /// <param name="appointmentId1">First appointment ID</param>
        /// <param name="appointmentId2">Second appointment ID</param>
        /// <response code="200">Successfully swapped appointments</response>
        /// <response code="400">Bad request error</response>
        [ProducesResponseType(typeof(ApiResponse<string>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<string>), StatusCodes.Status400BadRequest)]
        [HttpPost(ApiRoutes.Reception.SwapClinicAppointments)]
        public async Task<IActionResult> SwapClinicAppointments([FromRoute] string appointmentId1, [FromRoute] string appointmentId2)
        {
            var result = await _receptionServices.SwapClinicAppointments(appointmentId1, appointmentId2);
            return GetResponseWithoutType(result);
        }

        /// <summary>
        /// Swap two medical lab appointments
        /// </summary>
        /// <remarks>
        /// Sample request:
        /// <code>
        /// POST /api/reception/appointments/medical-lab/swap/{appointmentId1}/{appointmentId2}
        /// </code>
        /// </remarks>
        /// <param name="appointmentId1">First appointment ID</param>
        /// <param name="appointmentId2">Second appointment ID</param>
        /// <response code="200">Successfully swapped appointments</response>
        /// <response code="400">Bad request error</response>
        [ProducesResponseType(typeof(ApiResponse<string>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<string>), StatusCodes.Status400BadRequest)]
        [HttpPost(ApiRoutes.Reception.SwapMedicalLabAppointments)]
        public async Task<IActionResult> SwapMedicalLabAppointments([FromRoute] string appointmentId1, [FromRoute] string appointmentId2)
        {
            var result = await _receptionServices.SwapMedicalLabAppointments(appointmentId1, appointmentId2);
            return GetResponseWithoutType(result);
        }

        /// <summary>
        /// Swap two radiology center appointments
        /// </summary>
        /// <remarks>
        /// Sample request:
        /// <code>
        /// POST /api/reception/appointments/radiology/swap/{appointmentId1}/{appointmentId2}
        /// </code>
        /// </remarks>
        /// <param name="appointmentId1">First appointment ID</param>
        /// <param name="appointmentId2">Second appointment ID</param>
        /// <response code="200">Successfully swapped appointments</response>
        /// <response code="400">Bad request error</response>
        [ProducesResponseType(typeof(ApiResponse<string>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<string>), StatusCodes.Status400BadRequest)]
        [HttpPost(ApiRoutes.Reception.SwapRadiologyCenterAppointments)]
        public async Task<IActionResult> SwapRadiologyCenterAppointments([FromRoute] string appointmentId1, [FromRoute] string appointmentId2)
        {
            var result = await _receptionServices.SwapRadiologyCenterAppointments(appointmentId1, appointmentId2);
            return GetResponseWithoutType(result);
        }

        /// <summary>
        /// Reschedule a clinic appointment
        /// </summary>
        /// <remarks>
        /// Sample request:
        /// <code>
        /// POST /api/reception/appointments/clinic/{appointmentId}/reschedule/{departmentId}?newDateTime=2024-03-20T10:00:00Z
        /// </code>
        /// </remarks>
        /// <param name="appointmentId">The appointment ID</param>
        /// <param name="departmentId">The department ID</param>
        /// <param name="newDateTime">The new appointment date and time</param>
        /// <response code="200">Successfully rescheduled appointment</response>
        /// <response code="400">Bad request error</response>
        [ProducesResponseType(typeof(ApiResponse<string>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<string>), StatusCodes.Status400BadRequest)]
        [HttpPost(ApiRoutes.Reception.RescheduleClinicAppointment)]
        public async Task<IActionResult> RescheduleClinicAppointment([FromRoute] string appointmentId, [FromRoute] string departmentId, [FromQuery] DateTime newDateTime)
        {
            var result = await _receptionServices.RescheduleClinicAppointment(appointmentId, departmentId, newDateTime);
            return GetResponseWithoutType(result);
        }

        /// <summary>
        /// Reschedule a medical lab appointment
        /// </summary>
        /// <remarks>
        /// Sample request:
        /// <code>
        /// POST /api/reception/appointments/medical-lab/{appointmentId}/reschedule/{departmentId}?newDateTime=2024-03-20T10:00:00Z
        /// </code>
        /// </remarks>
        /// <param name="appointmentId">The appointment ID</param>
        /// <param name="departmentId">The department ID</param>
        /// <param name="newDateTime">The new appointment date and time</param>
        /// <response code="200">Successfully rescheduled appointment</response>
        /// <response code="400">Bad request error</response>
        [ProducesResponseType(typeof(ApiResponse<string>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<string>), StatusCodes.Status400BadRequest)]
        [HttpPost(ApiRoutes.Reception.RescheduleMedicalLabAppointment)]
        public async Task<IActionResult> RescheduleMedicalLabAppointment([FromRoute] string appointmentId, [FromRoute] string departmentId, [FromQuery] DateTime newDateTime)
        {
            var result = await _receptionServices.RescheduleMedicalLabAppointment(appointmentId, departmentId, newDateTime);
            return GetResponseWithoutType(result);
        }

        /// <summary>
        /// Reschedule a radiology appointment
        /// </summary>
        /// <remarks>
        /// Sample request:
        /// <code>
        /// POST /api/reception/appointments/radiology/{appointmentId}/reschedule/{departmentId}?newDateTime=2024-03-20T10:00:00Z
        /// </code>
        /// </remarks>
        /// <param name="appointmentId">The appointment ID</param>
        /// <param name="departmentId">The department ID</param>
        /// <param name="newDateTime">The new appointment date and time</param>
        /// <response code="200">Successfully rescheduled appointment</response>
        /// <response code="400">Bad request error</response>
        [ProducesResponseType(typeof(ApiResponse<string>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<string>), StatusCodes.Status400BadRequest)]
        [HttpPost(ApiRoutes.Reception.RescheduleRadiologyAppointment)]
        public async Task<IActionResult> RescheduleRadiologyAppointment([FromRoute] string appointmentId, [FromRoute] string departmentId, [FromQuery] DateTime newDateTime)
        {
            var result = await _receptionServices.RescheduleRadiologyAppointment(appointmentId, departmentId, newDateTime);
            return GetResponseWithoutType(result);
        }

        /// <summary>
        /// Remove a clinic appointment from queue
        /// </summary>
        /// <remarks>
        /// Sample request:
        /// <code>
        /// POST /api/reception/queue/clinic/{appointmentId}/remove
        /// </code>
        /// </remarks>
        /// <param name="appointmentId">The appointment ID</param>
        /// <response code="200">Successfully removed appointment from queue</response>
        /// <response code="400">Bad request error</response>
        [ProducesResponseType(typeof(ApiResponse<string>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<string>), StatusCodes.Status400BadRequest)]
        [HttpPost(ApiRoutes.Reception.RemoveClinicAppointmentFromQueue)]
        public async Task<IActionResult> RemoveClinicAppointmentFromQueue([FromRoute] string appointmentId)
        {
            var result = await _receptionServices.RemoveClinicAppointmentFromQueue(appointmentId);
            return GetResponseWithoutType(result);
        }

        /// <summary>
        /// Remove a medical lab appointment from queue
        /// </summary>
        /// <remarks>
        /// Sample request:
        /// <code>
        /// POST /api/reception/queue/medical-lab/{appointmentId}/remove
        /// </code>
        /// </remarks>
        /// <param name="appointmentId">The appointment ID</param>
        /// <response code="200">Successfully removed appointment from queue</response>
        /// <response code="400">Bad request error</response>
        [ProducesResponseType(typeof(ApiResponse<string>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<string>), StatusCodes.Status400BadRequest)]
        [HttpPost(ApiRoutes.Reception.RemoveMedicalLabAppointmentFromQueue)]
        public async Task<IActionResult> RemoveMedicalLabAppointmentFromQueue([FromRoute] string appointmentId)
        {
            var result = await _receptionServices.RemoveMedicalLabAppointmentFromQueue(appointmentId);
            return GetResponseWithoutType(result);
        }

        /// <summary>
        /// Remove a radiology center appointment from queue
        /// </summary>
        /// <remarks>
        /// Sample request:
        /// <code>
        /// POST /api/reception/queue/radiology/{appointmentId}/remove
        /// </code>
        /// </remarks>
        /// <param name="appointmentId">The appointment ID</param>
        /// <response code="200">Successfully removed appointment from queue</response>
        /// <response code="400">Bad request error</response>
        [ProducesResponseType(typeof(ApiResponse<string>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<string>), StatusCodes.Status400BadRequest)]
        [HttpPost(ApiRoutes.Reception.RemoveRadiologyCenterAppointmentFromQueue)]
        public async Task<IActionResult> RemoveRadiologyCenterAppointmentFromQueue([FromRoute] string appointmentId)
        {
            var result = await _receptionServices.RemoveRadiologyCenterAppointmentFromQueue(appointmentId);
            return GetResponseWithoutType(result);
        }

        /// <summary>
        /// Add a clinic appointment to queue
        /// </summary>
        /// <remarks>
        /// Sample request:
        /// <code>
        /// POST /api/reception/queue/clinic/{appointmentId}/add
        /// </code>
        /// </remarks>
        /// <param name="appointmentId">The appointment ID</param>
        /// <response code="200">Successfully added appointment to queue</response>
        /// <response code="400">Bad request error</response>
        [ProducesResponseType(typeof(ApiResponse<string>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<string>), StatusCodes.Status400BadRequest)]
        [HttpPost(ApiRoutes.Reception.AddClinicAppointmentForQueue)]
        public async Task<IActionResult> AddClinicAppointmentForQueue([FromRoute] string appointmentId)
        {
            var result = await _receptionServices.AddClinicAppointmentForQueue(appointmentId);
            return GetResponseWithoutType(result);
        }

        /// <summary>
        /// Add a medical lab appointment to queue
        /// </summary>
        /// <remarks>
        /// Sample request:
        /// <code>
        /// POST /api/reception/queue/medical-lab/{appointmentId}/add
        /// </code>
        /// </remarks>
        /// <param name="appointmentId">The appointment ID</param>
        /// <response code="200">Successfully added appointment to queue</response>
        /// <response code="400">Bad request error</response>
        [ProducesResponseType(typeof(ApiResponse<string>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<string>), StatusCodes.Status400BadRequest)]
        [HttpPost(ApiRoutes.Reception.AddMedicalLabAppointmentForQueue)]
        public async Task<IActionResult> AddMedicalLabAppointmentForQueue([FromRoute] string appointmentId)
        {
            var result = await _receptionServices.AddMedicalLabAppointmentForQueue(appointmentId);
            return GetResponseWithoutType(result);
        }

        /// <summary>
        /// Add a radiology center appointment to queue
        /// </summary>
        /// <remarks>
        /// Sample request:
        /// <code>
        /// POST /api/reception/queue/radiology/{appointmentId}/add
        /// </code>
        /// </remarks>
        /// <param name="appointmentId">The appointment ID</param>
        /// <response code="200">Successfully added appointment to queue</response>
        /// <response code="400">Bad request error</response>
        [ProducesResponseType(typeof(ApiResponse<string>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<string>), StatusCodes.Status400BadRequest)]
        [HttpPost(ApiRoutes.Reception.AddRadiologyCenterAppointmentForQueue)]
        public async Task<IActionResult> AddRadiologyCenterAppointmentForQueue([FromRoute] string appointmentId)
        {
            var result = await _receptionServices.AddRadiologyCenterAppointmentForQueue(appointmentId);
            return GetResponseWithoutType(result);
        }

        /// <summary>
        /// Create a new ticket
        /// </summary>
        /// <remarks>
        /// Sample request:
        /// <code>
        /// POST /api/reception/tickets
        /// {
        ///     "patientIdentifier": "string",
        ///     "identifierType": "NationalId"
        /// }
        /// </code>
        /// </remarks>
        /// <param name="model">The ticket creation model</param>
        /// <response code="200">Successfully created ticket</response>
        /// <response code="400">Bad request error</response>
        [ProducesResponseType(typeof(ApiResponse<string>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<string>), StatusCodes.Status400BadRequest)]
        [HttpPost(ApiRoutes.Reception.CreateNewTicket)]
        public async Task<IActionResult> CreateNewTicket([FromBody] CreateTicketModelForReception model)
        {
            var hospitalId = GetHospitalIdFromClaims();
            var result = await _receptionServices.CreateNewTicket(model.PatientIdentifier, model.IdentifierType, hospitalId);
            return GetResponseWithoutType(result);
        }

        /// <summary>
        /// Get available time slots for a clinic
        /// </summary>
        /// <remarks>
        /// Sample request:
        /// <code>
        /// GET /api/reception/available-slots/clinic/{clinicId}?date=2024-03-20
        /// </code>
        /// </remarks>
        /// <param name="clinicId">The clinic ID</param>
        /// <param name="date">Optional date filter</param>
        /// <response code="200">Successfully retrieved time slots</response>
        /// <response code="400">Bad request error</response>
        [ProducesResponseType(typeof(ApiResponse<List<DateTime>>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<string>), StatusCodes.Status400BadRequest)]
        [HttpGet(ApiRoutes.Reception.GetAvailableTimeSlotsForClinic)]
        public async Task<IActionResult> GetAvailableTimeSlotsForClinic([FromRoute] string clinicId, [FromQuery] DateTime? date)
        {
            var result = await _receptionServices.GetAvailableTimeSlotsForClinic(clinicId, date);
            return GetResponse(result);
        }

        /// <summary>
        /// Get available time slots for a medical lab
        /// </summary>
        /// <remarks>
        /// Sample request:
        /// <code>
        /// GET /api/reception/available-slots/medical-lab/{medicalLabId}?date=2024-03-20
        /// </code>
        /// </remarks>
        /// <param name="medicalLabId">The medical lab ID</param>
        /// <param name="date">Optional date filter</param>
        /// <response code="200">Successfully retrieved time slots</response>
        /// <response code="400">Bad request error</response>
        [ProducesResponseType(typeof(ApiResponse<List<DateTime>>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<string>), StatusCodes.Status400BadRequest)]
        [HttpGet(ApiRoutes.Reception.GetAvailableTimeSlotsForMedicalLab)]
        public async Task<IActionResult> GetAvailableTimeSlotsForMedicalLab([FromRoute] string medicalLabId, [FromQuery] DateTime? date)
        {
            var result = await _receptionServices.GetAvailableTimeSlotsForMedicalLab(medicalLabId, date);
            return GetResponse(result);
        }

        /// <summary>
        /// Get available time slots for a radiology center
        /// </summary>
        /// <remarks>
        /// Sample request:
        /// <code>
        /// GET /api/reception/available-slots/radiology/{radiologyCenterId}?date=2024-03-20
        /// </code>
        /// </remarks>
        /// <param name="radiologyCenterId">The radiology center ID</param>
        /// <param name="date">Optional date filter</param>
        /// <response code="200">Successfully retrieved time slots</response>
        /// <response code="400">Bad request error</response>
        [ProducesResponseType(typeof(ApiResponse<List<DateTime>>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<string>), StatusCodes.Status400BadRequest)]
        [HttpGet(ApiRoutes.Reception.GetAvailableTimeSlotsForRadiologyCenter)]
        public async Task<IActionResult> GetAvailableTimeSlotsForRadiologyCenter([FromRoute] string radiologyCenterId, [FromQuery] DateTime? date)
        {
            var result = await _receptionServices.GetAvailableTimeSlotsForRadiologyCenter(radiologyCenterId, date);
            return GetResponse(result);
        }

        /// <summary>
        /// Start a clinic appointment
        /// </summary>
        /// <remarks>
        /// Sample request:
        /// <code>
        /// POST /api/appointments/clinic/{appointmentId}/start
        /// </code>
        /// </remarks>
        /// <param name="appointmentId">The appointment ID</param>
        /// <response code="200">Successfully started appointment</response>
        /// <response code="400">Bad request error</response>
        [ProducesResponseType(typeof(ApiResponse<string>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<string>), StatusCodes.Status400BadRequest)]
        [HttpPost(ApiRoutes.Reception.StartClinicAppointment)]
        public async Task<IActionResult> StartClinicAppointment([FromRoute] string appointmentId)
        {
            var result = await _receptionServices.StartClinicAppointment(appointmentId);
            return GetResponseWithoutType(result);
        }

        /// <summary>
        /// Start a clinic appointment
        /// </summary>
        /// <remarks>
        /// Sample request:
        /// <code>
        /// POST /api/appointments/medical-lab/{appointmentId}/start
        /// </code>
        /// </remarks>
        /// <param name="appointmentId">The appointment ID</param>
        /// <response code="200">Successfully started appointment</response>
        /// <response code="400">Bad request error</response>
        [ProducesResponseType(typeof(ApiResponse<string>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<string>), StatusCodes.Status400BadRequest)]
        [HttpPost(ApiRoutes.Reception.StartMedicalLabAppointment)]
        public async Task<IActionResult> StartMedicalLabAppointment([FromRoute] string appointmentId)
        {
            var result = await _receptionServices.StartMedicalLabAppointment(appointmentId);
            return GetResponseWithoutType(result);
        }

        /// <summary>
        /// Start a clinic appointment
        /// </summary>
        /// <remarks>
        /// Sample request:
        /// <code>
        /// POST /api/appointments/radiology/{appointmentId}/start
        /// </code>
        /// </remarks>
        /// <param name="appointmentId">The appointment ID</param>
        /// <response code="200">Successfully started appointment</response>
        /// <response code="400">Bad request error</response>
        [ProducesResponseType(typeof(ApiResponse<string>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<string>), StatusCodes.Status400BadRequest)]
        [HttpPost(ApiRoutes.Reception.StartRadiologyAppointment)]
        public async Task<IActionResult> StartRadiologyAppointment([FromRoute] string appointmentId)
        {
            var result = await _receptionServices.StartRadiologyAppointment(appointmentId);
            return GetResponseWithoutType(result);
        }
    }
}
