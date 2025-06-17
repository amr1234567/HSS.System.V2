using HSS.System.V2.Domain.Helpers.Models;
using HSS.System.V2.Presentation.Controllers.Base;
using HSS.System.V2.Services.Contracts;
using HSS.System.V2.Services.DTOs.PatientDTOs;
using Microsoft.AspNetCore.Mvc;
using HSS.System.V2.Domain.Attributes;
using HSS.System.V2.Domain.Enums;

namespace HSS.System.V2.Presentation.Controllers
{
    /// <summary>
    /// Controller for managing patient-related operations in the healthcare system.
    /// Provides endpoints for managing appointments, notifications, medical tests, and patient profile.
    /// </summary>
    /// <remarks>
    /// This controller handles:
    /// - Patient notifications and profile management
    /// - Appointment scheduling and management for clinics, radiology, and medical labs
    /// - Medical test results and prescriptions
    /// - Patient tickets and hospital interactions
    /// </remarks>
    [ApiExplorerSettings(GroupName = "PatientAPI")]
    [AuthorizeByEnum(UserRole.Patient)]
    public class PatientController : CustomBaseController
    {
        private readonly IPatientService _patientService;

        /// <summary>
        /// Initializes a new instance of the PatientController.
        /// </summary>
        /// <param name="patientService">The patient service for handling business logic.</param>
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
        /// Retrieves current appointments for the authenticated patient with pagination.
        /// </summary>
        /// <remarks>
        /// Sample request:
        /// <code>
        /// GET /api/patient/appointments/current?page=1&amp;pageSize=10
        /// </code>
        /// </remarks>
        /// <param name="pagination">Pagination parameters including page number and page size</param>
        /// <returns>A paged list of current appointments</returns>
        /// <response code="200">Returns the list of current appointments</response>
        /// <response code="400">If there was an error retrieving the appointments</response>
        [ProducesResponseType(typeof(ApiResponse<PagedResult<AppointmentView>>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<PagedResult<AppointmentView>>), StatusCodes.Status400BadRequest)]
        [HttpGet(ApiRoutes.Patient.GetCurrentAppointments)]
        public async Task<IActionResult> GetCurrentAppointments([FromQuery] PaginationRequest pagination)
        {
            var result = await _patientService.GetCurrentAppontments(pagination);
            return GetResponse(result);
        }

        /// <summary>
        /// Retrieves detailed information about a specific appointment.
        /// </summary>
        /// <remarks>
        /// Sample request:
        /// <code>
        /// GET /api/patient/appointments/{appointmentId}
        /// </code>
        /// </remarks>
        /// <param name="appointmentId">The unique identifier of the appointment</param>
        /// <returns>Detailed appointment information</returns>
        /// <response code="200">Returns the appointment details</response>
        /// <response code="400">If the appointment was not found or there was an error</response>
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
        /// GET /specifications?page=1&amp;pageSize=10
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
        /// Retrieves all medical lab tests with pagination.
        /// </summary>
        /// <remarks>
        /// Sample request:
        /// <code>
        /// GET /api/patient/medical-lab-tests?page=1&amp;pageSize=10
        /// </code>
        /// </remarks>
        /// <param name="pagination">Pagination parameters including page number and page size</param>
        /// <returns>A paged list of medical lab tests</returns>
        /// <response code="200">Returns the list of medical lab tests</response>
        /// <response code="400">If there was an error retrieving the tests</response>
        [ProducesResponseType(typeof(ApiResponse<PagedResult<MedicalLabTestDto>>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<PagedResult<MedicalLabTestDto>>), StatusCodes.Status400BadRequest)]
        [HttpGet(ApiRoutes.Patient.GetAllMedicalLabTests)]
        public async Task<IActionResult> GetAllMedicalLabTests([FromQuery] PaginationRequest pagination)
        {
            var result = await _patientService.GetAllMedicalLabTest(pagination);
            return GetResponse(result);
        }

