using HSS.System.V2.Domain.Models.Common;
using HSS.System.V2.Domain.Models.Prescriptions;

namespace HSS.System.V2.Services.DTOs.ReceptionDTOs
{
    #region DTO and Input Models

    /// <summary>
    /// Data transfer object representing a ticket.
    /// </summary>
    public class TicketDto : IOutputDto<TicketDto, Ticket>
    {
        public string TicketId { get; set; }
        public DateTime CreatedAt { set; get; }
        public string PatientName { get; set; }
        public string PatientNationalId { get; set; }
        public string PatientId { get; set; }
        public List<AppointmentDto> Appointments { get; set; } = [];
        //public List<MedicalPrescriptionDto> Prescriptions { get; set; } = [];

        //public List<TestRequiredDto> MedicalLabTestsRequired { get; set; } = [];
        //public List<TestRequiredDto> RadiologyTestsRequired { get; set; } = [];
        public bool ReexaminationRequired { get; set; }
        public SpecializationDto? SpecializationForReexamination { set; get; }
        public string CurrentState { get; set; }
        public string HospitalName { get; set; }
        public string HospitalId { get; set; }

        //public TicketDto MapFromModel(Ticket model, List<TestRequired<MedicalLabTest>>? medicalLabTestsRequireds, List<TestRequired<RadiologyTest>>? radiologyTestsRequireds)
        //{
        //    TicketId = model.Id;
        //    CreatedAt = model.CreatedAt;
        //    PatientName = model.PatientName;
        //    PatientNationalId = model.PatientNationalId;
        //    PatientId = model.PatientId;
        //    HospitalName = model.Hospital.Name;
        //    HospitalId = model.Hospital.Id;
        //    CurrentState = model.CurrentState.ToString();
        //    MedicalLabTestsRequired = medicalLabTestsRequireds?.Select(r => new TestRequiredDto().MapFromModel(r)).ToList() ?? [];
        //    RadiologyTestsRequired = radiologyTestsRequireds?.Select(r => new TestRequiredDto().MapFromModel(r)).ToList() ?? [];

        //    var appointmentNeedExamination = model.Appointments.OfType<ClinicAppointment>().FirstOrDefault(t => t.ReExaminationNeeded && model.CurrentState == Domain.Enums.TicketState.Active);
        //    ReexaminationRequired = appointmentNeedExamination != null && appointmentNeedExamination.ReExaminationNeeded;
        //    SpecializationForReexamination = appointmentNeedExamination == null ? null : new()
        //    {
        //        Id = appointmentNeedExamination.Clinic.Specialization.Id,
        //        Name = appointmentNeedExamination.Clinic.Specialization.Name,
        //        Icon = appointmentNeedExamination.Clinic.Specialization.Icon,
        //        Description = appointmentNeedExamination.Clinic.Specialization.Description,
        //    };

        //    // تحويل المواعيد
        //    if (model.Appointments != null)
        //    {
        //        foreach (var appointment in model.Appointments)
        //        {
        //            var appointmentDto = new AppointmentDto().MapFromModel(appointment);
        //            Appointments.Add(appointmentDto);
        //        }
        //    }

        //    // تحويل الوصفات الطبية
        //    if (model.Prescriptions != null)
        //    {
        //        foreach (var prescription in model.Prescriptions)
        //        {
        //            var prescriptionDto = new MedicalPrescriptionDto().MapFromModel(prescription);
        //            Prescriptions.Add(prescriptionDto);
        //        }
        //    }

        //    return this;
        //}
        public TicketDto MapFromModel(Ticket model)
        {
            TicketId = model.Id;
            CreatedAt = model.CreatedAt;
            PatientName = model.PatientName;
            PatientNationalId = model.PatientNationalId;
            PatientId = model.PatientId;
            //HospitalName = model.Hospital?.Name;
            //HospitalId = model.HospitalId;
            //CurrentState = model.CurrentState.ToString();

            // تحويل المواعيد
            if (model.Appointments != null)
            {
                foreach (var appointment in model.Appointments)
                {
                    var appointmentDto = new AppointmentDto().MapFromModel(appointment);
                    Appointments.Add(appointmentDto);
                }
            }

            // تحويل الوصفات الطبية
            //if (model.Prescriptions != null)
            //{
            //    foreach (var prescription in model.Prescriptions)
            //    {
            //        var prescriptionDto = new MedicalPrescriptionDto().MapFromModel(prescription);
            //        Prescriptions.Add(prescriptionDto);
            //    }
            //}

            return this;
        }

        //public TicketDto SetAppointments(List<TestRequired<MedicalLabTest>>? medicalLabTestsRequireds, List<TestRequired<RadiologyTest>>? radiologyTestsRequireds)
        //{
        //    MedicalLabTestsRequired = medicalLabTestsRequireds?.Select(r => new TestRequiredDto().MapFromModel(r)).ToList() ?? [];
        //    RadiologyTestsRequired = radiologyTestsRequireds?.Select(r => new TestRequiredDto().MapFromModel(r)).ToList() ?? [];
        //    return this;
        //}
    }

    #endregion
}
