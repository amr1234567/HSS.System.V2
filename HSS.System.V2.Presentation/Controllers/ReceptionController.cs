using HSS.System.V2.Presentation.Controllers.Base;
using HSS.System.V2.Services.Contracts;
using HSS.System.V2.Services.DTOs.ReceptionDTOs;
using HSS.System.V2.Domain.Models.Requests;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using HSS.System.V2.Domain.Attributes;
using HSS.System.V2.Domain.Enums;

namespace HSS.System.V2.Presentation.Controllers
{
    [AuthorizeByEnum(UserRole.Receptionist)]
    [ApiExplorerSettings(GroupName = "RecpetionAPI")]
    public class ReceptionController : CustomBaseController
    {
        private readonly IReceptionServices _receptionServices;

        public ReceptionController(IReceptionServices receptionServices)
        {
            _receptionServices = receptionServices;
        }

        [HttpGet(ApiRoutes.Reception.GetAllHospitalDepartments)]
        public async Task<IActionResult> GetAllHospitalDepartmentsInHospital([FromRoute] string hospitalId)
        {
            var result = await _receptionServices.GetAllHospitalDepartmentsInHospital(hospitalId);
            return GetResponse(result);
        }

        [HttpGet(ApiRoutes.Reception.GetAllSpecializations)]
        public async Task<IActionResult> GetAllSpecializationsInHospital([FromRoute] string hospitalId, [FromQuery] PaginationRequest pagination)
        {
            var result = await _receptionServices.GetAllSpecializationsInHospital(hospitalId, pagination.Page, pagination.PageSize);
            return GetResponse(result);
        }

        [HttpGet(ApiRoutes.Reception.GetAllClinics)]
        public async Task<IActionResult> GetAllClinics([FromRoute] string specializationId, [FromRoute] string hospitalId, [FromQuery] PaginationRequest pagination)
        {
            var result = await _receptionServices.GetAllClinics(specializationId, hospitalId, pagination.Page, pagination.PageSize);
            return GetResponse(result);
        }

        [HttpGet(ApiRoutes.Reception.GetAllRadiologyCenters)]
        public async Task<IActionResult> GetAllRadiologyCenters([FromRoute] string hospitalId, [FromQuery] PaginationRequest pagination)
        {
            var result = await _receptionServices.GetAllRadiologyCenters(hospitalId, pagination.Page, pagination.PageSize);
            return GetResponse(result);
        }

        [HttpGet(ApiRoutes.Reception.GetAllRadiologyCentersDoTest)]
        public async Task<IActionResult> GetAllRadiologyCentersDoTest([FromRoute] string hospitalId, [FromRoute] string radiologyTestId, [FromQuery] PaginationRequest pagination)
        {
            var result = await _receptionServices.GetAllRadiologyCentersDoTest(hospitalId, radiologyTestId, pagination.Page, pagination.PageSize);
            return GetResponse(result);
        }

        [HttpGet(ApiRoutes.Reception.GetAllMedicalLabs)]
        public async Task<IActionResult> GetAllMedicalLabs([FromRoute] string hospitalId, [FromQuery] PaginationRequest pagination)
        {
            var result = await _receptionServices.GetAllMedicalLabs(hospitalId, pagination.Page, pagination.PageSize);
            return GetResponse(result);
        }

        [HttpGet(ApiRoutes.Reception.GetAllMedicalLabsDoTest)]
        public async Task<IActionResult> GetAllMedicalLabsDoTest([FromRoute] string hospitalId, [FromRoute] string medicalTestId, [FromQuery] PaginationRequest pagination)
        {
            var result = await _receptionServices.GetAllMedicalLabsDoTest(hospitalId, medicalTestId, pagination.Page, pagination.PageSize);
            return GetResponse(result);
        }

        [HttpGet(ApiRoutes.Reception.GetAllAppointmentsForClinic)]
        public async Task<IActionResult> GetAllAppointmentsForClinic([FromRoute] string clinicId, [FromQuery] AppointmentFilterRequest filter, [FromQuery] PaginationRequest pagination)
        {
            var result = await _receptionServices.GetAllAppointmentsForClinic(clinicId, filter.DateFrom, filter.DateTo, pagination.Page, pagination.PageSize);
            return GetResponse(result);
        }

        [HttpGet(ApiRoutes.Reception.GetQueueForClinic)]
        public async Task<IActionResult> GetQueueForClinic([FromRoute] string clinicId, [FromQuery] PaginationRequest pagination)
        {
            var result = await _receptionServices.GetQueueForClinic(clinicId, pagination.Page, pagination.PageSize);
            return GetResponse(result);
        }

        [HttpGet(ApiRoutes.Reception.GetAllAppointmentsForRadiologyCenter)]
        public async Task<IActionResult> GetAllAppointmentsForRadiologyCenter([FromRoute] string radiologyCenterId, [FromQuery] AppointmentFilterRequest filter, [FromQuery] PaginationRequest pagination)
        {
            var result = await _receptionServices.GetAllAppointmentsForRadiologyCenter(radiologyCenterId, filter.DateFrom, filter.DateTo, pagination.Page, pagination.PageSize);
            return GetResponse(result);
        }

