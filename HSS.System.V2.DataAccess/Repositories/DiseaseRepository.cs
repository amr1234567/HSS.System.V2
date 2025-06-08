using FluentResults;

using HSS.System.V2.DataAccess.Contexts;

using Microsoft.EntityFrameworkCore;
using HSS.System.V2.Domain.Models.Medical;
using HSS.System.V2.DataAccess.Contracts;

namespace HSS.System.V2.DataAccess.Repositories;

public class DiseaseRepository : IDiseaseRepository
{
    private readonly AppDbContext _context;

    public DiseaseRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<Result<Disease>> GetDiseaseById(string diseaseId)
    {
        return await _context.Diseases.FirstOrDefaultAsync(d => d.Id == diseaseId);
    }

    public async Task<Result<IEnumerable<Disease>>> GetAllDiseases(string? querySearch)
    {
        return await _context.Diseases.Where(d => string.IsNullOrEmpty(querySearch) || (d.Name.Trim().Contains(querySearch.Trim(), StringComparison.CurrentCultureIgnoreCase) || d.Description.Trim().Contains(querySearch.Trim(), StringComparison.CurrentCultureIgnoreCase))).ToListAsync();
    }
}
