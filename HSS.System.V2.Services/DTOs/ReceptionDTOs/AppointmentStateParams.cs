using System.ComponentModel.DataAnnotations;

namespace HSS.System.V2.Services.DTOs.ReceptionDTOs
{
    public class AppointmentStateParams
    {
        [Required]
        public string AppointmentId { set; get; }
        [Required]
        [AllowedValues("NotStarted", "InProgress", "Ended")]
        public string NewState { set; get; }
    }
}