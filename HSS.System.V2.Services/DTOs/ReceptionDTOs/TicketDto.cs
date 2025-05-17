using HSS.System.V2.Domain.Models.Common;
using HSS.System.V2.Domain.Models.Prescriptions;

namespace HSS.System.V2.Services.DTOs.ReceptionDTOs
{
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
        public List<PrescriptionDto> Prescriptions { get; set; } = [];

        public List<TestRequiredDto> MedicalLabTestsRequired { get; set; } = [];
        public List<TestRequiredDto> RadiologyTestsRequired { get; set; } = [];
        public bool ReexaminationRequired { get; set; }
        public SpecializationDto? SpecializationForReexamination { set; get; }
        public string CurrentState { get; set; }
        public string HospitalName { get; set; }
        public string HospitalId { get; set; }

        public TicketDto MapFromModel(Ticket model)
        {
            TicketId = model.Id;
            CreatedAt = model.CreatedAt;
            PatientName = model.PatientName;
            PatientNationalId = model.PatientNationalId;
            PatientId = model.PatientId;
            CurrentState = model.State.ToString();
            HospitalName = model.HospitalCreatedIn.Name;
            HospitalId = model.HospitalCreatedInId;

            // تحويل المواعيد
            if (model.Appointments != null)
                Appointments = model.Appointments.Select(x => new AppointmentDto().MapFromModel(x)).ToList();
            var clinicAppointment = model.FirstClinicAppointment;
            if (!string.IsNullOrEmpty(model.FirstClinicAppointmentId))
            {
                while (clinicAppointment is not null)
                {
                    Prescriptions.Add(new PrescriptionDto().MapFromModel(clinicAppointment.Prescription));
                    var tests = clinicAppointment.TestsRequired.Select(t => new TestRequiredDto().MapFromModel(t));
                    MedicalLabTestsRequired.AddRange(tests.Where(t => t.TestType == TestType.MedicalLabTest));
                    RadiologyTestsRequired.AddRange(tests.Where(t => t.TestType == TestType.RadiologyTest));
                    clinicAppointment = clinicAppointment.ReExamiationClinicAppointemnt;
                    
                    if(clinicAppointment.ReExamiationClinicAppointemnt is null && clinicAppointment.ReExaminationNeeded.HasValue)
                    {
                        ReexaminationRequired = clinicAppointment.ReExaminationNeeded.Value;

                        if (clinicAppointment.ReExaminationNeeded.Value)
                        {
                            SpecializationForReexamination = new()
                            {
                                Id = clinicAppointment.Clinic.Specialization.Id,
                                Name = clinicAppointment.Clinic.Specialization.Name,
                                Icon = clinicAppointment.Clinic.Specialization.Icon,
                                Description = clinicAppointment.Clinic.Specialization.Description
                            };
                        }
                    }
                }
            }
            return this;
        }
    }
}
