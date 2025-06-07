using FluentResults;
using HSS.System.V2.Domain.Helpers.Models;
using HSS.System.V2.Services.DTOs.MedicalLabDTOs;
using HSS.System.V2.Services.DTOs.PatientDTOs;
using HSS.System.V2.Services.DTOs.RadiologyCenterDTOs;
using HSS.System.V2.Services.DTOs.ReceptionDTOs;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HSS.System.V2.Services.Contracts
{
    public interface IMedicalLabServices
    {
        Task<Result> AddMedicalLabTestResult(ICollection<TestResultDto> result, string appointmentId);
        Task<Result> EndAppointment(string appointmentId);
        Task<Result<IEnumerable<TestResultField>>> GetTestResultField(string testId); 
        Task<Result<MedicaLabAppointmentModel>> GetCurrentMedicalLabAppointment(string appointmentId);
        Task<Result<PagedResult<AppointmentDto>>> GetQueueForMedicalLab(string medicalLabId, int page = 1, int pageSize = 10);
    }
}
