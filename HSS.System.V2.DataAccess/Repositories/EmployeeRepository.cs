using FluentResults;

using HSS.System.V2.DataAccess.Contexts;
using HSS.System.V2.DataAccess.Contracts;
using HSS.System.V2.Domain.Enums;
using HSS.System.V2.Domain.Helpers.Methods;
using HSS.System.V2.Domain.Models.People;

using Microsoft.EntityFrameworkCore;

namespace HSS.System.V2.DataAccess.Repositories
{
    public class EmployeeRepository : IEmployeeRepository
    {
        private readonly AppDbContext _context;

        public EmployeeRepository(AppDbContext context)
        {
            _context = context;
        }
        public async Task<Result<Employee?>> GetEmployeeByNationalId(string nationalId)
        {
            return await _context.Employees.Where(e => e.NationalId == nationalId).FirstOrDefaultAsync();
        }

        public async Task<Result<Employee?>> GetEmployeeById(string id)
        {
            return await _context.Employees.Where(e => e.Id == id).FirstOrDefaultAsync();
        }

        public async Task<Result> CreateLoginActivity(Employee employee, ActivityType activityType = ActivityType.Login)
        {
            await _context.LoginActivities.AddAsync(new()
            {
                ActivityType = activityType,
                CreatedAt = HelperDate.GetCurrentDate(),
                EmployeeId = employee.Id,
                EmployeeName = employee.Name,
                Id = Guid.NewGuid().ToString(),
                UpdatedAt = HelperDate.GetCurrentDate(),
            });
            await _context.SaveChangesAsync();
            return Result.Ok();
        }
    }
}
