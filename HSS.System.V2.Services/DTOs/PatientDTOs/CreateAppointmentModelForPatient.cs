using System.ComponentModel.DataAnnotations;

using HSS.System.V2.Domain.Models.Appointments;
using HSS.System.V2.Domain.Models.Common;
using HSS.System.V2.Domain.Models.Facilities;

namespace HSS.System.V2.Services.DTOs.PatientDTOs
{
    public record CreateAppointmentModelForPatient
    {
        [Required]
        public DateTime ExpectedTimeForStart { get; set; }
    }

    public record CreateClinicAppointmentModelForPatient : CreateAppointmentModelForPatient, IInputModel<ClinicAppointment>
    {
        [Required]
        public string ClinicId { set; get; }
        [Required]
        public string TicketId { get; set; }

        public ClinicAppointment ToModel()
        {
            return new()
            {
                ClinicId = ClinicId,
                Id = Guid.NewGuid().ToString(),
                TicketId = TicketId,
                State = Domain.Enums.AppointmentState.NotStarted,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
                SchaudleStartAt = ExpectedTimeForStart
            };
        }
    }

    public record CreateRadiologyAppointmentModelForPatient : CreateAppointmentModelForPatient, IInputModel<RadiologyCeneterAppointment>
    {
        [Required]
        public string RadiologyCenterId { get; set; }
        public string? TicketId { get; set; }
        public string? TextRequiredId { get; set; }


        public RadiologyCeneterAppointment ToModel()
        {
            return new()
            {
                SchaudleStartAt = ExpectedTimeForStart,
                CreatedAt = DateTime.UtcNow,
                Id = Guid.NewGuid().ToString(),
                UpdatedAt = DateTime.UtcNow,
                State = Domain.Enums.AppointmentState.NotStarted,
                RadiologyCeneterId = RadiologyCenterId
            };
        }
    }

    public record CreateMedicalLabAppointmentModelForPatient : CreateAppointmentModelForPatient, IInputModel<MedicalLabAppointment>
    {
        [Required]
        public string MedicalLabId { get; set; }
        public string? TicketId { get; set; }
        public string? TextRequiredId { get; set; }


        public MedicalLabAppointment ToModel()
        {
            return new()
            {
                SchaudleStartAt = ExpectedTimeForStart,
                CreatedAt = DateTime.UtcNow,
                Id = Guid.NewGuid().ToString(),
                UpdatedAt = DateTime.UtcNow,
                State = Domain.Enums.AppointmentState.NotStarted,
                MedicalLabId = MedicalLabId
            };
        }
    }

}