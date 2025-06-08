
namespace HSS.System.V2.Services.DTOs.RadiologyCenterDTOs
{
    public record RadiologyAppointmentModel
    {
        public string AppointmentId { get; set; }
        public string PatientNationalId { get; set; }
        public string PatientId { get; set; }
        public string PatientName { get; set; }
        public string? LastAppointmentDiagnosis { get; set; }
        public string TestNeededId { get; set; }
        public string TestNeededName { get; set; }
        public List<string> Images { get; internal set; }
    }
}
