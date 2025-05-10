using FluentResults;

using HSS.System.V2.DataAccess.Contexts;
using HSS.System.V2.DataAccess.Contracts;
using HSS.System.V2.Domain.Helpers.Models;
using HSS.System.V2.Domain.Medical;
using HSS.System.V2.Domain.ResultHelpers.Errors;

namespace HSS.System.V2.DataAccess.Repositories
{
    public class TestsRepository(AppDbContext context) : ITestsRepository
    {
        public async Task<Result> CreateTestAsync(Test model)
        {
            try
            {
                await context.Tests.AddAsync(model);
                await context.SaveChangesAsync();
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
                context.Tests.Update(model);
                await context.SaveChangesAsync();
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
                var test = await context.Tests.FindAsync(id);
                if (test == null)
                    return EntityNotExistsError.Happen<Test>(id);
                context.Tests.Remove(test);
                await context.SaveChangesAsync();
                return Result.Ok();
            }
            catch (Exception ex)
            {
                return new SavingDataInDbContextError(ex);
            }
        }

        public Task<Result<IEnumerable<Test>>> GetAllTestsAsync<T>() where T : Test
        {
            throw new NotImplementedException();
        }

        public Task<Result<Test>> GetTestByIdAsync(string id)
        {
            throw new NotImplementedException();
        }

        public Task<Result> IsTestInHospital(string testId, string hospitalId)
        {
            throw new NotImplementedException();
        }

        public Task<Result<PagedResult<Test>>> GetAllTestsInHospitalAsync(string hospitalId, int size = 10, int page = 1)
        {
            throw new NotImplementedException();
        }
    }
}
