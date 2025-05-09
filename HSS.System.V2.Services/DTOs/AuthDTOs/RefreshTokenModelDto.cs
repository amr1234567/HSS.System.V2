using System.ComponentModel.DataAnnotations;

namespace HSS.System.V2.Services.DTOs.AuthDTOs
{
    public class RefreshTokenModelDto
    {
        [Required]
        public string RefreshToken { get; set; }
        [Required]
        public string AccessToken { get; set; }
    }
}
