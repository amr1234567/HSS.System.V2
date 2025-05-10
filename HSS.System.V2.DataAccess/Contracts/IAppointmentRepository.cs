using FluentResults;

using HSS.System.V2.Domain.Appointments;
using HSS.System.V2.Domain.Common;
using HSS.System.V2.Domain.Helpers.Models;

using System.Linq.Expressions;

namespace HSS.System.V2.DataAccess.Contracts
{
    public interface IAppointmentRepository
    {
        Task<Result> CreateAppointmentAsync(Appointment model);
        Task<Result> UpdateAppointmentAsync(Appointment model);
        Task<Result> BulkUpdateAppointmentAsync<T>(Expression<Func<T, bool>> condition, Action<T> action) where T : Appointment;
        Task<Result> DeleteAppointmentAsync(Appointment model);
        Task<Result<T>> GetAppointmentByIdAsync<T>(string id) where T : Appointment;
        Task<Result<Appointment>> GetAppointmentByDateTimeInDepartmentAsync<TDept>(string departmentId, DateTime dateTime) where TDept : IHospitalDepartmentItem;
        Task<Result<PagedResult<ClinicAppointment>>> GetAllForClinicAsync(string clinicId, DateFilterationRequest dateFilters, PaginationRequest pagination);
        Task<Result<PagedResult<RadiologyCeneterAppointment>>> GetAllForRadiologyCenterAsync(string radiologyCenterId, DateFilterationRequest dateFilters, PaginationRequest pagination);
        Task<Result<PagedResult<MedicalLabAppointment>>> GetAllForMedicalLabAsync(string medicalLabId, DateFilterationRequest dateFilters, PaginationRequest pagination);
        Task<Result<PagedResult<Appointment>>> GetAllForHospitalAsync(string hospitalId, DateFilterationRequest dateFilters, PaginationRequest pagination);
        Task<Result<PagedResult<Appointment>>> GetAllForHospitalAsync(string hospitalId, string specializationId, DateFilterationRequest dateFilters, PaginationRequest pagination);
        Task<Result> DeleteAppointmentAsync(string appointmentId);
        Task<Result> SwapAppointmentsAsync<T>(string appointmentId1, string appointmentId2) where T : Appointment;
    }
}
