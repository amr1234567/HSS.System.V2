using System.ComponentModel.DataAnnotations;

namespace HSS.System.V2.Services.DTOs.ReceptionDTOs
{
    public class CreateTicketModelForReception
    {
        [Required(ErrorMessage = "يجب إدخال رقم الهوية الوطنية أو معرف المريض")]
        public string PatientIdentifier { get; set; }

        [Required(ErrorMessage = "يجب تحديد نوع المعرف")]
        public PatientIdentifierType IdentifierType { get; set; }
    }

    public enum PatientIdentifierType
    {
        NationalId,
        PatientId
    }
} 