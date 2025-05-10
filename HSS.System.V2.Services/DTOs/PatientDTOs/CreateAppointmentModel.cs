using System.ComponentModel.DataAnnotations;

using HSS.System.V2.Domain.Models.Appointments;
using HSS.System.V2.Domain.Models.Common;

namespace HSS.System.V2.Services.DTOs.PatientDTOs
{
    public record CreateAppointmentModel
    {
        [Required]
        public DateTime ExpectedTimeForStart { get; set; }
        [Required]
        public TimeSpan ExpectedPeriodPerApp { get; set; }
        public string HospitalId { get; set; }
        public string? PatientId { get; set; }
        public string? PatientNationalId { get; set; }
        [Required]
        public string TicketId { get; set; }
    }

    public record CreateClinicAppointmentModel : CreateAppointmentModel, IInputModel<ClinicAppointment>
    {
        [Required]
        public string ClinicId { set; get; }
        public bool IsReExamination { set; get; } = false;

        public ClinicAppointment ToModel()
        {
            return new()
            {
                //ClinicId = ClinicId,
                //ExpectedTimeForStart = ExpectedTimeForStart,
                //ExpectedPeriodPerAppointment = ExpectedPeriodPerApp,
                //Id = Guid.NewGuid().ToString(),
                //TicketId = TicketId,
                //HospitalId = HospitalId,
                //StartAt = null,
                //ActualDuration = null,
                //State = Domain.Enums.AppointmentState.NotStarted,
                //CreatedAt = DateTime.UtcNow,
                //UpdatedAt = DateTime.UtcNow
            };
        }
    }

    public record CreateRadiologyAppointmentModel : CreateAppointmentModel, IInputModel<RadiologyCeneterAppointment>
    {
        [Required]
        public string RadiologyCenterId { get; set; }

        public RadiologyCeneterAppointment ToModel()
        {
            return new()
            {
                //RadiologyCenterId = RadiologyCenterId,
                //ExpectedTimeForStart = ExpectedTimeForStart,
                //CreatedAt = DateTime.UtcNow,
                //Id = Guid.NewGuid().ToString(),
                //UpdatedAt = DateTime.UtcNow,
                //HospitalId = HospitalId,
                //TicketId = TicketId,
                //State = Domain.Enums.AppointmentState.NotStarted,

            };
        }
    }

    public record CreateMedicalLabAppointmentModel : CreateAppointmentModel, IInputModel<MedicalLabAppointment>
    {
        [Required]
        public string MedicalLabId { get; set; }

        public MedicalLabAppointment ToModel()
        {
            return new()
            {
                //MedicalLabId = MedicalLabId,
                //ExpectedTimeForStart = ExpectedTimeForStart,
                //CreatedAt = DateTime.UtcNow,
                //Id = Guid.NewGuid().ToString(),
                //HospitalId = HospitalId,
                //UpdatedAt = DateTime.UtcNow,
                //TicketId = TicketId,
                //State = Domain.Enums.AppointmentState.NotStarted
            };
        }
    }

}