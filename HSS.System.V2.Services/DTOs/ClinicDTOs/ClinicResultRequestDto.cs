namespace HSS.System.V2.Services.DTOs.ClinicDTOs;

public class ClinicResultRequestDto
{
    public string AppointmentId { get; set; }
    public string? Diagnosis { get; set; }
    public string? DiseaseId { get; set; }
    public PrescriptionToCreateModel? Prescription { get; set; }
    public bool ReExaminationNeeded { get; set; }
    public List<TestRequiredNeeded>? TestsRequired { get; set; }
}