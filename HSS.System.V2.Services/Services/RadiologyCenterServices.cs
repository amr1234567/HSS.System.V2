using FluentResults;

using HSS.System.V2.DataAccess.Contracts;
using HSS.System.V2.Domain.Helpers.Methods;
using HSS.System.V2.Domain.Helpers.Models;
using HSS.System.V2.Domain.Models.Appointments;
using HSS.System.V2.Domain.Models.Queues;
using HSS.System.V2.Domain.ResultHelpers.Errors;
using HSS.System.V2.Services.Contracts;
using HSS.System.V2.Services.DTOs.RadiologyCenterDTOs;
using HSS.System.V2.Services.DTOs.ReceptionDTOs;
using HSS.System.V2.Domain.Enums;

using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using HSS.System.V2.Domain.Constants;
using Hangfire;
using HSS.System.V2.Domain.Models.Prescriptions;
using Microsoft.Extensions.Logging;
using HSS.System.V2.Services.Helpers;
using HSS.System.V2.DataAccess.Repositories;

namespace HSS.System.V2.Services.Services
{
    public class RadiologyCenterServices : IRadiologyCenterServices
    {
        private readonly IQueueRepository _queueRepository;
        private readonly IAppointmentRepository _appointmentRepository;
        private readonly IWebHostEnvironment _env;
        private readonly IMedicalHistoryRepository _medicalHistoryRepository;
        private readonly ILogger<RadiologyCenterServices> _logger;
        private readonly ITicketRepository _ticketRepository;

        public RadiologyCenterServices(IQueueRepository queueRepository, IAppointmentRepository appointmentRepository,
            IWebHostEnvironment env, IMedicalHistoryRepository medicalHistoryRepository, ILogger<RadiologyCenterServices> logger,
            ITicketRepository ticketRepository)
        {
            _queueRepository = queueRepository;
            _appointmentRepository = appointmentRepository;
            _env = env;
            _medicalHistoryRepository = medicalHistoryRepository;
            _logger = logger;
            _ticketRepository = ticketRepository;
        }

        public async Task<Result<PagedResult<AppointmentDto>>> GetQueueForRadiologyCenter(string radiologyCenterId, int page = 1, int pageSize = 10)
        {
            var apps = await _queueRepository.GetQueueByDepartmentId<RadiologyCenterQueue>(radiologyCenterId);
            return apps.Value.RadiologyCeneterAppointments
                //.Where(a => a.ActualStartAt!.Value.Date == HelperDate.GetCurrentDate().Date)
                .Where(a => a.State is AppointmentState.InQueue or AppointmentState.InProgress)
                .OrderByDescending(x => x.CreatedAt)
                .OrderBy(x => x.State)
                .Select(a => new AppointmentDto().MapFromModel(a))
                .GetPaged(page, pageSize);
        }

        public async Task<Result<RadiologyAppointmentModel>> GetCurrentRadiologyAppointment(string appointmentId)
        {
            var appResult = await _appointmentRepository.GetAppointmentByIdAsync<RadiologyCeneterAppointment>(appointmentId)
                .EnsureNoneAsync(
                (r => r is null, new EntityNotExistsError("this appointment can't be found")),
                (r => r.State is not AppointmentState.InProgress, new BadArgumentsError("هذا الحجز لم يبدأ بعد")));
            if (appResult.IsFailed)
                return Result.Fail(appResult.Errors);
            return new RadiologyAppointmentModel()
            {
                AppointmentId = appointmentId,
                LastAppointmentDiagnosis = appResult.Value.ClinicAppointment is null ? null : appResult.Value.ClinicAppointment.Diagnosis ?? "لم يتم التحديد",
                PatientId = appResult.Value.PatientId,
                PatientName = appResult.Value.PatientName,
                PatientNationalId = appResult.Value.PatientNationalId,
                TestNeededId = appResult.Value.TestId,
                TestNeededName = appResult.Value.Test.Name,
                Images = (await _appointmentRepository.GetRadiologyAppointmentResultImages(appointmentId)).Value
                            .Select(r => DomainPaths.Domain + r.ImagePath).ToList()
            };
        }

        public async Task<Result> AddRadiologyImagesToAppointment(string appointmentId, params IFormFile[] images)
        {
            try
            {
                var appResult = await _appointmentRepository.GetAppointmentByIdAsync<RadiologyCeneterAppointment>(appointmentId)
                                .EnsureNoneAsync(
                                    (r => r is null, new EntityNotExistsError("this appointment can't be found")));
                if (appResult.IsFailed)
                    return Result.Fail(appResult.Errors);

                foreach (var image in images)
                {
                    var imagePath = await UploadFiles.Upload(image, _env.WebRootPath, "RadiologyImages");
                    if (imagePath.IsFailed)
                        return Result.Fail(imagePath.Errors);
                    var newIamge = new RadiologyReseltImage
                    {
                        Id = Guid.NewGuid().ToString(),
                        AppointmentId = appointmentId,
                        CreatedAt = DateTime.UtcNow,
                        UpdatedAt = DateTime.UtcNow,
                        ImagePath = imagePath.Value,
                    };
                    var result = await _appointmentRepository.AddImageToRadiologyAppointmentResult(newIamge);
                    if (result.IsFailed)
                        return Result.Fail(result.Errors);
                }
                return Result.Ok();
            }
            catch (Exception e)
            {
                return new ExceptionalError(e);
            }
        }

        public async Task<Result> EndAppointmentAsync(string appointmentId)
        {
            var appointmentResult = await _appointmentRepository.GetAppointmentByIdAsync<RadiologyCeneterAppointment>(appointmentId);
            if (appointmentResult.IsFailed)
                return Result.Fail(appointmentResult.Errors);
            var appointment = appointmentResult.Value;
            appointment.State = AppointmentState.Completed;
            return await _appointmentRepository.UpdateAppointmentAsync(appointment)
                .OnSuccessAsync(() => BackgroundJob.Enqueue<IRadiologyCenterServices>(
                    svg => svg.CreateMedicalHistoryIfPossible(appointment.TicketId)));
        }

        public async Task CreateMedicalHistoryIfPossible(Ticket ticket)
        {
            try
            {
                var app = ticket.FirstClinicAppointment;
                if(app is null)
                {
                    if(ticket.Appointments is not null && ticket.Appointments.Count > 0)
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
