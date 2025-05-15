using System.ComponentModel.DataAnnotations;

namespace HSS.System.V2.Domain.Models.Requests
{
    public class FilterRequest
    {
        [Required(ErrorMessage = "معرف المستشفى مطلوب")]
        public string HospitalId { get; set; } = string.Empty;

        [Required(ErrorMessage = "معرف القسم مطلوب")]
        public string DepartmentId { get; set; } = string.Empty;

        public DateTime? DateFrom { get; set; }

        public DateTime? DateTo { get; set; }

        public bool ValidateDateRange()
        {
            if (DateFrom.HasValue && DateTo.HasValue)
            {
                return DateFrom.Value <= DateTo.Value;
            }
            return true;
        }
    }

    public class AppointmentFilterRequest : FilterRequest
    {
        public string? PatientId { get; set; }

        public string? PatientNationalId { get; set; }

        public string? AppointmentState { get; set; }

        public bool ValidatePatientInfo()
        {
            return !string.IsNullOrEmpty(PatientId) || !string.IsNullOrEmpty(PatientNationalId);
        }
    }

    public class QueueFilterRequest : FilterRequest
    {
        public string? QueueState { get; set; }

        public bool? IsActive { get; set; }
    }

    public class TicketFilterRequest : FilterRequest
    {
        public string? TicketState { get; set; }

        public bool? IsActive { get; set; }

        public string? PatientId { get; set; }

        public string? PatientNationalId { get; set; }

        public bool ValidatePatientInfo()
        {
            return !string.IsNullOrEmpty(PatientId) || !string.IsNullOrEmpty(PatientNationalId);
        }
    }
} 