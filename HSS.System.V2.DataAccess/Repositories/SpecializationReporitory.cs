using FluentResults;

using HSS.System.V2.DataAccess.Contexts;
using HSS.System.V2.DataAccess.Contracts;
using HSS.System.V2.Domain.Helpers.Models;
using HSS.System.V2.Domain.Models.Medical;
using HSS.System.V2.Domain.Helpers.Methods;

using Microsoft.EntityFrameworkCore;

namespace HSS.System.V2.DataAccess.Repositories
{
    public class SpecializationReporitory : ISpecializationReporitory
    {
        private readonly AppDbContext _context;

        public SpecializationReporitory(AppDbContext context)
        {
            _context = context;
        }
        public async Task<Result<IEnumerable<Specialization>>> GetAllAsync()
        {
            return await _context.Specializations.AsNoTracking()
                .Include(s => s.Clinics)
                .ToListAsync();
        }

        public async Task<Result<PagedResult<Specialization>>> GetAllAsync(PaginationRequest pagination)
        {
            return await _context.Specializations.AsNoTracking()
                .Include(s => s.Clinics)
                .GetPagedAsync(pagination.Page, pagination.Size);
        }

        public async Task<Result<IEnumerable<Specialization>>> GetAllInHospitalAsync(string hospitalId)
        {
            return await _context.Specializations.AsNoTracking()
                .Include(s => s.Clinics)
                .Where(s => s.Clinics.Any(c => c.HospitalId == hospitalId))
                .ToListAsync();
        }
    }
}
