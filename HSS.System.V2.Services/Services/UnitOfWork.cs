using HSS.System.V2.DataAccess.Contexts;
using HSS.System.V2.Services.Contracts;

namespace HSS.System.V2.Services.Services;

public class UnitOfWork : IUnitOfWork
{
    private readonly AppDbContext _context;

    public UnitOfWork(AppDbContext context)
    {
        _context = context;
    }

    public async Task<int> SaveAllChanges()
    {
        return await _context.SaveChangesAsync();
    }
}