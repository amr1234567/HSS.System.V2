using Domain.Models.Appointments;

using SharedServices.Helpers;

namespace HSS.System.V2.Services.DTOs.ReceptionDTOs
{
    public class CreateTicketModel : IInputModel<Ticket>
    {
        public string? PatientId { get; set; }
        public string? PatientNationalId { get; set; }
        public string HospitalId { get; set; }

        public Ticket ToModel()
        {
            return new()
            {
                CreatedAt = DateTime.UtcNow,
                Id = Guid.NewGuid().ToString(),
                HospitalId = HospitalId,
            };
        }
    }
}