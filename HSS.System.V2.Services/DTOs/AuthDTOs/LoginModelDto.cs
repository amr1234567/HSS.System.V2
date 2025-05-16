using System.ComponentModel.DataAnnotations;

namespace HSS.System.V2.Services.DTOs.AuthDTOs
{
    public class LoginModelDto
    {
        [Required]
        [MinLength(14)]
        [MaxLength(14)]
        public string NationalId { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}
