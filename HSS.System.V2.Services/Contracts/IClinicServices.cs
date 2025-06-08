using FluentResults;

using HSS.System.V2.Domain.Helpers.Models;
using HSS.System.V2.Domain.Models.Prescriptions;
using HSS.System.V2.Services.DTOs.ClinicDTOs;
using HSS.System.V2.Services.DTOs.ReceptionDTOs;

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
    Task<Result<PagedResult<AppointmentDto>>> GetQueue(string departmentId, int page, int pageSize);
    Task<Result<IEnumerable<MedicineForClinicDto>>> GetMedinices(string? querySearch);
    Task<Result<IEnumerable<DiseaseForClinicDto>>> GetDiseases(string? querySearch);
    Task CreateMedicalHistoryIfPossible(string ticketId);
}
