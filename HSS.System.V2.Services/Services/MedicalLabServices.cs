using FluentResults;

using Hangfire;

using HSS.System.V2.DataAccess.Contracts;
using HSS.System.V2.Domain.Constants;
using HSS.System.V2.Domain.Enums;
using HSS.System.V2.Domain.Helpers.Methods;
using HSS.System.V2.Domain.Helpers.Models;
using HSS.System.V2.Domain.Models.Appointments;
using HSS.System.V2.Domain.Models.Medical;
using HSS.System.V2.Domain.Models.Prescriptions;
using HSS.System.V2.Domain.Models.Queues;
using HSS.System.V2.Domain.ResultHelpers.Errors;
using HSS.System.V2.Services.Contracts;
using HSS.System.V2.Services.DTOs.MedicalLabDTOs;
using HSS.System.V2.Services.DTOs.RadiologyCenterDTOs;
using HSS.System.V2.Services.DTOs.ReceptionDTOs;
using HSS.System.V2.Services.Helpers;

using Microsoft.Extensions.Logging;

namespace HSS.System.V2.Services.Services
{
    public class MedicalLabServices : IMedicalLabServices
    {
        private readonly IQueueRepository _queueRepository;
        private readonly IAppointmentRepository _appointmentRepository;
        private readonly ITestResultRepository _testResultRepository;
        private readonly IMedicalHistoryRepository _medicalHistoryRepository;
        private readonly ITicketRepository _ticketRepository;
        private readonly ILogger<MedicalLabServices> _logger;

        public MedicalLabServices(IQueueRepository queueRepository, IAppointmentRepository appointmentRepository,
            ITestResultRepository testResultRepository, IMedicalHistoryRepository medicalHistoryRepository,
            ITicketRepository ticketRepository, ILogger<MedicalLabServices> logger)
        {
            _appointmentRepository = appointmentRepository;
            _queueRepository = queueRepository;
            _testResultRepository = testResultRepository;
            _medicalHistoryRepository = medicalHistoryRepository;
            _ticketRepository = ticketRepository;
            _logger = logger;
        }


        public async Task<Result> EndAppointment(string appointmentId)
        {
            var appointmentResult = await _appointmentRepository.GetAppointmentByIdAsync<RadiologyCeneterAppointment>(appointmentId);
            if (appointmentResult.IsFailed)
                return Result.Fail(appointmentResult.Errors);
            var appointment = appointmentResult.Value;
            appointment.State = Domain.Enums.AppointmentState.Completed;
            return await _appointmentRepository.UpdateAppointmentAsync(appointment)
                 .OnSuccessAsync(() => BackgroundJob.Enqueue<IMedicalLabServices>(
                     svg => svg.CreateMedicalHistoryIfPossible(appointment.TicketId)));
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
                .Where(a => a.State is AppointmentState.InQueue or AppointmentState.InProgress)
                .OrderByDescending(x => x.CreatedAt)
                .OrderBy(x => x.State)
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

        public async Task<Result> AddMedicalLabTestResult(IEnumerable<DTOs.MedicalLabDTOs.TestResultDto> result, string appointmentId)
        {
            if (result is null || string.IsNullOrEmpty(appointmentId))
                return Result.Fail("null input");

            var allResult = new List<MedicalLabTestResultFieldValue>();
            foreach(var dto in result)
            {
                var field = await _testResultRepository.GetTestFieldById(dto.FieldId);
                if (field.IsFailed || field.Value is null)
                    return new BadRequestError();
                var medicalLabTestResult = new MedicalLabTestResultFieldValue
                {
                    Id = Guid.NewGuid().ToString(),
                    AppointmentId = appointmentId,
                    FieldId = dto.FieldId,
                    Value = dto.Value,
                    CreatedAt = HelperDate.GetCurrentDate(),
                    ResultFieldType = field.Value.ResultFieldType,
                    UpdatedAt = HelperDate.GetCurrentDate(),
                };

                allResult.Add(medicalLabTestResult);
            }

            var signResult = await _testResultRepository.AddTestResult(allResult);
            if (signResult.IsFailed)
                return Result.Fail(signResult.Errors);

            return Result.Ok();
        }


        public async Task CreateMedicalHistoryIfPossible(Ticket ticket)
        {
            try
            {
                var app = ticket.FirstClinicAppointment;
                if (app is null)
                {
                    if (ticket.Appointments is not null && ticket.Appointments.Count > 0)
                    {
                        await _medicalHistoryRepository.CreateMedicalHistory(ticket);
                    }
                    return;
                }

                while (app.ReExamiationClinicAppointemnt != null)
                {
                    app = app.ReExamiationClinicAppointemnt;
                }

                if (!app.ReExaminationNeeded.HasValue)
                    return;

                var checkTestRequireds = app.TestsRequired is not null && app.TestsRequired.Count != 0;
                if (checkTestRequireds && !app.ReExaminationNeeded.Value)
                {
                    app.ReExaminationNeeded = true;
                    await _appointmentRepository.UpdateAppointmentAsync(app);
                }

                if (!app.ReExaminationNeeded.Value)
                {
                    await _medicalHistoryRepository.CreateMedicalHistory(ticket);
                }
            }
            catch (Exception ex)
            {
                _logger.LogDescriptiveError(ex, nameof(CreateMedicalHistoryIfPossible), ticket, nameof(ticket));
            }
        }

        public async Task CreateMedicalHistoryIfPossible(string ticketId)
        {
            try
            {
                var ticket = await _ticketRepository.GetTicketById(ticketId)
                    .EnsureNoneAsync((t => t is null, new BadRequestError()));
                if (ticket.IsSuccess)
                    await CreateMedicalHistoryIfPossible(ticket.Value);
            }
            catch (Exception ex)
            {
                _logger.LogDescriptiveError(ex, nameof(CreateMedicalHistoryIfPossible), ticketId);
            }
        }
    }
}
