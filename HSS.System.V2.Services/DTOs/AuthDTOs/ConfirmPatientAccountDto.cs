using HSS.System.V2.Domain.Helpers.Models;

namespace HSS.System.V2.Services.DTOs.AuthDTOs
{
    public record ConfirmPatientAccountDto
    {
        public string NationalId { get; set; }
        public string Password { get; set; }
        public TokenModel Token { get; set; }
    }
}
