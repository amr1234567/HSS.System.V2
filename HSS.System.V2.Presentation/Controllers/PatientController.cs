using FluentResults;

using HSS.System.V2.Domain.Helpers.Models;
using HSS.System.V2.Domain.ResultHelpers.Errors;
using HSS.System.V2.Presentation.Controllers.Base;
using HSS.System.V2.Services.Contracts;
using HSS.System.V2.Services.DTOs.PatientDTOs;
using HSS.System.V2.Domain.Helpers.Methods;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HSS.System.V2.Presentation.Controllers
{
    [Authorize]
    [Route(ApiRoutes.Patient.Base)]
    public class PatientController : CustomBaseController
    {
        private readonly IPatientService _patientService;

        public PatientController(IPatientService patientService)
        {
            _patientService = patientService;
        }

        /// <summary>
        /// Get the count of notifications for the patient.
        /// </summary>
        /// <remarks>
        /// Sample request:
        /// <code>
        /// GET /notifications/count
        /// </code>
        /// </remarks>
        [ProducesResponseType(typeof(ApiResponse<int>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<int>), StatusCodes.Status400BadRequest)]
        [HttpGet(ApiRoutes.Patient.NotificationCount)]
        public async Task<IActionResult> NotificationCount()
        {
            var result = await _patientService.NotificationCount();
            return GetResponse(result);
        }

        /// <summary>
        /// Get all notifications for the patient.
        /// </summary>
        /// <remarks>
        /// Sample request:
        /// <code>
        /// GET /notifications
        /// </code>
        /// </remarks>
        [ProducesResponseType(typeof(ApiResponse<List<NotificationDto>>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<List<NotificationDto>>), StatusCodes.Status400BadRequest)]
        [HttpGet(ApiRoutes.Patient.GetNotifications)]
        public async Task<IActionResult> GetNotifications()
        {
            var result = await _patientService.GetNotifications();
            return GetResponse(result);
        }

        /// <summary>
        /// Get specific notification data by ID.
        /// </summary>
        /// <remarks>
        /// Sample request:
        /// <code>
        /// GET /notifications/{notificationId}
        /// </code>
        /// </remarks>
        /// <param name="notificationId">The ID of the notification to retrieve</param>
        [ProducesResponseType(typeof(ApiResponse<NotificationData>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<NotificationData>), StatusCodes.Status400BadRequest)]
        [HttpGet(ApiRoutes.Patient.GetNotification)]
        public async Task<IActionResult> GetNotificationData([FromRoute] string notificationId)
        {
            var result = await _patientService.GetNotificationData(notificationId);
            return GetResponse(result);
        }

        /// <summary>
        /// Mark a notification as seen.
        /// </summary>
        /// <remarks>
        /// Sample request:
        /// <code>
        /// PUT /notifications/{notificationId}/seen
        /// </code>
        /// </remarks>
        /// <param name="notificationId">The ID of the notification to mark as seen</param>
        [ProducesResponseType(typeof(ApiResponse<bool>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<bool>), StatusCodes.Status400BadRequest)]
        [HttpPut(ApiRoutes.Patient.SeenNotification)]
        public async Task<IActionResult> SeenNotification([FromRoute] string notificationId)
        {
            var result = await _patientService.SeenNotification(notificationId);
            return GetResponseWithoutType(result);
        }

        /// <summary>
        /// Submit a sugar test result.
        /// </summary>
        /// <remarks>
        /// Sample request:
        /// <code>
        /// POST /sugar-test
        /// </code>
        /// </remarks>
        /// <param name="model">The diabetes test model containing test results</param>
        [ProducesResponseType(typeof(ApiResponse<DiabetesResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<DiabetesResponse>), StatusCodes.Status400BadRequest)]
        [HttpPost(ApiRoutes.Patient.SugerTest)]
        public async Task<IActionResult> SugerTest([FromBody] DiabetesTestModel model)
        {
            var result = await _patientService.SugerTest(model);
            return GetResponse(result);
        }

        /// <summary>
        /// Get current appointments with pagination.
        /// </summary>
        /// <remarks>
        /// Sample request:
        /// <code>
        /// GET /appointments/current?page=1&pageSize=10
        /// </code>
        /// </remarks>
        /// <param name="pagination">Pagination parameters</param>
        [ProducesResponseType(typeof(ApiResponse<PagedResult<AppointmentView>>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<PagedResult<AppointmentView>>), StatusCodes.Status400BadRequest)]
        [HttpGet(ApiRoutes.Patient.GetCurrentAppointments)]
        public async Task<IActionResult> GetCurrentAppointments([FromQuery] PaginationRequest pagination)
        {
            var result = await _patientService.GetCurrentAppontments(pagination);
            return GetResponse(result);
        }

        /// <summary>
        /// Get details of a specific appointment.
        /// </summary>
        /// <remarks>
        /// Sample request:
        /// <code>
        /// GET /appointments/{appointmentId}
        /// </code>
        /// </remarks>
        /// <param name="appointmentId">The ID of the appointment to retrieve</param>
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
        [HttpGet(ApiRoutes.Patient.GetAppointmentDetails)]
        public async Task<IActionResult> GetAppointmentDetails([FromRoute] string appointmentId)
        {
            var result = await _patientService.GetAppointmentDetails(appointmentId);
            return GetResponse(result);
        }

        /// <summary>
        /// Get all medical specifications with pagination.
        /// </summary>
        /// <remarks>
        /// Sample request:
        /// <code>
        /// GET /specifications?page=1&pageSize=10
        /// </code>
        /// </remarks>
        /// <param name="pagination">Pagination parameters</param>
        [ProducesResponseType(typeof(ApiResponse<PagedResult<SpecialzationDto>>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<PagedResult<SpecialzationDto>>), StatusCodes.Status400BadRequest)]
        [HttpGet(ApiRoutes.Patient.GetAllSpecifications)]
        public async Task<IActionResult> GetAllSpecifications([FromQuery] PaginationRequest pagination)
        {
            var result = await _patientService.GetAllSpecifications(pagination);
            return GetResponse(result);
        }

        /// <summary>
        /// Get all radiology tests with pagination.
        /// </summary>
        /// <remarks>
        /// Sample request:
        /// <code>
        /// GET /radiology-tests?page=1&pageSize=10
        /// </code>
        /// </remarks>
        /// <param name="pagination">Pagination parameters</param>
        [ProducesResponseType(typeof(ApiResponse<PagedResult<RadiologyTestDto>>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<PagedResult<RadiologyTestDto>>), StatusCodes.Status400BadRequest)]
        [HttpGet(ApiRoutes.Patient.GetAllRadiologyTests)]
        public async Task<IActionResult> GetAllRadiologyTests([FromQuery] PaginationRequest pagination)
        {
            var result = await _patientService.GetAllRadiologyTest(pagination);
            return GetResponse(result);
        }

        /// <summary>
        /// Get all medical lab tests with pagination.
        /// </summary>
        /// <remarks>
        /// Sample request:
        /// <code>
        /// GET /medical-lab-tests?page=1&pageSize=10
        /// </code>
        /// </remarks>
        /// <param name="pagination">Pagination parameters</param>
        [ProducesResponseType(typeof(ApiResponse<PagedResult<MedicalLabTestDto>>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<PagedResult<MedicalLabTestDto>>), StatusCodes.Status400BadRequest)]
        [HttpGet(ApiRoutes.Patient.GetAllMedicalLabTests)]
        public async Task<IActionResult> GetAllMedicalLabTests([FromQuery] PaginationRequest pagination)
        {
            var result = await _patientService.GetAllMedicalLabTest(pagination);
            return GetResponse(result);
        }

        /// <summary>
        /// Get hospitals by specialization ID with pagination.
        /// </summary>
        /// <remarks>
        /// Sample request:
        /// <code>
        /// GET /hospitals/specialization/{specializationId}?page=1&pageSize=10
        /// </code>
        /// </remarks>
        /// <param name="specializationId">The ID of the specialization</param>
        /// <param name="pagination">Pagination parameters</param>
        [ProducesResponseType(typeof(ApiResponse<PagedResult<HospitalDto>>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<PagedResult<HospitalDto>>), StatusCodes.Status400BadRequest)]
        [HttpGet(ApiRoutes.Patient.GetHospitalsBySpecificationId)]
        public async Task<IActionResult> GetHospitalsBySpecificationId([FromRoute] string specializationId, [FromQuery] PaginationRequest pagination)
        {
            var result = await _patientService.GetHospitalsBySpecificationId(specializationId, pagination);
            return GetResponse(result);
        }

        /// <summary>
        /// Get hospitals by radiology test ID with pagination.
        /// </summary>
        /// <remarks>
        /// Sample request:
        /// <code>
        /// GET /hospitals/radiology/{radiologyTestId}?page=1&pageSize=10
        /// </code>
        /// </remarks>
        /// <param name="radiologyTestId">The ID of the radiology test</param>
        /// <param name="pagination">Pagination parameters</param>
        [ProducesResponseType(typeof(ApiResponse<PagedResult<HospitalDto>>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<PagedResult<HospitalDto>>), StatusCodes.Status400BadRequest)]
        [HttpGet(ApiRoutes.Patient.GetHospitalsByRadiologyTestId)]
        public async Task<IActionResult> GetHospitalsByRadiologyTestId([FromRoute] string radiologyTestId, [FromQuery] PaginationRequest pagination)
        {
            var result = await _patientService.GetHospitalsByRadiologyTestId(radiologyTestId, pagination);
            return GetResponse(result);
        }

        /// <summary>
        /// Get hospitals by medical lab test ID with pagination.
        /// </summary>
        /// <remarks>
        /// Sample request:
        /// <code>
        /// GET /hospitals/medical-lab/{medicalLabTestId}?page=1&pageSize=10
        /// </code>
        /// </remarks>
        /// <param name="medicalLabTestId">The ID of the medical lab test</param>
        /// <param name="pagination">Pagination parameters</param>
        [ProducesResponseType(typeof(ApiResponse<PagedResult<HospitalDto>>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<PagedResult<HospitalDto>>), StatusCodes.Status400BadRequest)]
        [HttpGet(ApiRoutes.Patient.GetHospitalsByMedicalLabTestId)]
        public async Task<IActionResult> GetHospitalsByMedicalLabTestId([FromRoute] string medicalLabTestId, [FromQuery] PaginationRequest pagination)
        {
            var result = await _patientService.GetHospitalsByMedicalLabTestId(medicalLabTestId, pagination);
            return GetResponse(result);
        }

        /// <summary>
        /// Get active tickets in a specific hospital with pagination.
        /// </summary>
        /// <remarks>
        /// Sample request:
        /// <code>
        /// GET /hospitals/{hospitalId}/tickets/active?page=1&pageSize=10
        /// </code>
        /// </remarks>
        /// <param name="hospitalId">The ID of the hospital</param>
        /// <param name="pagination">Pagination parameters</param>
        [ProducesResponseType(typeof(ApiResponse<PagedResult<TicketViewDto>>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<PagedResult<TicketViewDto>>), StatusCodes.Status400BadRequest)]
        [HttpGet(ApiRoutes.Patient.GetActiveTicketsInHospital)]
        public async Task<IActionResult> GetActiveTicketsInHospital([FromRoute] string hospitalId, [FromQuery] PaginationRequest pagination)
        {
            var result = await _patientService.GetActiveTicketInHospital(hospitalId, pagination);
            return GetResponse(result);
        }

        /// <summary>
        /// Get all active tickets in a specific hospital with pagination.
        /// </summary>
        /// <remarks>
        /// Sample request:
        /// <code>
        /// GET /hospitals/{hospitalId}/tickets/all-active?page=1&pageSize=10
        /// </code>
        /// </remarks>
        /// <param name="hospitalId">The ID of the hospital</param>
        /// <param name="pagination">Pagination parameters</param>
        [ProducesResponseType(typeof(ApiResponse<PagedResult<TicketViewDto>>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<PagedResult<TicketViewDto>>), StatusCodes.Status400BadRequest)]
        [HttpGet(ApiRoutes.Patient.GetAllActiveTicketsInHospital)]
        public async Task<IActionResult> GetAllActiveTicketsInHospital([FromRoute] string hospitalId, [FromQuery] PaginationRequest pagination)
        {
            var result = await _patientService.GetActiveTicketInHospital(hospitalId, pagination);
            return GetResponse(result);
        }

        /// <summary>
        /// Get clinics in a hospital for a specific specialization.
        /// </summary>
        /// <remarks>
        /// Sample request:
        /// <code>
        /// GET /hospitals/{hospitalId}/specializations/{specificationId}/clinics
        /// </code>
        /// </remarks>
        /// <param name="hospitalId">The ID of the hospital</param>
        /// <param name="specificationId">The ID of the specialization</param>
        [ProducesResponseType(typeof(ApiResponse<List<DebartmentDto>>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<List<DebartmentDto>>), StatusCodes.Status400BadRequest)]
        [HttpGet(ApiRoutes.Patient.GetClinics)]
        public async Task<IActionResult> GetClinics([FromRoute] string hospitalId, [FromRoute] string specificationId)
        {
            var result = await _patientService.GetClinics(hospitalId, specificationId);
            return GetResponse(result);
        }

        /// <summary>
        /// Get radiology centers in a hospital for a specific test.
        /// </summary>
        /// <remarks>
        /// Sample request:
        /// <code>
        /// GET /hospitals/{hospitalId}/radiology-tests/{testId}/centers
        /// </code>
        /// </remarks>
        /// <param name="hospitalId">The ID of the hospital</param>
        /// <param name="testId">The ID of the radiology test</param>
        [ProducesResponseType(typeof(ApiResponse<List<DebartmentDto>>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<List<DebartmentDto>>), StatusCodes.Status400BadRequest)]
        [HttpGet(ApiRoutes.Patient.GetRadiologyCenters)]
        public async Task<IActionResult> GetRadiologyCenters([FromRoute] string hospitalId, [FromRoute] string testId)
        {
            var result = await _patientService.GetRadiologyCenter(hospitalId, testId);
            return GetResponse(result);
        }

        /// <summary>
        /// Get medical labs in a hospital for a specific test.
        /// </summary>
        /// <remarks>
        /// Sample request:
        /// <code>
        /// GET /hospitals/{hospitalId}/medical-lab-tests/{testId}/labs
        /// </code>
        /// </remarks>
        /// <param name="hospitalId">The ID of the hospital</param>
        /// <param name="testId">The ID of the medical lab test</param>
        [ProducesResponseType(typeof(ApiResponse<List<DebartmentDto>>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<List<DebartmentDto>>), StatusCodes.Status400BadRequest)]
        [HttpGet(ApiRoutes.Patient.GetMedicalLabs)]
        public async Task<IActionResult> GetMedicalLabs([FromRoute] string hospitalId, [FromRoute] string testId)
        {
            var result = await _patientService.GetMedicalLabs(hospitalId, testId);
            return GetResponse(result);
        }

        /// <summary>
        /// Create a new ticket in a hospital.
        /// </summary>
        /// <remarks>
        /// Sample request:
        /// <code>
        /// POST /hospitals/{hospitalId}/tickets
        /// </code>
        /// </remarks>
        /// <param name="hospitalId">The ID of the hospital</param>
        [ProducesResponseType(typeof(ApiResponse<bool>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<bool>), StatusCodes.Status400BadRequest)]
        [HttpPost(ApiRoutes.Patient.CreateTicket)]
        public async Task<IActionResult> CreateTicket([FromRoute] string hospitalId)
        {
            var result = await _patientService.CreateTicket(hospitalId);
            return GetResponseWithoutType(result);
        }

        /// <summary>
        /// Create a new clinic appointment.
        /// </summary>
        /// <remarks>
        /// Sample request:
        /// <code>
        /// POST /appointments/clinic
        /// </code>
        /// </remarks>
        /// <param name="model">The clinic appointment creation model</param>
        [ProducesResponseType(typeof(ApiResponse<bool>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<bool>), StatusCodes.Status400BadRequest)]
        [HttpPost(ApiRoutes.Patient.CreateClinicAppointment)]
        public async Task<IActionResult> CreateClinicAppointment([FromBody] CreateClinicAppointmentModel model)
        {
            var result = await _patientService.CreateClinicAppointment(model);
            return GetResponseWithoutType(result);
        }

        /// <summary>
        /// Create a new radiology appointment.
        /// </summary>
        /// <remarks>
        /// Sample request:
        /// <code>
        /// POST /appointments/radiology
        /// </code>
        /// </remarks>
        /// <param name="model">The radiology appointment creation model</param>
        [ProducesResponseType(typeof(ApiResponse<bool>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<bool>), StatusCodes.Status400BadRequest)]
        [HttpPost(ApiRoutes.Patient.CreateRadiologyAppointment)]
        public async Task<IActionResult> CreateRadiologyAppointment([FromBody] CreateRadiologyAppointmentModel model)
        {
            var result = await _patientService.CreateRadiologyAppointMent(model);
            return GetResponseWithoutType(result);
        }

        /// <summary>
        /// Create a new medical lab appointment.
        /// </summary>
        /// <remarks>
        /// Sample request:
        /// <code>
        /// POST /appointments/medical-lab
        /// </code>
        /// </remarks>
        /// <param name="model">The medical lab appointment creation model</param>
        [ProducesResponseType(typeof(ApiResponse<bool>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<bool>), StatusCodes.Status400BadRequest)]
        [HttpPost(ApiRoutes.Patient.CreateMedicalLabAppointment)]
        public async Task<IActionResult> CreateMedicalLabAppointment([FromBody] CreateMedicalLabAppointmentModel model)
        {
            var result = await _patientService.CreateMedicalLabAppointment(model);
            return GetResponseWithoutType(result);
        }

        /// <summary>
        /// Get required medical lab tests.
        /// </summary>
        /// <remarks>
        /// Sample request:
        /// <code>
        /// GET /medical-lab-tests/required
        /// </code>
        /// </remarks>
        [ProducesResponseType(typeof(ApiResponse<List<TestRquired>>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<List<TestRquired>>), StatusCodes.Status400BadRequest)]
        [HttpGet(ApiRoutes.Patient.GetMedicalLabTestsRequired)]
        public async Task<IActionResult> GetMedicalLabTestsRequired()
        {
            var result = await _patientService.GetMedicalLabTestRequired();
            return GetResponse(result);
        }

        /// <summary>
        /// Get required radiology tests.
        /// </summary>
        /// <remarks>
        /// Sample request:
        /// <code>
        /// GET /radiology-tests/required
        /// </code>
        /// </remarks>
        [ProducesResponseType(typeof(ApiResponse<List<TestRquired>>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<List<TestRquired>>), StatusCodes.Status400BadRequest)]
        [HttpGet(ApiRoutes.Patient.GetRadiologyTestsRequired)]
        public async Task<IActionResult> GetRadiologyTestsRequired()
        {
            var result = await _patientService.GetRadiologyTestRequired();
            return GetResponse(result);
        }

        /// <summary>
        /// Get all required prescriptions with pagination.
        /// </summary>
        /// <remarks>
        /// Sample request:
        /// <code>
        /// GET /prescriptions/required?page=1&pageSize=10
        /// </code>
        /// </remarks>
        /// <param name="pagination">Pagination parameters</param>
        [ProducesResponseType(typeof(ApiResponse<PagedResult<PrescriptionDto>>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<PagedResult<PrescriptionDto>>), StatusCodes.Status400BadRequest)]
        [HttpGet(ApiRoutes.Patient.GetAllPrescriptionsRequired)]
        public async Task<IActionResult> GetAllPrescriptionsRequired([FromQuery] PaginationRequest pagination)
        {
            var result = await _patientService.GetAllPrescriptionsRequired(pagination);
            return GetResponse(result);
        }

        /// <summary>
        /// Get medicines by prescription ID.
        /// </summary>
        /// <remarks>
        /// Sample request:
        /// <code>
        /// GET /prescriptions/{prescriptionId}/medicines
        /// </code>
        /// </remarks>
        /// <param name="prescriptionId">The ID of the prescription</param>
        [ProducesResponseType(typeof(ApiResponse<List<MedicineDto>>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<List<MedicineDto>>), StatusCodes.Status400BadRequest)]
        [HttpGet(ApiRoutes.Patient.GetMedicineByPrescriptionId)]
        public async Task<IActionResult> GetMedicineByPrescriptionId([FromRoute] string prescriptionId)
        {
            var result = await _patientService.GetMedicineByPrescriptionId(prescriptionId);
            return GetResponse(result);
        }

        /// <summary>
        /// Get current clinic appointments with pagination.
        /// </summary>
        /// <remarks>
        /// Sample request:
        /// <code>
        /// GET /appointments/clinic/current?page=1&pageSize=10
        /// </code>
        /// </remarks>
        /// <param name="pagination">Pagination parameters</param>
        [ProducesResponseType(typeof(ApiResponse<PagedResult<AppointmentView>>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<PagedResult<AppointmentView>>), StatusCodes.Status400BadRequest)]
        [HttpGet(ApiRoutes.Patient.GetCurrentClinicAppointments)]
        public async Task<IActionResult> GetCurrentClinicAppointments([FromQuery] PaginationRequest pagination)
        {
            var result = await _patientService.GetCurrentClinicAppointments(pagination);
            return GetResponse(result);
        }

        /// <summary>
        /// Get current medical lab appointments with pagination.
        /// </summary>
        /// <remarks>
        /// Sample request:
        /// <code>
        /// GET /appointments/medical-lab/current?page=1&pageSize=10
        /// </code>
        /// </remarks>
        /// <param name="pagination">Pagination parameters</param>
        [ProducesResponseType(typeof(ApiResponse<PagedResult<AppointmentView>>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<PagedResult<AppointmentView>>), StatusCodes.Status400BadRequest)]
        [HttpGet(ApiRoutes.Patient.GetCurrentMedicalLabAppointments)]
        public async Task<IActionResult> GetCurrentMedicalLabAppointments([FromQuery] PaginationRequest pagination)
        {
            var result = await _patientService.GetCurrentMedicalLabAppointments(pagination);
            return GetResponse(result);
        }

        /// <summary>
        /// Get current radiology center appointments with pagination.
        /// </summary>
        /// <remarks>
        /// Sample request:
        /// <code>
        /// GET /appointments/radiology/current?page=1&pageSize=10
        /// </code>
        /// </remarks>
        /// <param name="pagination">Pagination parameters</param>
        [ProducesResponseType(typeof(ApiResponse<PagedResult<AppointmentView>>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<PagedResult<AppointmentView>>), StatusCodes.Status400BadRequest)]
        [HttpGet(ApiRoutes.Patient.GetCurrentRadiologyCenterAppointments)]
        public async Task<IActionResult> GetCurrentRadiologyCenterAppointments([FromQuery] PaginationRequest pagination)
        {
            var result = await _patientService.GetCurrentRadiologyCenterAppointments(pagination);
            return GetResponse(result);
        }

        /// <summary>
        /// Upload a profile picture.
        /// </summary>
        /// <remarks>
        /// Sample request:
        /// <code>
        /// POST /profile/picture
        /// </code>
        /// </remarks>
        /// <param name="param">The profile picture upload parameters</param>
        [ProducesResponseType(typeof(ApiResponse<bool>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<bool>), StatusCodes.Status400BadRequest)]
        [HttpPost(ApiRoutes.Patient.UploadProfilePicture)]
        public async Task<IActionResult> UploadProfilePicture([FromForm] UploadProfilePictureParam param)
        {
            var result = await _patientService.UploadProfilePicture(param.File);
            return GetResponseWithoutType(result);
        }

        /// <summary>
        /// Get patient profile information.
        /// </summary>
        /// <remarks>
        /// Sample request:
        /// <code>
        /// GET /profile
        /// </code>
        /// </remarks>
        [ProducesResponseType(typeof(ApiResponse<ProfileInformationDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<ProfileInformationDto>), StatusCodes.Status400BadRequest)]
        [HttpGet(ApiRoutes.Patient.GetPatientProfile)]
        public async Task<IActionResult> GetPatientProfileInformation()
        {
            var result = await _patientService.GetPatientProfileInformation();
            return GetResponse(result);
        }

        /// <summary>
        /// Get all active tickets of the patient.
        /// </summary>
        /// <remarks>
        /// Sample request:
        /// <code>
        /// GET /tickets/active
        /// </code>
        /// </remarks>
        [ProducesResponseType(typeof(ApiResponse<List<TicketInformationDto>>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<List<TicketInformationDto>>), StatusCodes.Status400BadRequest)]
        [HttpGet(ApiRoutes.Patient.GetAllActiveTickets)]
        public async Task<IActionResult> GetAllActiveTicketsOfPatient()
        {
            var result = await _patientService.GetAllActiveTicketsOfPatient();
            return GetResponse(result);
        }

        /// <summary>
        /// Get all inactive tickets of the patient.
        /// </summary>
        /// <remarks>
        /// Sample request:
        /// <code>
        /// GET /tickets/inactive
        /// </code>
        /// </remarks>
        [ProducesResponseType(typeof(ApiResponse<List<TicketInformationDto>>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<List<TicketInformationDto>>), StatusCodes.Status400BadRequest)]
        [HttpGet(ApiRoutes.Patient.GetAllInactiveTickets)]
        public async Task<IActionResult> GetAllInactiveTicketsOfPatient()
        {
            var result = await _patientService.GetAllInactiviTicketsOfPatient();
            return GetResponse(result);
        }

        /// <summary>
        /// Get ticket content with pagination.
        /// </summary>
        /// <remarks>
        /// Sample request:
        /// <code>
        /// GET /tickets/{ticketId}/content?page=1&pageSize=10
        /// </code>
        /// </remarks>
        /// <param name="ticketId">The ID of the ticket</param>
        /// <param name="pagination">Pagination parameters</param>
        [ProducesResponseType(typeof(ApiResponse<PagedResult<AppointmentView>>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<PagedResult<AppointmentView>>), StatusCodes.Status400BadRequest)]
        [HttpGet(ApiRoutes.Patient.GetTicketContent)]
        public async Task<IActionResult> GetTicketContent([FromRoute] string ticketId, [FromQuery] PaginationRequest pagination)
        {
            var result = await _patientService.GetTicketContent(ticketId, pagination);
            return GetResponse(result);
        }

        /// <summary>
        /// Get appointment content.
        /// </summary>
        /// <remarks>
        /// Sample request:
        /// <code>
        /// GET /appointments/{appointmentId}/content
        /// </code>
        /// </remarks>
        /// <param name="appointmentId">The ID of the appointment</param>
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
        [HttpGet(ApiRoutes.Patient.GetAppointmentContent)]
        public async Task<IActionResult> GetAppointmentContent([FromRoute] string appointmentId)
        {
            var result = await _patientService.GetAppointmentContent(appointmentId);
            return GetResponse(result);
        }

        /// <summary>
        /// Get all specifications with pagination.
        /// </summary>
        /// <remarks>
        /// Sample request:
        /// <code>
        /// GET /specifications/paged?page=1&pageSize=10
        /// </code>
        /// </remarks>
        /// <param name="pagination">Pagination parameters</param>
        [ProducesResponseType(typeof(ApiResponse<PagedResult<SpecialzationDto>>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<PagedResult<SpecialzationDto>>), StatusCodes.Status400BadRequest)]
        [HttpGet(ApiRoutes.Patient.GetAllSpecificationsPaged)]
        public async Task<IActionResult> GetAllSpecificationsPaged([FromQuery] PaginationRequest pagination)
        {
            var result = await _patientService.GetAllSpecifications(pagination);
            return GetResponse(result);
        }

        /// <summary>
        /// Cancel an appointment.
        /// </summary>
        /// <remarks>
        /// Sample request:
        /// <code>
        /// DELETE /appointments/{appointmentId}
        /// </code>
        /// </remarks>
        /// <param name="appointmentId">The ID of the appointment to cancel</param>
        [ProducesResponseType(typeof(ApiResponse<bool>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<bool>), StatusCodes.Status400BadRequest)]
        [HttpDelete(ApiRoutes.Patient.CancelAppointment)]
        public async Task<IActionResult> CancelAppointment([FromRoute] string appointmentId)
        {
            var result = await _patientService.CancelAppointment(appointmentId);
            return GetResponseWithoutType(result);
        }
    }

    public class UploadProfilePictureParam
    {
        public IFormFile File { set; get; }
    }
}