        [HttpGet(ApiRoutes.Reception.GetQueueForRadiologyCenter)]
        public async Task<IActionResult> GetQueueForRadiologyCenter([FromRoute] string radiologyCenterId, [FromQuery] PaginationRequest pagination)
        {
            var result = await _receptionServices.GetQueueForRadiologyCenter(radiologyCenterId, pagination.Page, pagination.PageSize);
            return GetResponse(result);
        }

        [HttpGet(ApiRoutes.Reception.GetAllAppointmentsForMedicalLab)]
        public async Task<IActionResult> GetAllAppointmentsForMedicalLab([FromRoute] string medicalLabId, [FromQuery] AppointmentFilterRequest filter, [FromQuery] PaginationRequest pagination)
        {
            var result = await _receptionServices.GetAllAppointmentsForMedicalLab(medicalLabId, filter.DateFrom, filter.DateTo, pagination.Page, pagination.PageSize);
            return GetResponse(result);
        }

        [HttpGet(ApiRoutes.Reception.GetQueueForMedicalLab)]
        public async Task<IActionResult> GetQueueForMedicalLab([FromRoute] string medicalLabId, [FromQuery] PaginationRequest pagination)
        {
            var result = await _receptionServices.GetQueueForMedicalLab(medicalLabId, pagination.Page, pagination.PageSize);
            return GetResponse(result);
        }

        [HttpGet(ApiRoutes.Reception.GetOpenTicketsByNationalId)]
        public async Task<IActionResult> GetOpenTicketsForPatientByNationalId([FromRoute] string nationalId, [FromQuery] PaginationRequest pagination)
        {
            var result = await _receptionServices.GetOpenTicketsForPatientByNationalId(nationalId, pagination.Page, pagination.PageSize);
            return GetResponse(result);
        }

        [HttpGet(ApiRoutes.Reception.GetOpenTicketsByPatientId)]
        public async Task<IActionResult> GetOpenTicketsForPatientById([FromRoute] string patientId, [FromQuery] PaginationRequest pagination)
        {
            var result = await _receptionServices.GetOpenTicketsForPatientById(patientId, pagination.Page, pagination.PageSize);
            return GetResponse(result);
        }

        [HttpPost(ApiRoutes.Reception.CreateClinicAppointment)]
        public async Task<IActionResult> CreateClinicAppointment([FromBody] CreateClinicAppointmentModelForReception model)
        {
            var result = await _receptionServices.CreateAppointment(model);
            return GetResponseWithoutType(result);
        }

        [HttpPost(ApiRoutes.Reception.CreateRadiologyAppointment)]
        public async Task<IActionResult> CreateRadiologyAppointment([FromBody] CreateRadiologyAppointmentModelForReception model)
        {
            var result = await _receptionServices.CreateAppointment(model);
            return GetResponseWithoutType(result);
        }

        [HttpPost(ApiRoutes.Reception.CreateMedicalLabAppointment)]
        public async Task<IActionResult> CreateMedicalLabAppointment([FromBody] CreateMedicalLabAppointmentModelForReception model)
        {
            var result = await _receptionServices.CreateAppointment(model);
            return GetResponseWithoutType(result);
        }

        [HttpPost(ApiRoutes.Reception.TerminateAppointment)]
        public async Task<IActionResult> TerminateAppointment([FromRoute] string appointmentId)
        {
            var result = await _receptionServices.TerminateAppointment(appointmentId);
            return GetResponseWithoutType(result);
        }

        [HttpPost(ApiRoutes.Reception.SwapClinicAppointments)]
        public async Task<IActionResult> SwapClinicAppointments([FromRoute] string appointmentId1, [FromRoute] string appointmentId2)
        {
            var result = await _receptionServices.SwapClinicAppointments(appointmentId1, appointmentId2);
            return GetResponseWithoutType(result);
        }

        [HttpPost(ApiRoutes.Reception.SwapMedicalLabAppointments)]
        public async Task<IActionResult> SwapMedicalLabAppointments([FromRoute] string appointmentId1, [FromRoute] string appointmentId2)
        {
            var result = await _receptionServices.SwapMedicalLabAppointments(appointmentId1, appointmentId2);
            return GetResponseWithoutType(result);
        }

        [HttpPost(ApiRoutes.Reception.SwapRadiologyCenterAppointments)]
        public async Task<IActionResult> SwapRadiologyCenterAppointments([FromRoute] string appointmentId1, [FromRoute] string appointmentId2)
        {
            var result = await _receptionServices.SwapRadiologyCenterAppointments(appointmentId1, appointmentId2);
            return GetResponseWithoutType(result);
        }

        [HttpPost(ApiRoutes.Reception.RescheduleClinicAppointment)]
        public async Task<IActionResult> RescheduleClinicAppointment([FromRoute] string appointmentId, [FromRoute] string departmentId, [FromQuery] DateTime newDateTime)
        {
            var result = await _receptionServices.RescheduleClinicAppointment(appointmentId, departmentId, newDateTime);
            return GetResponseWithoutType(result);
        }

