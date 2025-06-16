using FluentResults;

using Hangfire;

using HSS.System.V2.DataAccess.Contracts;
using HSS.System.V2.Domain.Enums;
using HSS.System.V2.Domain.Helpers.Methods;
using HSS.System.V2.Domain.Helpers.Models;
using HSS.System.V2.Domain.Models.Appointments;
using HSS.System.V2.Domain.Models.Prescriptions;
using HSS.System.V2.Domain.Models.Queues;
using HSS.System.V2.Domain.ResultHelpers.Errors;
using HSS.System.V2.Services.Contracts;
using HSS.System.V2.Services.DTOs.ClinicDTOs;
using HSS.System.V2.Services.DTOs.ReceptionDTOs;
using HSS.System.V2.Services.Helpers;

using Microsoft.Extensions.Logging;

using System.Net.Sockets;

namespace HSS.System.V2.Services.Services;

public class ClinicServices : IClinicServices
{
    private readonly IAppointmentRepository _appointmentRepository;
    private readonly ITicketRepository _ticketRepository;
    private readonly IMedicalHistoryRepository _medicalHistoryRepository;
    private readonly IDiseaseRepository _diseaseRepository;
    private readonly ITestsRepository _testsRepository;
    private readonly IMedicineRepository _medicineRepository;
    private readonly IQueueRepository _queueRepository;
    private readonly ILogger<ClinicServices> _logger;
    private readonly IUnitOfWork _unitOfWork;

    public ClinicServices(IAppointmentRepository appointmentRepository, ITicketRepository ticketRepository,
        IMedicalHistoryRepository medicalHistoryRepository, IDiseaseRepository diseaseRepository,
        ITestsRepository testsRepository, IMedicineRepository medicineRepository, 
        IQueueRepository queueRepository, ILogger<ClinicServices> logger, IUnitOfWork unitOfWork)
    {
        _appointmentRepository = appointmentRepository;
        _ticketRepository = ticketRepository;
        _medicalHistoryRepository = medicalHistoryRepository;
        _diseaseRepository = diseaseRepository;
        _testsRepository = testsRepository;
        _medicineRepository = medicineRepository;
        _queueRepository = queueRepository;
        _logger = logger;
        _unitOfWork = unitOfWork;
    }
    public async Task<Result<ClinicAppointmentDto>> GetAppointmentDetailsById(string appointmentId)
    {
        var appointmentResult = await _appointmentRepository.GetAppointmentByIdAsync<ClinicAppointment>(appointmentId);
        if (appointmentResult.IsFailed)
            return Result.Fail(appointmentResult.Errors);
        var appointment = appointmentResult.Value;
        var result = new ClinicAppointmentDto()
        {
            AppointmentId = appointment.Id,
            ExpectedTimeToGetIn = appointment.SchaudleStartAt,
            PatientAge = appointment.Patient.GetAge(),
            PatientName = appointment.Patient.Name,
            PatientId = appointment.PatientId,
            PatientNationalId = appointment.PatientNationalId,
            TicketId = appointment.TicketId
        };
        return result;
    }

    public async Task<Result<AppointmentTicketDetailsDto>> GetCurrentTicketDetails(string ticketId)
    {
        var ticketresult = await _ticketRepository.GetTicketByIdInDetails(ticketId);
        if (ticketresult.IsFailed)
            return Result.Fail(ticketresult.Errors);
        var result = new AppointmentTicketDetailsDto()
        {
            Appointments = ticketresult.Value.Appointments.Select(a => new AppointmentInDetailsDto().MapFromModel(a)).ToList()
        };
        return result;
    }

    public async Task<Result<PagedResult<MinMedicalHistoryDto>>> GetMedicalHistoriesForPatient(string patientId, PaginationRequest pagination)
    {
        var result = await _medicalHistoryRepository.GetAllMedicalHistoryById(patientId);
        if (result.IsFailed)
            return Result.Fail(result.Errors);
        var medicalHistories = result.Value;
        var dtos = medicalHistories.Skip((pagination.Page - 1) * pagination.Size)
                .Take(pagination.Size)
                .Select(m => new MinMedicalHistoryDto().MapFromModel(m));
        var pagedResult = new PagedResult<MinMedicalHistoryDto>
                                    (dtos, medicalHistories.Count(), pagination.Page, pagination.Size);
        return pagedResult;
    }

    public async Task<Result<IEnumerable<MinMedicalHistoryDto>>> GetMedicalHistoriesForPatient(string patientId)
    {
        var result = await _medicalHistoryRepository.GetAllMedicalHistoryById(patientId);
        if (result.IsFailed)
            return Result.Fail(result.Errors);
        return result.Value.Select(m => new MinMedicalHistoryDto().MapFromModel(m)).ToList();
    }

    public async Task<Result<MidecalHistoryDto>> GetMedicalHistoryDetails(string medicalHistoryId)
    {
        var result = await _medicalHistoryRepository.GetMedicalHistoryByIdInDetails(medicalHistoryId);
        if (result.IsFailed)
            return Result.Fail(result.Errors);
        return new MidecalHistoryDto().MapFromModel(result.Value);
    }

