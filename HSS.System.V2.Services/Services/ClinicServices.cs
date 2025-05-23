using FluentResults;

using HSS.System.V2.Application.DTOs.Clinic;
using HSS.System.V2.DataAccess.Contracts;
using HSS.System.V2.Domain.Helpers.Models;
using HSS.System.V2.Domain.Models.Appointments;
using HSS.System.V2.Services.Contracts;
using HSS.System.V2.Services.DTOs.ClinicDTOs;

namespace HSS.System.V2.Services.Services;

public class ClinicServices : IClinicServices
{
    private readonly IAppointmentRepository _appointmentRepository;
    private readonly ITicketRepository _ticketRepository;
    private readonly IMedicalHistoryRepository _medicalHistoryRepository;

    public ClinicServices(IAppointmentRepository appointmentRepository, ITicketRepository ticketRepository,
        IMedicalHistoryRepository medicalHistoryRepository)
    {
        _appointmentRepository = appointmentRepository;
        _ticketRepository = ticketRepository;
        _medicalHistoryRepository = medicalHistoryRepository;
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
        var result = await _medicalHistoryRepository.GetAllMedicalHistoryByIdInDetails(patientId);
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

    public Task<Result<IEnumerable<MinMedicalHistoryDto>>> GetMedicalHistoriesForPatient(string patientId)
    {
        throw new NotImplementedException();
    }

    public Task<Result<MinMedicalHistoryDto>> GetMedicalHistoryDetails(string medicalHistoryId)
    {
        throw new NotImplementedException();
    }

    public Task<Result> SubmitClinicResultAsync(ClinicResultRequestDto request)
    {
        throw new NotImplementedException();
    }

    public Task<Result> EndAppointmentAsync(int appointmentId)
    {
        throw new NotImplementedException();
    }
}