        [HttpPost(ApiRoutes.Reception.RescheduleMedicalLabAppointment)]
        public async Task<IActionResult> RescheduleMedicalLabAppointment([FromRoute] string appointmentId, [FromRoute] string departmentId, [FromQuery] DateTime newDateTime)
        {
            var result = await _receptionServices.RescheduleMedicalLabAppointment(appointmentId, departmentId, newDateTime);
            return GetResponseWithoutType(result);
        }

        [HttpPost(ApiRoutes.Reception.RescheduleRadiologyAppointment)]
        public async Task<IActionResult> RescheduleRadiologyAppointment([FromRoute] string appointmentId, [FromRoute] string departmentId, [FromQuery] DateTime newDateTime)
        {
            var result = await _receptionServices.RescheduleRadiologyAppointment(appointmentId, departmentId, newDateTime);
            return GetResponseWithoutType(result);
        }

        [HttpPost(ApiRoutes.Reception.RemoveClinicAppointmentFromQueue)]
        public async Task<IActionResult> RemoveClinicAppointmentFromQueue([FromRoute] string appointmentId)
        {
            var result = await _receptionServices.RemoveClinicAppointmentFromQueue(appointmentId);
            return GetResponseWithoutType(result);
        }

        [HttpPost(ApiRoutes.Reception.RemoveMedicalLabAppointmentFromQueue)]
        public async Task<IActionResult> RemoveMedicalLabAppointmentFromQueue([FromRoute] string appointmentId)
        {
            var result = await _receptionServices.RemoveMedicalLabAppointmentFromQueue(appointmentId);
            return GetResponseWithoutType(result);
        }

        [HttpPost(ApiRoutes.Reception.RemoveRadiologyCenterAppointmentFromQueue)]
        public async Task<IActionResult> RemoveRadiologyCenterAppointmentFromQueue([FromRoute] string appointmentId)
        {
            var result = await _receptionServices.RemoveRadiologyCenterAppointmentFromQueue(appointmentId);
            return GetResponseWithoutType(result);
        }

        [HttpPost(ApiRoutes.Reception.AddClinicAppointmentForQueue)]
        public async Task<IActionResult> AddClinicAppointmentForQueue([FromRoute] string appointmentId, [FromRoute] string departmentId)
        {
            var result = await _receptionServices.AddClinicAppointmentForQueue(appointmentId, departmentId);
            return GetResponseWithoutType(result);
        }

        [HttpPost(ApiRoutes.Reception.AddMedicalLabAppointmentForQueue)]
        public async Task<IActionResult> AddMedicalLabAppointmentForQueue([FromRoute] string appointmentId, [FromRoute] string departmentId)
        {
            var result = await _receptionServices.AddMedicalLabAppointmentForQueue(appointmentId, departmentId);
            return GetResponseWithoutType(result);
        }

        [HttpPost(ApiRoutes.Reception.AddRadiologyCenterAppointmentForQueue)]
        public async Task<IActionResult> AddRadiologyCenterAppointmentForQueue([FromRoute] string appointmentId, [FromRoute] string departmentId)
        {
            var result = await _receptionServices.AddRadiologyCenterAppointmentForQueue(appointmentId, departmentId);
            return GetResponseWithoutType(result);
        }

        [HttpPost(ApiRoutes.Reception.CreateNewTicket)]
        public async Task<IActionResult> CreateNewTicket([FromBody] CreateTicketModel model)
        {
            var result = await _receptionServices.CreateNewTicket(model);
            return GetResponseWithoutType(result);
        }

        [HttpGet(ApiRoutes.Reception.GetAvailableTimeSlotsForClinic)]
        public async Task<IActionResult> GetAvailableTimeSlotsForClinic([FromRoute] string clinicId, [FromQuery] DateTime? date)
        {
            var result = await _receptionServices.GetAvailableTimeSlotsForClinic(clinicId, date);
            return GetResponse(result);
        }

        [HttpGet(ApiRoutes.Reception.GetAvailableTimeSlotsForMedicalLab)]
        public async Task<IActionResult> GetAvailableTimeSlotsForMedicalLab([FromRoute] string medicalLabId, [FromQuery] DateTime? date)
        {
            var result = await _receptionServices.GetAvailableTimeSlotsForMedicalLab(medicalLabId, date);
            return GetResponse(result);
        }

        [HttpGet(ApiRoutes.Reception.GetAvailableTimeSlotsForRadiologyCenter)]
        public async Task<IActionResult> GetAvailableTimeSlotsForRadiologyCenter([FromRoute] string radiologyCenterId, [FromQuery] DateTime? date)
        {
            var result = await _receptionServices.GetAvailableTimeSlotsForRadiologyCenter(radiologyCenterId, date);
            return GetResponse(result);
        }

        [HttpPost(ApiRoutes.Reception.StartClinicAppointment)]
        public async Task<IActionResult> StartClinicAppointment([FromRoute] string appointmentId)
        {
            var result = await _receptionServices.StartClinicAppointment(appointmentId);
            return GetResponseWithoutType(result);
        }
    }
}
