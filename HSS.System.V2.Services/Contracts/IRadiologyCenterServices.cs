using FluentResults;

using HSS.System.V2.Domain.Helpers.Models;
using HSS.System.V2.Services.DTOs.RadiologyCenterDTOs;
using HSS.System.V2.Services.DTOs.ReceptionDTOs;

using Microsoft.AspNetCore.Http;

namespace HSS.System.V2.Services.Contracts
{
    public interface IRadiologyCenterServices
    {
        Task<Result> AddRadiologyImagesToAppointment(string appointmentId, params IFormFile[] images);
        Task CreateMedicalHistoryIfPossible(string ticketId);
        Task<Result> EndAppointmentAsync(string appointmentId);
        Task<Result<RadiologyAppointmentModel>> GetCurrentRadiologyAppointment(string appointmentId);
        Task<Result<PagedResult<AppointmentDto>>> GetQueueForRadiologyCenter(string radiologyCenterId, int page = 1, int pageSize = 10);
    }
}