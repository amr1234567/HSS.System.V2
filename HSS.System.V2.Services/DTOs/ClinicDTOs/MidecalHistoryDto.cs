using HSS.System.V2.Domain.Models.Common;
using HSS.System.V2.Domain.Models.Medical;

namespace HSS.System.V2.Services.DTOs.ClinicDTOs;

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