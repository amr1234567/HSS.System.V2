using HSS.System.V2.Application.DTOs.Clinic;
using HSS.System.V2.Application.Interfaces;
using HSS.System.V2.DataAccess.Contracts;
using HSS.System.V2.Services.DTOs.ReceptionDTOs;
using Microsoft.AspNetCore.Mvc;

namespace HSS.System.V2.Controllers
{
    [Route("api/v1/clinics")]
    [ApiController]
    public class ClinicController : ControllerBase
    {
        private readonly IClinicServices _clinicService;

        public ClinicController(IClinicServices clinicService)
        {
            _clinicService = clinicService;
        }

       
        // Get clinic details
 
        [HttpGet("{clinicId}")]
        public async Task<ActionResult<ClinicDto>> GetClinicDetails(int clinicId)
        {
            var result = await _clinicService.GetClinicDetailsAsync(clinicId);
            return Ok(result);
        }

     
        // Get comprehensive current clinic details
       
        [HttpGet("{clinicId}/current-details")]
        public async Task<ActionResult<CurrentClinicDetailsDto>> GetCurrentClinicDetails(int clinicId)
        {
            var result = await _clinicService.GetCurrentClinicDetailsAsync(clinicId);
            return Ok(result);
        }

     
        // Get all medical histories for clinic
       
        [HttpGet("{clinicId}/medical-histories")]
        public async Task<ActionResult<List<MedicalHistoryDto>>> GetClinicMedicalHistories(int clinicId)
        {
            var result = await _clinicService.GetMedicalHistoriesAsync(clinicId);
            return Ok(result);
        }

       
        // Get specific medical history
     
        [HttpGet("{clinicId}/medical-histories/{medicalHistoryId}")]
        public async Task<ActionResult<MedicalHistoryDto>> GetMedicalHistoryById(int clinicId, int medicalHistoryId)
        {
            var result = await _clinicService.GetMedicalHistoryByIdAsync(clinicId, medicalHistoryId);
            return Ok(result);
        }

       
        // Submit clinic results
    
        [HttpPost("{clinicId}/results")]
        public async Task<IActionResult> SubmitClinicResult(int clinicId, [FromBody] ClinicResultRequestDto request)
        {
            await _clinicService.SubmitClinicResultAsync(clinicId, request);
            return Ok(new { Success = true });
        }

      
        // End an appointment
       
        [HttpPost("{clinicId}/appointments/{appointmentId}/end")]
        public async Task<IActionResult> EndAppointment(int clinicId, int appointmentId)
        {
            await _clinicService.EndAppointmentAsync(clinicId, appointmentId);
            return Ok(new { Success = true });
        }
    }
}
