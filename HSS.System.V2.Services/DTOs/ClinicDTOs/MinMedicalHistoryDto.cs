using HSS.System.V2.Domain.Models.Common;
using HSS.System.V2.Domain.Models.Medical;

namespace HSS.System.V2.Services.DTOs.ClinicDTOs;

public class MinMedicalHistoryDto : IOutputDto<MinMedicalHistoryDto, MedicalHistory>
{
    public string Id { get; set; }
    public string PatientName { get; set; }
    public DateTime RecordDate { get; set; }
    public string Diagnosis { get; set; }

    public MinMedicalHistoryDto MapFromModel(MedicalHistory model)
    {
        Id = model.Id;
        PatientName = model.Patient.Name;
        RecordDate = model.CreatedAt;
        Diagnosis = model.FinalDiagnosis;

        return this;
    }
}
