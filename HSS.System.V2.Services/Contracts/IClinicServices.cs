using FluentResults;

using HSS.System.V2.Domain.Helpers.Models;
using HSS.System.V2.Services.DTOs.ClinicDTOs;

namespace HSS.System.V2.Services.Contracts;

public interface IClinicServices
{
    Task<Result<ClinicAppointmentDto>> GetAppointmentDetailsById(string appointmentId);
    Task<Result<AppointmentTicketDetailsDto>> GetCurrentTicketDetails(string appoitmentId);
    Task<Result<PagedResult<MinMedicalHistoryDto>>> GetMedicalHistoriesForPatient(string patientId, PaginationRequest pagination);
    Task<Result<IEnumerable<MinMedicalHistoryDto>>> GetMedicalHistoriesForPatient(string patientId);
    Task<Result<MidecalHistoryDto>> GetMedicalHistoryDetails(string medicalHistoryId);
    Task<Result> SubmitClinicResultAsync( ClinicResultRequestDto request);
    Task<Result> EndAppointmentAsync(string appointmentId);
}
