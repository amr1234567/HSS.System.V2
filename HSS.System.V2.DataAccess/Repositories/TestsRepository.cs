using FluentResults;

using HSS.System.V2.DataAccess.Contexts;
using HSS.System.V2.DataAccess.Contracts;
using HSS.System.V2.Domain.Helpers.Models;
using HSS.System.V2.Domain.Medical;
using HSS.System.V2.Domain.ResultHelpers.Errors;
using HSS.System.V2.Domain.Helpers.Methods;
using Microsoft.EntityFrameworkCore;
using HSS.System.V2.Domain.Common;


namespace HSS.System.V2.DataAccess.Repositories
{
    public class TestsRepository : ITestsRepository
    {
        private readonly AppDbContext _context;

        public TestsRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Result> CreateTestAsync(Test model)
        {
            try
            {
                await _context.Tests.AddAsync(model);
                await _context.SaveChangesAsync();
                return Result.Ok();
            }
            catch (Exception ex)
            {
                return new SavingDataInDbContextError(ex);
            }
        }

        public async Task<Result> UpdateRadiologyTestAsync(Test model)
        {

            try
            {
                _context.Tests.Update(model);
                await _context.SaveChangesAsync();
                return Result.Ok();
            }
            catch (Exception ex)
            {
                return new SavingDataInDbContextError(ex);
            }
        }

        public async Task<Result> DeleteRadiologyTestAsync(string id)
        {
            try
            {
                var test = await _context.Tests.FindAsync(id);
                if (test == null)
                    return EntityNotExistsError.Happen<Test>(id);
                _context.Tests.Remove(test);
                await _context.SaveChangesAsync();
                return Result.Ok();
            }
            catch (Exception ex)
            {
                return new SavingDataInDbContextError(ex);
            }
        }

        public async Task<Result<PagedResult<T>>> GetAllTestsAsync<T>(PaginationRequest pagination) where T : Test
        {
            return await _context.Set<T>().AsNoTracking().GetPagedAsync(pagination);
        }

        public async Task<Result<Test>> GetTestByIdAsync(string id)
        {
            var test = await _context.Tests.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);
            if (test == null)
                return EntityNotExistsError.Happen<Test>(id);
            return test;
        }

        public async Task<Result> IsTestInHospital<TDept, ITest>(string testId, string hospitalId)
            where TDept : BaseClass, IHospitalDepartmentItem, ITestableDepartment<TDept, ITest>
            where ITest : Test
        {
            var test = await _context.Set<TDept>()
                .Include(x => x.GetInclude())
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == hospitalId);
            return test is not null ? Result.Ok() : Result.Fail(new Error(""));
        }

        public Task<Result<PagedResult<Test>>> GetAllTestsInHospitalAsync(string hospitalId, int size = 10, int page = 1)
        {
            throw new NotImplementedException();
        }
    }
}
