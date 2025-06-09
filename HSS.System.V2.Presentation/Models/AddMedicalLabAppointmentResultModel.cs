using HSS.System.V2.Services.DTOs.MedicalLabDTOs;

using System.ComponentModel.DataAnnotations;

namespace HSS.System.V2.Presentation.Models
{
    public record AddMedicalLabAppointmentResultModel
    {
        [Required]
        public List<TestResultDto> Results {  get; set; }
    }
}
