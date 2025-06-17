using HSS.System.V2.Domain.Models.Common;
using HSS.System.V2.Domain.Models.Medical;

namespace HSS.System.V2.Services.DTOs.ReceptionDTOs
{
    public class TestRequiredDto : IOutputDto<TestRequiredDto, TestRequired>
    {
        public string TestRequiredId { get; set; }
        public string TestId { get; set; }
        public string TestName { get; set; }
        public string ClinicAppointmentId { get; set; }
        public string PatientNationalId { get; set; }
        public bool Used { get; set; }
        public TestType TestType { get; set; }

        public TestRequiredDto MapFromModel(TestRequired model)
        {
            TestRequiredId = model.Id;
            TestId = model.TestId;
            TestName = model.TestName;
            ClinicAppointmentId = model.ClinicAppointmentId;
            PatientNationalId = model.PatientNationalId;
            Used = model.Used;
            TestType = model.Test is MedicalLabTest ? TestType.MedicalLabTest : TestType.RadiologyTest;
            return this;
        }
    }

    public enum TestType
    {
        RadiologyTest = 1,
        MedicalLabTest = 2
    }
}
