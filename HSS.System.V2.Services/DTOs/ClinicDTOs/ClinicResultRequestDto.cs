namespace HSS.System.V2.Application.DTOs.Clinic
{
    public class ClinicResultRequestDto
    {
        public int AppointmentId { get; set; }
        public string Diagnosis { get; set; }
        public string TreatmentPlan { get; set; }
        public string Prescription { get; set; }
        public string Notes { get; set; }
        public object Treatment { get; internal set; }
    }
}