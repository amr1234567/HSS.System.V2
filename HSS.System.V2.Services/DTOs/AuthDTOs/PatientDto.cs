using HSS.System.V2.Domain.Enums;

using System.ComponentModel.DataAnnotations;

namespace HSS.System.V2.Services.DTOs.AuthDTOs
{
    public class PatientDto
    {
        [Required]
        [MaxLength(50)]
        public string Name { get; set; }

        [Required]
        [MinLength(14)]
        [MaxLength(14)]
        public string NationalId { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        public Gender Gender { get; set; }

        [Required]
        [MinLength(8)]
        public string Passowrd {  get; set; }

        [Required]
        [Compare(nameof(Passowrd))]
        public string ConfirmPassword { get; set; }
    }
}
