using System.ComponentModel.DataAnnotations;

namespace HSS.System.V2.Services.DTOs.ReceptionDTOs
{
    public class AppointmentQueueParams
    {
        [Required]
        public string AppointmentId { get; set; }

        [Required]
        public string DepartmentId { get; set; }
    }
}