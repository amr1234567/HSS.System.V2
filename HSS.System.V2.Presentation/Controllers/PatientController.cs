using FluentResults;

using HSS.System.V2.Domain.Helpers.Models;
using HSS.System.V2.Domain.ResultHelpers.Errors;
using HSS.System.V2.Presentation.Controllers.Base;
using HSS.System.V2.Services.Contracts;
using HSS.System.V2.Services.DTOs.PatientDTOs;

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

        [HttpGet(ApiRoutes.Patient.NotificationCount)]
        public async Task<IActionResult> NotificationCount()
        {
            var result = await _patientService.NotificationCount();
            return GetResponse(result);
        }

        [HttpGet(ApiRoutes.Patient.GetNotifications)]
        public async Task<IActionResult> GetNotifications()
        {
            var result = await _patientService.GetNotifications();
            return GetResponse(result);
        }

        [HttpGet(ApiRoutes.Patient.GetNotification)]
        public async Task<IActionResult> GetNotificationData([FromRoute] string notificationId)
        {
            var result = await _patientService.GetNotificationData(notificationId);
            return GetResponse(result);
        }

        [HttpPut(ApiRoutes.Patient.SeenNotification)]
        public async Task<IActionResult> SeenNotification([FromRoute] string notificationId)
        {
            var result = await _patientService.SeenNotification(notificationId);
            return GetResponseWithoutType(result);
        }

        [HttpPost(ApiRoutes.Patient.SugerTest)]
        public async Task<IActionResult> SugerTest([FromBody] DiabetesTestModel model)
        {
            var result = await _patientService.SugerTest(model);
            return GetResponse(result);
        }

        [HttpGet(ApiRoutes.Patient.GetCurrentAppointments)]
        public async Task<IActionResult> GetCurrentAppointments([FromQuery] PaginationRequest pagination)
        {
            var result = await _patientService.GetCurrentAppontments(pagination);
            return GetResponse(result);
        }

        [HttpGet(ApiRoutes.Patient.GetAppointmentDetails)]
        public async Task<IActionResult> GetAppointmentDetails([FromRoute] string appointmentId)
        {
            var result = await _patientService.GetAppointmentDetails(appointmentId);
            return GetResponse(result);
        }

        [HttpGet(ApiRoutes.Patient.GetAllSpecifications)]
        public async Task<IActionResult> GetAllSpecifications([FromQuery] PaginationRequest pagination)
        {
            var result = await _patientService.GetAllSpecifications(pagination);
            return GetResponse(result);
        }

        [HttpGet(ApiRoutes.Patient.GetAllRadiologyTests)]
        public async Task<IActionResult> GetAllRadiologyTests([FromQuery] PaginationRequest pagination)
        {
            var result = await _patientService.GetAllRadiologyTest(pagination);
            return GetResponse(result);
        }

        [HttpGet(ApiRoutes.Patient.GetAllMedicalLabTests)]
        public async Task<IActionResult> GetAllMedicalLabTests([FromQuery] PaginationRequest pagination)
        {
            var result = await _patientService.GetAllMedicalLabTest(pagination);
            return GetResponse(result);
        }

        [HttpGet(ApiRoutes.Patient.GetHospitalsBySpecificationId)]
        public async Task<IActionResult> GetHospitalsBySpecificationId([FromRoute] string specializationId, [FromQuery] PaginationRequest pagination)
        {
            var result = await _patientService.GetHospitalsBySpecificationId(specializationId, pagination);
            return GetResponse(result);
        }

        [HttpGet(ApiRoutes.Patient.GetHospitalsByRadiologyTestId)]
        public async Task<IActionResult> GetHospitalsByRadiologyTestId([FromRoute] string radiologyTestId, [FromQuery] PaginationRequest pagination)
        {
            var result = await _patientService.GetHospitalsByRadiologyTestId(radiologyTestId, pagination);
            return GetResponse(result);
        }

        [HttpGet(ApiRoutes.Patient.GetHospitalsByMedicalLabTestId)]
        public async Task<IActionResult> GetHospitalsByMedicalLabTestId([FromRoute] string medicalLabTestId, [FromQuery] PaginationRequest pagination)
        {
            var result = await _patientService.GetHospitalsByMedicalLabTestId(medicalLabTestId, pagination);
            return GetResponse(result);
        }

        [HttpGet(ApiRoutes.Patient.GetActiveTicketsInHospital)]
        public async Task<IActionResult> GetActiveTicketsInHospital([FromRoute] string hospitalId, [FromQuery] PaginationRequest pagination)
        {
            var result = await _patientService.GetActiveTicketInHospital(hospitalId, pagination);
            return GetResponse(result);
        }

        [HttpGet(ApiRoutes.Patient.GetAllActiveTicketsInHospital)]
        public async Task<IActionResult> GetAllActiveTicketsInHospital([FromRoute] string hospitalId, [FromQuery] PaginationRequest pagination)
        {
            var result = await _patientService.GetActiveTicketInHospital(hospitalId, pagination);
            return GetResponse(result);
        }

        [HttpGet(ApiRoutes.Patient.GetClinics)]
        public async Task<IActionResult> GetClinics([FromRoute] string hospitalId, [FromRoute] string specificationId)
        {
            var result = await _patientService.GetClinics(hospitalId, specificationId);
            return GetResponse(result);
        }

        [HttpGet(ApiRoutes.Patient.GetRadiologyCenters)]
        public async Task<IActionResult> GetRadiologyCenters([FromRoute] string hospitalId, [FromRoute] string testId)
        {
            var result = await _patientService.GetRadiologyCenter(hospitalId, testId);
            return GetResponse(result);
        }

        [HttpGet(ApiRoutes.Patient.GetMedicalLabs)]
        public async Task<IActionResult> GetMedicalLabs([FromRoute] string hospitalId, [FromRoute] string testId)
        {
            var result = await _patientService.GetMedicalLabs(hospitalId, testId);
            return GetResponse(result);
        }

        [HttpPost(ApiRoutes.Patient.CreateTicket)]
        public async Task<IActionResult> CreateTicket([FromRoute] string hospitalId)
        {
            var result = await _patientService.CreateTicket(hospitalId);
            return GetResponseWithoutType(result);
        }

        [HttpPost(ApiRoutes.Patient.CreateClinicAppointment)]
        public async Task<IActionResult> CreateClinicAppointment([FromBody] CreateClinicAppointmentModel model)
        {
            var result = await _patientService.CreateClinicAppointment(model);
            return GetResponseWithoutType(result);
        }

        [HttpPost(ApiRoutes.Patient.CreateRadiologyAppointment)]
        public async Task<IActionResult> CreateRadiologyAppointment([FromBody] CreateRadiologyAppointmentModel model)
        {
            var result = await _patientService.CreateRadiologyAppointMent(model);
            return GetResponseWithoutType(result);
        }

        [HttpPost(ApiRoutes.Patient.CreateMedicalLabAppointment)]
        public async Task<IActionResult> CreateMedicalLabAppointment([FromBody] CreateMedicalLabAppointmentModel model)
        {
            var result = await _patientService.CreateMedicalLabAppointment(model);
            return GetResponseWithoutType(result);
        }

        [HttpGet(ApiRoutes.Patient.GetMedicalLabTestsRequired)]
        public async Task<IActionResult> GetMedicalLabTestsRequired()
        {
            var result = await _patientService.GetMedicalLabTestRequired();
            return GetResponse(result);
        }

        [HttpGet(ApiRoutes.Patient.GetRadiologyTestsRequired)]
        public async Task<IActionResult> GetRadiologyTestsRequired()
        {
            var result = await _patientService.GetRadiologyTestRequired();
            return GetResponse(result);
        }

        [HttpGet(ApiRoutes.Patient.GetAllPrescriptionsRequired)]
        public async Task<IActionResult> GetAllPrescriptionsRequired([FromQuery] PaginationRequest pagination)
        {
            var result = await _patientService.GetAllPrescriptionsRequired(pagination);
            return GetResponse(result);
        }

        [HttpGet(ApiRoutes.Patient.GetMedicineByPrescriptionId)]
        public async Task<IActionResult> GetMedicineByPrescriptionId([FromRoute] string prescriptionId)
        {
            var result = await _patientService.GetMedicineByPrescriptionId(prescriptionId);
            return GetResponse(result);
        }

        [HttpGet(ApiRoutes.Patient.GetCurrentClinicAppointments)]
        public async Task<IActionResult> GetCurrentClinicAppointments([FromQuery] PaginationRequest pagination)
        {
            var result = await _patientService.GetCurrentClinicAppointments(pagination);
            return GetResponse(result);
        }

        [HttpGet(ApiRoutes.Patient.GetCurrentMedicalLabAppointments)]
        public async Task<IActionResult> GetCurrentMedicalLabAppointments([FromQuery] PaginationRequest pagination)
        {
            var result = await _patientService.GetCurrentMedicalLabAppointments(pagination);
            return GetResponse(result);
        }

        [HttpGet(ApiRoutes.Patient.GetCurrentRadiologyCenterAppointments)]
        public async Task<IActionResult> GetCurrentRadiologyCenterAppointments([FromQuery] PaginationRequest pagination)
        {
            var result = await _patientService.GetCurrentRadiologyCenterAppointments(pagination);
            return GetResponse(result);
        }

        [HttpPost(ApiRoutes.Patient.UploadProfilePicture)]
        public async Task<IActionResult> UploadProfilePicture([FromForm] UploadProfilePictureParam param)
        {
            var result = await _patientService.UploadProfilePicture(param.File);
            return GetResponseWithoutType(result);
        }

        [HttpGet(ApiRoutes.Patient.GetPatientProfile)]
        public async Task<IActionResult> GetPatientProfileInformation()
        {
            var result = await _patientService.GetPatientProfileInformation();
            return GetResponse(result);
        }

        [HttpGet(ApiRoutes.Patient.GetAllActiveTickets)]
        public async Task<IActionResult> GetAllActiveTicketsOfPatient()
        {
            var result = await _patientService.GetAllActiveTicketsOfPatient();
            return GetResponse(result);
        }

        [HttpGet(ApiRoutes.Patient.GetAllInactiveTickets)]
        public async Task<IActionResult> GetAllInactiveTicketsOfPatient()
        {
            var result = await _patientService.GetAllInactiviTicketsOfPatient();
            return GetResponse(result);
        }

        [HttpGet(ApiRoutes.Patient.GetTicketContent)]
        public async Task<IActionResult> GetTicketContent([FromRoute] string ticketId, [FromQuery] PaginationRequest pagination)
        {
            var result = await _patientService.GetTicketContent(ticketId, pagination);
            return GetResponse(result);
        }

        [HttpGet(ApiRoutes.Patient.GetAppointmentContent)]
        public async Task<IActionResult> GetAppointmentContent([FromRoute] string appointmentId)
        {
            var result = await _patientService.GetAppointmentContent(appointmentId);
            return GetResponse(result);
        }

        [HttpGet(ApiRoutes.Patient.GetAllSpecificationsPaged)]
        public async Task<IActionResult> GetAllSpecificationsPaged([FromQuery] PaginationRequest pagination)
        {
            var result = await _patientService.GetAllSpecifications(pagination);
            return GetResponse(result);
        }

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