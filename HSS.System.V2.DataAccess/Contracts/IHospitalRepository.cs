using FluentResults;

using HSS.System.V2.Domain.DTOs;
using HSS.System.V2.Domain.Helpers.Models;
using HSS.System.V2.Domain.Models.Common;
using HSS.System.V2.Domain.Models.Facilities;

namespace HSS.System.V2.DataAccess.Contracts
{
    public interface IHospitalRepository
    {
        Task<Result> CreateHospitalItem(Hospital hospital);
        Task<Result> DeleteHospitalItem(Hospital hospital);
        Task<Result> DeleteHospitalItem(string hospitalId);
        Task<Result> UpdateHospitalItem(Hospital hospital);
        Task<Result<PagedResult<Hospital>>> GetAllHospitalItems(int size = 10, int page = 1);
        Task<Result<PagedResult<Hospital>>> GetAllHospitalItemsInCity(int cityId, int size = 10, int page = 1);
        Task<Result<Hospital>> GetHospitalById(string hospitalId);
        Task<Result<Hospital>> GetHospitalByName(string hospitalName);
        Task<Result<HospitalDepartments>> GetAllHospitalDepartments(string hospitalId);
        Task<Result<PagedResult<TDept>>> GetHospitalDepartmentItems<TDept>(string hospitalId, PaginationRequest pagination) where TDept : class, IHospitalDepartmentItem;
        Task<Result<IEnumerable<TDept>>> GetHospitalDepartmentItems<TDept>(string hospitalId) where TDept : class, IHospitalDepartmentItem;
        Task<Result<PagedResult<Hospital>>> GetNearByHospitals(double lat, double lng, int size = 10, int page = 1);
        Task<Result<IEnumerable<Hospital>>> GetHospitalsBySpecificationId(string specializationId);
    }
}