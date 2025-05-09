using System.ComponentModel.DataAnnotations;

namespace HSS.System.V2.Services.DTOs.AuthDTOs
{
    public class LogoutModelDto
    {
        [Required]
        public string RefreshToken { get; set; }
    }
}
