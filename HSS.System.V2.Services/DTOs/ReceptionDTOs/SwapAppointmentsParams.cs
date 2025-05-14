using System.ComponentModel.DataAnnotations;

namespace HSS.System.V2.Services.DTOs.ReceptionDTOs
{
    public record SwapAppointmentsParams([property: Required] string AppointmentId1, [property: Required] string AppointmentId2);
}