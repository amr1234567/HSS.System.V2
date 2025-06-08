using FluentResults;

using HSS.System.V2.DataAccess.Contexts;
using HSS.System.V2.DataAccess.Contracts;
using HSS.System.V2.Domain.Helpers.Methods;
using HSS.System.V2.Domain.Helpers.Models;
using HSS.System.V2.Domain.Models.Medical;
using HSS.System.V2.Domain.ResultHelpers.Errors;

using Microsoft.EntityFrameworkCore;

using System.Linq.Expressions;

namespace HSS.System.V2.DataAccess.Repositories
{
    public class MedicineRepository : IMedicineRepository
    {
        private readonly AppDbContext _context;

        public MedicineRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Result<PagedResult<Medicine>>> GetAllMedicinesAsync(int page = 1, int size = 10, params Expression<Func<Medicine, object>>[]? includes)
        {
            try
            {
                IQueryable<Medicine> medicinesQuery = _context.Medicines;
                if (includes is not null)
                {
                    foreach (var item in includes)
                    {
                        medicinesQuery = medicinesQuery.Include(item);
                    }
                }
                return await medicinesQuery.GetPagedAsync(page, size);
            }
            catch (Exception ex)
            {
                return new RetrievingDataFromDbContextError(ex);
            }
        }

        public async Task<Result<PagedResult<Medicine>>> GetAllMedicinesAsync(string query, int page = 1, int size = 10, params Expression<Func<Medicine, object>>[]? includes)
        {
            try
            {
                IQueryable<Medicine> medicinesQuery = _context.Medicines.Where(item => item.Name.Contains(query));
                if (includes is not null)
                {
                    foreach (var item in includes)
                    {
                        medicinesQuery = medicinesQuery.Include(item);
                    }
                }
                return await medicinesQuery.GetPagedAsync(page, size);
            }
            catch (Exception ex)
            {
                return new RetrievingDataFromDbContextError(ex);
            }
        }

        public async Task<Result<PagedResult<MedicinePharmacy>>> GetAllMedicinesInPharmacyAsync(string pharmacyId, int page = 1, int size = 10)
        {
            try
            {
                return await _context.MedicinePharmacies
                    .Where(item => item.PharmacyId == pharmacyId)
                    .GetPagedAsync(page, size);
            }
            catch (Exception ex)
            {
                return new RetrievingDataFromDbContextError(ex);
            }
        }

        public async Task<Result<Medicine>> GetMedicineByIdAsync(string id, params Expression<Func<Medicine, object>>[]? includes)
        {
            try
            {
                IQueryable<Medicine> medicinesQuery = _context.Medicines.Where(item => item.Id == id);
                if (includes is not null)
                {
                    foreach (var item in includes)
                    {
                        medicinesQuery = medicinesQuery.Include(item);
                    }
                }
                var medicine = await medicinesQuery.FirstOrDefaultAsync();
                return medicine is null ? EntityNotExistsError.Happen<Medicine>() : medicine;
            }
            catch (Exception ex)
            {
                return new RetrievingDataFromDbContextError(ex);
            }
        }

        public async Task<Result<MedicinePharmacy?>> GetMedicineInPharmacyAsync(string pharmacyId, string medicineId)
        {
            try
            {
                return await _context.MedicinePharmacies
                    .Where(item => item.PharmacyId == pharmacyId && item.MedicineId == medicineId)
                    .Include(i => i.Medicine)
                    .FirstOrDefaultAsync();
            }
            catch (Exception ex)
            {
                return new RetrievingDataFromDbContextError(ex);
            }
        }

        public async Task<Result<IEnumerable<Medicine>>> GetAllMedicinesAsync(string? query)
        {
            try
            {
                return await _context.Medicines.Where(m => string.IsNullOrEmpty(query) || m.Name.Trim().Contains(query.Trim(), StringComparison.OrdinalIgnoreCase)).ToListAsync();
            }
            catch (Exception ex)
            {
                return new RetrievingDataFromDbContextError(ex);
            }
        }
    }
}
