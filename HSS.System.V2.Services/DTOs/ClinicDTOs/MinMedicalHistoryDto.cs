using HSS.System.V2.Application.DTOs.Clinic;
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


public class MidecalHistoryDto : IOutputDto<MidecalHistoryDto, MedicalHistory>
{
    public string PatientId { get; set; }
    public string PatientNationalId { get; set; }
    public string PatientName { get; set; }
    public DateTime CreatedAt { get; set; }
    public string TicketId { get; set; }
    public virtual List<AppointmentInDetailsDto> Appointments { get; set; }
    public string FinalDiagnosis { get; set; }

    public MidecalHistoryDto MapFromModel(MedicalHistory model)
    {
        PatientId = model.PatientId;
        PatientNationalId = model.PatientNationalId;
        PatientName = model.Patient.Name;
        CreatedAt = model.CreatedAt;
        TicketId = model.TicketId;
        Appointments = model.Appointments
            .Select(x => new AppointmentInDetailsDto().MapFromModel(x)).ToList();
        FinalDiagnosis = model.FinalDiagnosis;
        return this;
    }
}