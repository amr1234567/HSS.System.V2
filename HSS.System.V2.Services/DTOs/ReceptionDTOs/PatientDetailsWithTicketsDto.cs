using HSS.System.V2.Domain.Helpers.Models;

namespace HSS.System.V2.Services.DTOs.ReceptionDTOs
{
    public class PatientDetailsWithTicketsDto
    {
        public string PatientName { get; set; }
        public string PatientNationalId { get; set; }
        public string PatientId { get; set; }
        public PagedResult<TicketDto> Tickets { get; set; }
    }
}