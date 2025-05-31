using System.ComponentModel.DataAnnotations;

namespace HSS.System.V2.Presentation.Models
{
    public record RadiologyAppointmentResultModel
    {
        [Required]
        public IFormFile[] Result { get; set; }
    }
}
