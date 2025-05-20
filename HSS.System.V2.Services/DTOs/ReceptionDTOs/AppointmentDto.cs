using HSS.System.V2.Domain.Models.Appointments;
using HSS.System.V2.Domain.Models.Common;

namespace HSS.System.V2.Services.DTOs.ReceptionDTOs
{
    #region DTO and Input Models

    /// <summary>
    /// Data transfer object representing an appointment.
    /// </summary>
    public class AppointmentDto : IOutputDto<AppointmentDto, Appointment>
    {
        public string AppointmentId { set; get; }
        public TimeSpan PeriodPerAppointment { get; set; }
        public DateTime? StartAt { get; set; }
        public string PatientName { get; set; } = "غير معروف";
        public string PatientNationalId { get; set; }
        public decimal PatientAge { get; set; }
        public string? HospitalName { get; set; } = "غير معروف";
        public string TicketId { get; set; }
        public string QueueId { get; set; }
        public string State { get; set; }
        public string DepartmentName { get; set; }
        public string? ClinicAppointmentRelatedTo { get; set; }

        public AppointmentDto MapFromModel(Appointment model)
        {
            AppointmentId = model.Id;
            PeriodPerAppointment = model.ExpectedDuration;
            StartAt = model.ActualStartAt ?? model.SchaudleStartAt;
            PatientName = model.PatientName;
            HospitalName = model.HospitalName;
            TicketId = model.TicketId;
            QueueId = model.QueueId;
            State = model.State.ToString();
            DepartmentName = model.DepartmentName;
            PatientNationalId = model.PatientNationalId;
            PatientAge = model.Ticket.Patient.GetAge();
            if(model is ClinicAppointment c)
                ClinicAppointmentRelatedTo = c.PreExamiationClinicAppointemntId;
            if (model is MedicalLabAppointment m)
                ClinicAppointmentRelatedTo = m.ClinicAppointmentId;
            if (model is RadiologyCeneterAppointment r)
                ClinicAppointmentRelatedTo = r.ClinicAppointmentId;

            return this;
        }
    }

    #endregion
}
