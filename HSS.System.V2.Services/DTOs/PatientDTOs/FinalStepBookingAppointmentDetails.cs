namespace HSS.System.V2.Services.DTOs.PatientDTOs
{
    public record FinalStepBookingAppointmentDetails
    {
        public string HospitalName { get; set; }
        public string EmployeeName { get; set; }
        public string DepartmentName { get; set; }

        public int AppointmentIndex { get; set; }
        public DateTime StartAt { get; set; }

        public TimeSpan PeriodPerAppointment { get; set; }
        public string HospitalAddress { get; set; }
    }
}
