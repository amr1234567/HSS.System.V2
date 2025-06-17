using HSS.System.V2.Domain.Helpers.Models;

namespace HSS.System.V2.Services.DTOs.AuthDTOs
{
    public class UserDetails
    {
        public string UserName { get; set; }
        public string UserNationalId { get; set; }
        public string UserId { get; set; }
        public string UserRole { get; set; }
        public TokenModel TokenModel { get; set; }
    }
}
