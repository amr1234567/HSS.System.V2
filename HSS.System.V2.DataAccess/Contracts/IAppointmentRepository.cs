using FluentResults;

using HSS.System.V2.Domain.Enums;
using HSS.System.V2.Domain.Helpers.Models;
using HSS.System.V2.Domain.Models.Appointments;
using HSS.System.V2.Domain.Models.Common;

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
        Task<Result<Appointment?>> GetAppointmentByIdAsync(string id);
        Task<Result<Appointment?>> GetAppointmentByIdBlukIncludeAsync(string appointmentId);
        Task<Result<Appointment>> GetAppointmentByDateTimeInDepartmentAsync<TDept>(string departmentId, DateTime dateTime) where TDept : IHospitalDepartmentItem;
        Task<Result<List<ClinicAppointment>>> GetAllForClinicAsync(string clinicId, DateFilterationRequest dateFilters);
        Task<Result<List<RadiologyCeneterAppointment>>> GetAllForRadiologyCenterAsync(string radiologyCenterId, DateFilterationRequest dateFilters);
        Task<Result<List<MedicalLabAppointment>>> GetAllForMedicalLabAsync(string medicalLabId, DateFilterationRequest dateFilters);
        Task<Result<List<Appointment>>> GetAllForHospitalAsync(string hospitalId, DateFilterationRequest dateFilters);
        Task<Result<List<Appointment>>> GetAllForHospitalAsync(string hospitalId, string specializationId, DateFilterationRequest dateFilters);
        Task<Result> DeleteAppointmentAsync(string appointmentId);
        Task<Result> SwapAppointmentsAsync<T>(string appointmentId1, string appointmentId2) where T : Appointment;
        Task<Result<IEnumerable<Appointment>>> GetAllAppointmentsForUser(string apiUserId, AppointmentState state);
        Task<Result<IEnumerable<Appointment>>> GetAllAppointmentsForUser(string apiUserId);
        Task<Result<PagedResult<Appointment>>> GetAllAppointmentsForUser(string apiUserId, PaginationRequest pagination);
        Task<Result<PagedResult<Appointment>>> GetAllAppointmentsForUser(string apiUserId, PaginationRequest pagination, AppointmentState state);

        Task<Result> AddImageToRadiologyAppointmentResult(RadiologyReseltImage result);
        Task<Result<IEnumerable<RadiologyReseltImage>>> GetRadiologyAppointmentResultImages(string appId);
    }
}
