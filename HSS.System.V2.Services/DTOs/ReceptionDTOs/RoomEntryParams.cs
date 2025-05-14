using System.ComponentModel.DataAnnotations;

namespace HSS.System.V2.Services.DTOs.ReceptionDTOs
{
    public record RoomEntryParams([property: Required] string RoomId, [property: Required] string NationalId);
}