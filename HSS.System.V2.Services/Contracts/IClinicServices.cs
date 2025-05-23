
using HSS.System.V2.Application.DTOs;
using HSS.System.V2.Application.DTOs.Clinic;
using HSS.System.V2.Services.DTOs.ReceptionDTOs;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HSS.System.V2.Application.Interfaces
{
    public interface IClinicServices
    {
        Task<ClinicDto> GetClinicDetailsAsync(int clinicId);
        Task<CurrentClinicDetailsDto> GetCurrentClinicDetailsAsync(int clinicId);
        Task<List<MedicalHistoryDto>> GetMedicalHistoriesAsync(int clinicId);
        Task<MedicalHistoryDto> GetMedicalHistoryByIdAsync(int clinicId, int medicalHistoryId);
        Task SubmitClinicResultAsync(int clinicId, ClinicResultRequestDto request);
        Task EndAppointmentAsync(int clinicId, int appointmentId);
        Task GetByIdAsync(int clinicId);
    }
}

