using FluentResults;
using HSS.System.V2.DataAccess.Contexts;
using HSS.System.V2.DataAccess.Contracts;
using HSS.System.V2.Domain.Helpers.Methods;
using HSS.System.V2.Domain.Helpers.Models;
using HSS.System.V2.Domain.Models.Appointments;
using HSS.System.V2.Domain.Models.Facilities;
using HSS.System.V2.Domain.Models.Medical;
using HSS.System.V2.Domain.Models.Queues;
using HSS.System.V2.Domain.ResultHelpers.Errors;
using HSS.System.V2.Services.Contracts;
using HSS.System.V2.Services.DTOs.MedicalLabDTOs;
using HSS.System.V2.Services.DTOs.RadiologyCenterDTOs;
using HSS.System.V2.Services.DTOs.ReceptionDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HSS.System.V2.Services.Services
{
    public class MedicalLabServices : IMedicalLabServices
    {
        private readonly IQueueRepository _queueRepository;
        private readonly IAppointmentRepository _appointmentRepository;
        private readonly ITestResultRepository _testResultRepository;
        public MedicalLabServices(IQueueRepository queueRepository, IAppointmentRepository appointmentRepository, ITestResultRepository testResultRepository)
        {
            _appointmentRepository = appointmentRepository;
            _queueRepository = queueRepository;
            _testResultRepository = testResultRepository;
        }


        public async Task<Result> EndAppointment(string appointmentId)
        {
            var appointmentResult = await _appointmentRepository.GetAppointmentByIdAsync<RadiologyCeneterAppointment>(appointmentId);
            if (appointmentResult.IsFailed)
                return Result.Fail(appointmentResult.Errors);
            var appointment = appointmentResult.Value;
            appointment.State = Domain.Enums.AppointmentState.Completed;
            return await _appointmentRepository.UpdateAppointmentAsync(appointment);
        }

        public async Task<Result<MedicaLabAppointmentModel>> GetCurrentMedicalLabAppointment(string appointmentId)
        {
            var appResult = await _appointmentRepository.GetAppointmentByIdAsync<MedicalLabAppointment>(appointmentId)
                .EnsureNoneAsync((r => r is null, new EntityNotExistsError("this appointment can't be found")));
            if (appResult.IsFailed)
                return Result.Fail(appResult.Errors);
            return new MedicaLabAppointmentModel()
            {
                AppointmentId = appointmentId,
                LastAppointmentDiagnosis = appResult.Value.ClinicAppointment!.Diagnosis ?? "لم يتم التحديد",
                PatientId = appResult.Value.PatientId,
                PatientName = appResult.Value.PatientName,
                PatientNationalId = appResult.Value.PatientNationalId,
                TestNeededId = appResult.Value.TestId,
                TestNeededName = appResult.Value.Test.Name,
            };
        }

        public async Task<Result<PagedResult<AppointmentDto>>> GetQueueForMedicalLab(string medicalLabId, int page = 1, int pageSize = 10)
        {
            var apps = await _queueRepository.GetQueueByDepartmentId<MedicalLabQueue>(medicalLabId);
            return apps.Value.MedicalLabAppointments
                .Select(a => new AppointmentDto().MapFromModel(a))
                .GetPaged(page, pageSize);
        }

        public async Task<Result<IEnumerable<TestResultField>>> GetTestResultField(string testId)
        {
            var fields = await _testResultRepository.GetMedicalLabTestResultFieldsAsync(testId);
            if (fields.IsFailed)
                return Result.Fail(fields.Errors);

            var allFields = fields.Value.Select(TestResultField.MapFromModel);
            return Result.Ok(allFields);
        }

        public async Task<Result> AddMedicalLabTestResult(ICollection<DTOs.MedicalLabDTOs.TestResultDto> result, string appointmentId)
        {
            if (result is null || string.IsNullOrEmpty(appointmentId))
                return Result.Fail("null input");

            var medicalLabTestResult = new MedicalLabTestResult();
            var allResult = new List<MedicalLabTestResult>();
            foreach(var dto in result)
            {
                medicalLabTestResult.Id = Guid.NewGuid().ToString();
                medicalLabTestResult.AppointmentId = appointmentId;
                medicalLabTestResult.FieldId = dto.FieldId;
                medicalLabTestResult.Value = dto.Value;
                allResult.Add(medicalLabTestResult);
            }

            var signResult = await _testResultRepository.AddTestResult(allResult);
            if (signResult.IsFailed)
                return Result.Fail("Failed Add Result");

            return Result.Ok();
        }
    }
}
