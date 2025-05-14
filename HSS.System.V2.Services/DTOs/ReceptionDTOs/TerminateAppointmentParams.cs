using System.ComponentModel.DataAnnotations;

namespace HSS.System.V2.Services.DTOs.ReceptionDTOs
{
    public class TerminateAppointmentParams
    {
        [Required]
        public string AppointmentId { set; get; }
    }
}