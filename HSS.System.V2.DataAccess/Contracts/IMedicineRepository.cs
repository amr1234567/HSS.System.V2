using FluentResults;

using HSS.System.V2.Domain.Helpers.Models;
using HSS.System.V2.Domain.Models.Medical;

using System.Linq.Expressions;

namespace HSS.System.V2.DataAccess.Contracts
{
    public interface IMedicineRepository
    {
        Task<Result<Medicine>> GetMedicineByIdAsync(string id, params Expression<Func<Medicine, object>>[]? includes);
        Task<Result<PagedResult<Medicine>>> GetAllMedicinesAsync(int page = 1, int size = 10, params Expression<Func<Medicine, object>>[]? includes);
        Task<Result<PagedResult<Medicine>>> GetAllMedicinesAsync(string query, int page = 1, int size = 10, params Expression<Func<Medicine, object>>[]? includes);
        Task<Result<PagedResult<MedicinePharmacy>>> GetAllMedicinesInPharmacyAsync(string pharmacyId, int page = 1, int size = 10);
        Task<Result<MedicinePharmacy?>> GetMedicineInPharmacyAsync(string pharmacyId, string medicineId);
    }
}