        /// <summary>
        /// Retrieves all radiology tests with pagination.
        /// </summary>
        /// <remarks>
        /// Sample request:
        /// <code>
        /// GET /api/patient/radiology-tests?page=1&amp;pageSize=10
        /// </code>
        /// </remarks>
        /// <param name="pagination">Pagination parameters including page number and page size</param>
        /// <returns>A paged list of radiology tests</returns>
        /// <response code="200">Returns the list of radiology tests</response>
        /// <response code="400">If there was an error retrieving the tests</response>
        [ProducesResponseType(typeof(ApiResponse<PagedResult<RadiologyTestDto>>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<PagedResult<RadiologyTestDto>>), StatusCodes.Status400BadRequest)]
        [HttpGet(ApiRoutes.Patient.GetAllRadiologyTests)]
        public async Task<IActionResult> GetAllRadiologyTests([FromQuery] PaginationRequest pagination)
        {
            var result = await _patientService.GetAllRadiologyTest(pagination);
            return GetResponse(result);
        }

        /// <summary>
        /// Get hospitals by specialization ID with pagination.
        /// </summary>
        /// <remarks>
        /// Sample request:
        /// <code>
        /// GET /hospitals/specialization/{specializationId}?page=1&amp;pageSize=10
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
        /// GET /hospitals/radiology/{radiologyTestId}?page=1&amp;pageSize=10
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
        /// GET /hospitals/medical-lab/{medicalLabTestId}?page=1&amp;pageSize=10
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
        /// GET /hospitals/{hospitalId}/tickets/active?page=1&amp;pageSize=10
        /// </code>
        /// </remarks>
        /// <param name="pagination">Pagination parameters</param>
        [ProducesResponseType(typeof(ApiResponse<PagedResult<TicketViewDto>>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<PagedResult<TicketViewDto>>), StatusCodes.Status400BadRequest)]
        [HttpGet(ApiRoutes.Patient.GetActiveTicketsInHospital)]
        public async Task<IActionResult> GetActiveTicketsInHospital([FromQuery] PaginationRequest pagination)
        {
            var result = await _patientService.GetActiveTicketForPatient(pagination);
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
        /// Creates a new clinic appointment for the authenticated patient.
        /// </summary>
        /// <remarks>
        /// Sample request:
        /// <code>
        /// POST /api/patient/appointments/clinic
        /// {
        ///     "clinicId": "string",
        ///     "ticketId": "string",
        ///     "expectedTimeForStart": "2024-03-20T10:00:00Z"
        /// }
        /// </code>
        /// </remarks>
        /// <param name="model">The clinic appointment creation model</param>
        /// <returns>Success status of the appointment creation</returns>
        /// <response code="200">The appointment was successfully created</response>
        /// <response code="400">If the appointment data was invalid or there was an error</response>
        [ProducesResponseType(typeof(ApiResponse<bool>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<bool>), StatusCodes.Status400BadRequest)]
        [HttpPost(ApiRoutes.Patient.CreateClinicAppointment)]
        public async Task<IActionResult> CreateClinicAppointment([FromBody] CreateClinicAppointmentModelForPatient model)
        {
            var result = await _patientService.CreateClinicAppointment(model);
            return GetResponseWithoutType(result);
        }

        /// <summary>
        /// Creates a new radiology appointment for the authenticated patient.
        /// </summary>
        /// <remarks>
        /// Sample request:
        /// <code>
        /// POST /api/patient/appointments/radiology
        /// {
        ///     "radiologyCenterId": "string",
        ///     "ticketId": "string",
        ///     "expectedTimeForStart": "2024-03-20T10:00:00Z"
        /// }
        /// </code>
        /// </remarks>
        /// <param name="model">The radiology appointment creation model</param>
        /// <returns>Success status of the appointment creation</returns>
        /// <response code="200">The appointment was successfully created</response>
        /// <response code="400">If the appointment data was invalid or there was an error</response>
        [ProducesResponseType(typeof(ApiResponse<bool>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<bool>), StatusCodes.Status400BadRequest)]
        [HttpPost(ApiRoutes.Patient.CreateRadiologyAppointment)]
        public async Task<IActionResult> CreateRadiologyAppointment([FromBody] CreateRadiologyAppointmentModelForPatient model)
        {
            model.NationalId = GetNationalId();
            var result = await _patientService.CreateRadiologyAppointMent(model);
            return GetResponseWithoutType(result);
        }

        /// <summary>
        /// Creates a new medical lab appointment for the authenticated patient.
        /// </summary>
        /// <remarks>
        /// Sample request:
        /// <code>
        /// POST /api/patient/appointments/medical-lab
        /// {
        ///     "medicalLabId": "string",
        ///     "ticketId": "string",
        ///     "expectedTimeForStart": "2024-03-20T10:00:00Z"
        /// }
        /// </code>
        /// </remarks>
        /// <param name="model">The medical lab appointment creation model</param>
        /// <returns>Success status of the appointment creation</returns>
        /// <response code="200">The appointment was successfully created</response>
        /// <response code="400">If the appointment data was invalid or there was an error</response>
        [ProducesResponseType(typeof(ApiResponse<bool>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<bool>), StatusCodes.Status400BadRequest)]
        [HttpPost(ApiRoutes.Patient.CreateMedicalLabAppointment)]
        public async Task<IActionResult> CreateMedicalLabAppointment([FromBody] CreateMedicalLabAppointmentModelForPatient model)
        {
            var result = await _patientService.CreateMedicalLabAppointment(model);
            return GetResponseWithoutType(result);
        }

        /// <summary>
        /// Retrieves a list of required medical lab tests for the patient.
        /// </summary>
        /// <remarks>
        /// Sample request:
        /// <code>
        /// GET /api/patient/medical-lab-tests/required
        /// </code>
        /// </remarks>
        /// <returns>List of required medical lab tests</returns>
        /// <response code="200">Returns the list of required medical lab tests</response>
        /// <response code="400">If there was an error retrieving the tests</response>
        [ProducesResponseType(typeof(ApiResponse<List<TestRquired>>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<List<TestRquired>>), StatusCodes.Status400BadRequest)]
        [HttpGet(ApiRoutes.Patient.GetMedicalLabTestsRequired)]
        public async Task<IActionResult> GetMedicalLabTestsRequired()
        {
            var result = await _patientService.GetMedicalLabTestRequired();
            return GetResponse(result);
        }

        /// <summary>
        /// Retrieves a list of required radiology tests for the patient.
        /// </summary>
        /// <remarks>
        /// Sample request:
        /// <code>
        /// GET /api/patient/radiology-tests/required
        /// </code>
        /// </remarks>
        /// <returns>List of required radiology tests</returns>
        /// <response code="200">Returns the list of required radiology tests</response>
        /// <response code="400">If there was an error retrieving the tests</response>
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
        /// GET /prescriptions/required?page=1&amp;pageSize=10
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
        /// GET /appointments/clinic/current?page=1&amp;pageSize=10
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
        /// GET /appointments/medical-lab/current?page=1&amp;pageSize=10
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
        /// GET /appointments/radiology/current?page=1&amp;pageSize=10
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
        /// Uploads a profile picture for the authenticated patient.
        /// </summary>
        /// <remarks>
        /// Sample request:
        /// <code>
        /// POST /api/patient/profile/picture
        /// Content-Type: multipart/form-data
        /// 
        /// file=[binary data]
        /// </code>
        /// </remarks>
        /// <param name="param">The form parameters containing the profile picture file</param>
        /// <returns>Success status of the upload operation</returns>
        /// <response code="200">The profile picture was successfully uploaded</response>
        /// <response code="400">If the file was invalid or there was an error uploading</response>
        [ProducesResponseType(typeof(ApiResponse<bool>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<bool>), StatusCodes.Status400BadRequest)]
        [HttpPost(ApiRoutes.Patient.UploadProfilePicture)]
        public async Task<IActionResult> UploadProfilePicture([FromForm] UploadProfilePictureParam param)
        {
            var result = await _patientService.UploadProfilePicture(param.File);
            return GetResponseWithoutType(result);
        }

        /// <summary>
        /// Retrieves the profile information of the authenticated patient.
        /// </summary>
        /// <remarks>
        /// Sample request:
        /// <code>
        /// GET /api/patient/profile
        /// </code>
        /// </remarks>
        /// <returns>The patient's profile information</returns>
        /// <response code="200">Returns the patient's profile information</response>
        /// <response code="400">If there was an error retrieving the profile</response>
        [ProducesResponseType(typeof(ApiResponse<ProfileInformationDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<ProfileInformationDto>), StatusCodes.Status400BadRequest)]
        [HttpGet(ApiRoutes.Patient.GetPatientProfile)]
        public async Task<IActionResult> GetPatientProfileInformation()
        {
            var result = await _patientService.GetPatientProfileInformation();
            return GetResponse(result);
        }

        /// <summary>
        /// Retrieves all active tickets for the authenticated patient.
        /// </summary>
        /// <remarks>
        /// Sample request:
        /// <code>
        /// GET /api/patient/tickets/active
        /// </code>
        /// </remarks>
        /// <returns>List of active tickets</returns>
        /// <response code="200">Returns the list of active tickets</response>
        /// <response code="400">If there was an error retrieving the tickets</response>
        [ProducesResponseType(typeof(ApiResponse<List<TicketInformationDto>>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<List<TicketInformationDto>>), StatusCodes.Status400BadRequest)]
        [HttpGet(ApiRoutes.Patient.GetAllActiveTickets)]
        public async Task<IActionResult> GetAllActiveTicketsOfPatient()
        {
            var result = await _patientService.GetAllActiveTicketsOfPatient();
            return GetResponse(result);
        }

        /// <summary>
        /// Retrieves all inactive (completed or expired) tickets for the authenticated patient.
        /// </summary>
        /// <remarks>
        /// Sample request:
        /// <code>
        /// GET /api/patient/tickets/inactive
        /// </code>
        /// </remarks>
        /// <returns>List of inactive tickets</returns>
        /// <response code="200">Returns the list of inactive tickets</response>
        /// <response code="400">If there was an error retrieving the tickets</response>
        [ProducesResponseType(typeof(ApiResponse<List<TicketInformationDto>>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<List<TicketInformationDto>>), StatusCodes.Status400BadRequest)]
        [HttpGet(ApiRoutes.Patient.GetAllInactiveTickets)]
        public async Task<IActionResult> GetAllInactiveTicketsOfPatient()
        {
            var result = await _patientService.GetAllInactiviTicketsOfPatient();
            return GetResponse(result);
        }

        /// <summary>
        /// Retrieves the content (appointments, tests, etc.) of a specific ticket with pagination.
        /// </summary>
        /// <remarks>
        /// Sample request:
        /// <code>
        /// GET /api/patient/tickets/{ticketId}/content?page=1&amp;pageSize=10
        /// </code>
        /// </remarks>
        /// <param name="ticketId">The unique identifier of the ticket</param>
        /// <param name="pagination">Pagination parameters including page number and page size</param>
        /// <returns>A paged list of ticket content items</returns>
        /// <response code="200">Returns the ticket content</response>
        /// <response code="400">If the ticket was not found or there was an error</response>
        [ProducesResponseType(typeof(ApiResponse<PagedResult<AppointmentView>>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<PagedResult<AppointmentView>>), StatusCodes.Status400BadRequest)]
        [HttpGet(ApiRoutes.Patient.GetTicketContent)]
        public async Task<IActionResult> GetTicketContent([FromRoute] string ticketId, [FromQuery] PaginationRequest pagination)
        {
            var result = await _patientService.GetTicketContent(ticketId, pagination);
            return GetResponse(result);
        }

        /// <summary>
        /// Retrieves the content of a specific appointment.
        /// </summary>
        /// <remarks>
        /// Sample request:
        /// <code>
        /// GET /api/patient/appointments/{appointmentId}/content
        /// </code>
        /// </remarks>
        /// <param name="appointmentId">The unique identifier of the appointment</param>
        /// <returns>The appointment content details</returns>
        /// <response code="200">Returns the appointment content</response>
        /// <response code="400">If the appointment was not found or there was an error</response>
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
        [HttpGet(ApiRoutes.Patient.GetAppointmentContent)]
        public async Task<IActionResult> GetAppointmentContent([FromRoute] string appointmentId)
        {
            var result = await _patientService.GetAppointmentContent(appointmentId);
            return GetResponse(result);
        }

        /// <summary>
        /// Retrieves all medical specializations with pagination.
        /// </summary>
        /// <remarks>
        /// Sample request:
        /// <code>
        /// GET /api/patient/specifications/paged?page=1&amp;pageSize=10
        /// </code>
        /// </remarks>
        /// <param name="pagination">Pagination parameters including page number and page size</param>
        /// <returns>A paged list of medical specializations</returns>
        /// <response code="200">Returns the list of specializations</response>
        /// <response code="400">If there was an error retrieving the specializations</response>
        [ProducesResponseType(typeof(ApiResponse<PagedResult<SpecialzationDto>>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<PagedResult<SpecialzationDto>>), StatusCodes.Status400BadRequest)]
        [HttpGet(ApiRoutes.Patient.GetAllSpecilizations)]
        public async Task<IActionResult> GetAllSpecificationsPaged([FromQuery] PaginationRequest pagination)
        {
            var result = await _patientService.GetAllSpecifications(pagination);
            return GetResponse(result);
        }

        /// <summary>
        /// Cancels a specific appointment.
        /// </summary>
        /// <remarks>
        /// Sample request:
        /// <code>
        /// DELETE /api/patient/appointments/{appointmentId}
        /// </code>
        /// </remarks>
        /// <param name="appointmentId">The unique identifier of the appointment to cancel</param>
        /// <returns>Success status of the cancellation</returns>
        /// <response code="200">The appointment was successfully cancelled</response>
        /// <response code="400">If the appointment was not found or there was an error</response>
        [ProducesResponseType(typeof(ApiResponse<bool>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<bool>), StatusCodes.Status400BadRequest)]
        [HttpDelete(ApiRoutes.Patient.CancelAppointment)]
        public async Task<IActionResult> CancelAppointment([FromRoute] string appointmentId)
        {
            var result = await _patientService.CancelAppointment(appointmentId);
            return GetResponseWithoutType(result);
        }

        /// <summary>
        /// Get all details about the appointment after creation
        /// </summary>
        /// <remarks>
        /// Sample request:
        /// <code>
        /// GET /appointment/last-stage/{appointmentId}
        /// </code>
        /// </remarks>
        [ProducesResponseType(typeof(ApiResponse<FinalStepBookingAppointmentDetails>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<FinalStepBookingAppointmentDetails>), StatusCodes.Status400BadRequest)]
        [HttpGet(ApiRoutes.Patient.GetLastStageAppointment)]
        public async Task<IActionResult> GetFinalStepBookingAppointmentDetailsAsync([FromRoute] string appointmentId)
        {
            var result = await _patientService.GetFinalStepBookingAppointmentDetailsAsync(appointmentId);
            return GetResponse(result);
        }

        /// <summary>
        /// Get all details about the appointment after creation
        /// </summary>
        /// <remarks>
        /// Sample request:
        /// <code>
        /// GET /tests-required/{testRequiredId}
        /// </code>
        /// </remarks>
        [ProducesResponseType(typeof(ApiResponse<TestRquiredForPatientDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<TestRquiredForPatientDto>), StatusCodes.Status400BadRequest)]
        [HttpGet(ApiRoutes.Patient.GetTestRequiredById)]
        public async Task<IActionResult> GetTestRequiredById([FromRoute] string testRequiredId)
        {
            var result = await _patientService.GetTestRequiredById(testRequiredId);
            return GetResponse(result);
        }
    }

    /// <summary>
    /// Parameters for uploading a patient's profile picture.
    /// </summary>
    public class UploadProfilePictureParam
    {
        /// <summary>
        /// The profile picture file to upload.
        /// </summary>
        public IFormFile File { set; get; }
    }
}