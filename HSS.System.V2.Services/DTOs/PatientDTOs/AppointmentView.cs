
namespace HSS.System.V2.Services.DTOs.PatientDTOs
{
    public class AppointmentView
    {
        public string Id { get; set; }
        public string HospitalName { get; set; }
        public string Name { get; set; }
        public string EmployeeName { get; set; }
        public DateTime? StartAt { get; set; }
    }
}
