using FluentResults;

using HSS.System.V2.DataAccess.Contracts;
using HSS.System.V2.Domain.Helpers.Models;
using HSS.System.V2.Domain.Models.Appointments;
using HSS.System.V2.Domain.ResultHelpers.Errors;
using HSS.System.V2.Services.Contracts;
using HSS.System.V2.Services.DTOs.ClinicDTOs;

namespace HSS.System.V2.Services.Services;

public class ClinicServices : IClinicServices
{
    private readonly IAppointmentRepository _appointmentRepository;
    private readonly ITicketRepository _ticketRepository;
    private readonly IMedicalHistoryRepository _medicalHistoryRepository;
    private readonly IDiseaseRepository _diseaseRepository;
    private readonly ITestsRepository _testsRepository;
    private readonly IMedicineRepository _medicineRepository;

    public ClinicServices(IAppointmentRepository appointmentRepository, ITicketRepository ticketRepository,
        IMedicalHistoryRepository medicalHistoryRepository, IDiseaseRepository diseaseRepository,
        ITestsRepository testsRepository, IMedicineRepository medicineRepository)
    {
        _appointmentRepository = appointmentRepository;
        _ticketRepository = ticketRepository;
        _medicalHistoryRepository = medicalHistoryRepository;
        _diseaseRepository = diseaseRepository;
        _testsRepository = testsRepository;
        _medicineRepository = medicineRepository;
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

        if(request.TestsRequired is not null && request.TestsRequired.Any())
        {
            foreach(var testRequired in request.TestsRequired)
            {
                var testResult = await _testsRepository.GetTestByIdAsync(testRequired.TestId);
                if (testResult.IsFailed)
                    return Result.Fail(new BadRequestError($"test with id '{testRequired.TestId}' not found"));
                appointment.TestsRequired.Add(testRequired.ToModel());
            }
        }

        if(request.Prescription is not null)
        {
            var prescription = request.Prescription.ToModel();
            foreach(var item in prescription.Items)
            {
                var mdeinice = await _medicineRepository.GetMedicineByIdAsync(item.MedicineId);
                if (mdeinice.IsFailed)
                    return Result.Fail(new BadRequestError($"Medinice with id {item.MedicineId} not found"));
                item.MedicineName = mdeinice.Value.Name;
            }
            appointment.Prescription = prescription;
        }

        return await _appointmentRepository.UpdateAppointmentAsync(appointment);
    }

    public async Task<Result> EndAppointmentAsync(string appointmentId)
    {
        var appointmentResult = await _appointmentRepository.GetAppointmentByIdAsync<ClinicAppointment>(appointmentId);
        if (appointmentResult.IsFailed)
            return Result.Fail(appointmentResult.Errors);
        var appointment = appointmentResult.Value;
        appointment.State = Domain.Enums.AppointmentState.Completed;
        return await _appointmentRepository.UpdateAppointmentAsync(appointment);
    }
}