    public async Task<Result> SubmitClinicResultAsync(ClinicResultRequestDto request)
    {
        try
        {
            ArgumentException.ThrowIfNullOrEmpty(request.AppointmentId);
            var appointmentResult = await _appointmentRepository.GetAppointmentByIdAsync<ClinicAppointment>(request.AppointmentId);
            if (appointmentResult.IsFailed)
                return Result.Fail(appointmentResult.Errors);
            var appointment = appointmentResult.Value;

            if (!string.IsNullOrEmpty(request.DiseaseId))
            {
                var disease = await _diseaseRepository.GetDiseaseById(request.DiseaseId);
                if (disease.IsFailed)
                    return Result.Fail(disease.Errors);
                appointment.DiseaseId = request.DiseaseId;
            }

            appointment.Diagnosis = request.Diagnosis;
            appointment.EmployeeName = appointment.Clinic.CurrentWorkingDoctor.Name ?? "غير معروف";
            appointment.ReExaminationNeeded = request.ReExaminationNeeded;

            if (request.TestsRequired is not null && request.TestsRequired.Count != 0)
            {
                foreach (var testRequired in request.TestsRequired)
                {
                    var testResult = await _testsRepository.GetTestByIdAsync(testRequired.TestId);
                    if (testResult.IsFailed)
                        return Result.Fail(new BadRequestError($"test with id '{testRequired.TestId}' not found"));

                    var newTestRequired = testRequired.ToModel();
                    newTestRequired.ClinicAppointmentId = appointment.Id;
                    newTestRequired.PatientNationalId = appointment.PatientNationalId;
                    newTestRequired.TestName = testResult.Value.Name;

                    appointment.TestsRequired.Add(newTestRequired);
                }
            }

            if (request.Prescription is not null)
            {
                var prescription = request.Prescription.ToModel();
                foreach (var item in prescription.Items)
                {
                    var mdeinice = await _medicineRepository.GetMedicineByIdAsync(item.MedicineId);
                    if (mdeinice.IsFailed)
                        return Result.Fail(new BadRequestError($"Medinice with id {item.MedicineId} not found"));
                    item.MedicineName = mdeinice.Value.Name;
                }
                prescription.ClinicAppointmentId = appointment.Id;
                appointment.Prescription = prescription;
                appointment.PrescriptionId = prescription.Id;
            }

            return await _appointmentRepository.UpdateAppointmentAsync(appointment);
        }
        catch (Exception ex)
        {
            _logger.LogDescriptiveError(ex, nameof(SubmitClinicResultAsync), request, nameof(request));
            
            return new ExceptionalError(ex);
        }
    }

    public async Task<Result> EndAppointmentAsync(string appointmentId)
    {
        var appointmentResult = await _appointmentRepository.GetAppointmentByIdAsync<ClinicAppointment>(appointmentId);
        if (appointmentResult.IsFailed)
            return Result.Fail(appointmentResult.Errors);
        var appointment = appointmentResult.Value;
        appointment.State = Domain.Enums.AppointmentState.Completed;
        return await _appointmentRepository.UpdateAppointmentAsync(appointment)
            .OnSuccessAsync(() => BackgroundJob.Enqueue<IClinicServices>(svc =>
                    svc.CreateMedicalHistoryIfPossible(appointment.TicketId)));
    }

    public async Task<Result<PagedResult<AppointmentDto>>> GetQueue(string departmentId, int page, int pageSize)
    {
        return await _queueRepository.GetQueueByDepartmentId<ClinicQueue>(departmentId)
            .MapAsync(q => q.Appointments
                                        .Where(a => a.State is AppointmentState.InQueue or AppointmentState.InProgress)
                                        .OrderByDescending(x => x.CreatedAt)
                                        .OrderBy(x => x.State)
                                        .Select(a => new AppointmentDto().MapFromModel(a))
                                        .GetPaged(page, pageSize));
    }

    public async Task<Result<IEnumerable<MedicineForClinicDto>>> GetMedinices(string? querySearch)
    {
        return await _medicineRepository.GetAllMedicinesAsync(querySearch)
            .MapAsync(m => m.Select(m1 => new MedicineForClinicDto().MapFromModel(m1)));
    }

    public async Task<Result<IEnumerable<DiseaseForClinicDto>>> GetDiseases(string? querySearch)
    {
        return await _diseaseRepository.GetAllDiseases(querySearch)
            .MapAsync(ds => ds.Select(d => new DiseaseForClinicDto().MapFromModel(d)));
    }

    public async Task CreateMedicalHistoryIfPossible(Ticket ticket)
    {
        try
        {
            var app = ticket.FirstClinicAppointment;
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
                var create = await _medicalHistoryRepository.CreateMedicalHistory(ticket);
                ticket.State = TicketState.InActive;
                var update = await _ticketRepository.UpdateTicket(ticket);
                if (create.IsSuccess && update.IsSuccess)
                    await _unitOfWork.SaveAllChanges();
            }
        }
        catch (Exception ex)
        {
            _logger.LogDescriptiveError(ex, nameof(CreateMedicalHistoryIfPossible));
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
