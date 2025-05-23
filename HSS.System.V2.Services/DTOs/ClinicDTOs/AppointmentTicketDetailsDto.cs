using HSS.System.V2.Domain.Models.Appointments;
using HSS.System.V2.Domain.Models.Common;
using HSS.System.V2.Services.DTOs.ReceptionDTOs;

using System.Text.Json.Serialization;

namespace HSS.System.V2.Application.DTOs.Clinic
{
    public record AppointmentTicketDetailsDto
    {
        public List<AppointmentInDetailsDto> Appointments { get; set; }
    }

    public record AppointmentInDetailsDto : IOutputDto<AppointmentInDetailsDto, Appointment>
    {
        public string AppointmentId { set; get; }
        public TimeSpan PeriodPerAppointment { get; set; }
        public DateTime? StartAt { get; set; }
        public string PatientName { get; set; } = "غير معروف";
        public string? HospitalName { get; set; } = "غير معروف";
        public string DepartmentName { get; set; }
        public string? ClinicAppointmentRelatedTo { get; set; }

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public List<AppointmentInDetailsDto>? RelatedAppointments { get; set; }

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public PrescriptionDto? PrescriptionDto { get; set; }

        public AppointmentInDetailsDto MapFromModel(Appointment model)
        {
            AppointmentId = model.Id;
            PeriodPerAppointment = model.ExpectedDuration;
            StartAt = model.ActualStartAt ?? model.SchaudleStartAt;
            PatientName = model.PatientName;
            HospitalName = model.HospitalName;
            DepartmentName = model.DepartmentName;
            if (model is ClinicAppointment c)
            {
                RelatedAppointments = c.RadiologyCeneterAppointments
                    .Select(x => new AppointmentInDetailsDto().MapFromModel(x))
                    .Concat(c.MedicalLabAppointments
                                    .Select(x => new AppointmentInDetailsDto().MapFromModel(x))).ToList();
                PrescriptionDto = new PrescriptionDto().MapFromModel(c.Prescription);
                ClinicAppointmentRelatedTo = c.PreExamiationClinicAppointemntId;
            }
            if (model is MedicalLabAppointment m)
                ClinicAppointmentRelatedTo = m.ClinicAppointmentId;
            if (model is RadiologyCeneterAppointment r)
                ClinicAppointmentRelatedTo = r.ClinicAppointmentId;


            return this;
        }
    }
}

