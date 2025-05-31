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

using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;

namespace HSS.System.V2.Services.Services
{
    public class RadiologyCenterServices : IRadiologyCenterServices
    {
        private readonly IQueueRepository _queueRepository;
        private readonly IAppointmentRepository _appointmentRepository;
        private readonly IWebHostEnvironment _env;

        public RadiologyCenterServices(IQueueRepository queueRepository, IAppointmentRepository appointmentRepository, IWebHostEnvironment env)
        {
            _queueRepository = queueRepository;
            _appointmentRepository = appointmentRepository;
            _env = env;
        }

        public async Task<Result<PagedResult<AppointmentDto>>> GetQueueForRadiologyCenter(string radiologyCenterId, int page = 1, int pageSize = 10)
        {
            var apps = await _queueRepository.GetQueueByDepartmentId<RadiologyCenterQueue>(radiologyCenterId);
            return apps.Value.RadiologyCeneterAppointments
                .Select(a => new AppointmentDto().MapFromModel(a))
                .GetPaged(page, pageSize);
        }

        public async Task<Result<RadiologyAppointmentModel>> GetCurrentRadiologyAppointment(string appointmentId)
        {
            var appResult = await _appointmentRepository.GetAppointmentByIdAsync<RadiologyCeneterAppointment>(appointmentId)
                .EnsureNoneAsync((r => r is null, new EntityNotExistsError("this appointment can't be found")));
            if (appResult.IsFailed)
                return Result.Fail(appResult.Errors);
            return new RadiologyAppointmentModel()
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

        public async Task<Result> AddRadiologyImagesToAppointment(string appointmentId, params IFormFile[] images)
        {
            try
            {
                var appResult = await _appointmentRepository.GetAppointmentByIdAsync<RadiologyCeneterAppointment>(appointmentId)
                                .EnsureNoneAsync((r => r is null, new EntityNotExistsError("this appointment can't be found")));
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
            appointment.State = Domain.Enums.AppointmentState.Completed;
            return await _appointmentRepository.UpdateAppointmentAsync(appointment);
        }
    }
}
