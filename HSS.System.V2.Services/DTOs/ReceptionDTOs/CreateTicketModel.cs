using HSS.System.V2.Domain.Models.Common;
using HSS.System.V2.Domain.Models.Prescriptions;

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
                HospitalCreatedInId = HospitalId,
            };
        }
    }
}