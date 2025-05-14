using System.ComponentModel.DataAnnotations;

namespace HSS.System.V2.Services.DTOs.ReceptionDTOs
{
    public record RescheduleAppointmentParams
    {
        [Required]
        public string AppointmentId { get; init; }

        [Required]
        public string DepartmentId { get; init; }
        [Required]
        [DataType(DataType.DateTime)]
        public DateTime NewDateTime { get; init; }
    } 
}