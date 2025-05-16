namespace HSS.System.V2.Application.DTOs.Clinic
{
    public class MedicalHistoryDto
    {
        public int Id { get; set; }
        public int PatientId { get; set; }
        public string PatientName { get; set; }
        public DateTime RecordDate { get; set; }
        public string Diagnosis { get; set; }
        public string Treatment { get; set; }
        public string Notes { get; set; }
        public List<string> Attachments { get; set; }
        public object CreatedAt { get; internal set; }
    }
}
