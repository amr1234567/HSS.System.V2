using FluentResults;

using HSS.System.V2.DataAccess.Contexts;
using HSS.System.V2.DataAccess.Contracts;
using HSS.System.V2.Domain.Helpers.Models;
using HSS.System.V2.Domain.ResultHelpers.Errors;
using HSS.System.V2.Domain.Helpers.Methods;
using Microsoft.EntityFrameworkCore;
using HSS.System.V2.Domain.Models.Common;
using HSS.System.V2.Domain.Models.Medical;
using HSS.System.V2.Domain.Models.Facilities;
using static System.Net.Mime.MediaTypeNames;


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

        public async Task<Result> UpdateTestAsync(Test model)
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

        public async Task<Result> DeleteTestAsync(string id)
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
            return await _context.Set<T>().AsNoTracking().Distinct().GetPagedAsync(pagination);
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

        public async Task<Result<PagedResult<TTest>>> GetAllTestsInHospitalAsync<TTest>(string hospitalId, int size = 10, int page = 1) where TTest : Test
        {
            try
            {
                if (typeof(TTest) == typeof(MedicalLabTest))
                {
                    return await _context.MedicalLabs.AsNoTracking()
                       .Include(x => x.Hospital)
                       .Where(x => x.HospitalId == hospitalId)
                       .SelectMany(x => x.Tests)
                       .Cast<TTest>()
                       .GetPagedAsync(page, size);
                }
                else if (typeof(TTest) == typeof(RadiologyTest))
                {
                    return await _context.RadiologyCenters.AsNoTracking()
                        .Include(x => x.Hospital)
                        .Where(x => x.HospitalId == hospitalId)
                        .SelectMany(x => x.Tests)
                        .Cast<TTest>()
                        .GetPagedAsync(page, size);
                }
                return Result.Fail("there are not tests");
            }
            catch (Exception ex)
            {
                return new UnKnownError(ex);
            }
        }

        public async Task<Result<IEnumerable<Hospital>>> GetAllHospitalsDoTest<TTest>(string testId) where TTest : Test
        {
            try
            {
                var hospitals = Enumerable.Empty<Hospital>();
                if (typeof(TTest) == typeof(MedicalLabTest))
                {
                    hospitals = await _context.MedicalLabTests
                    .AsNoTracking()
                    .Include(t => t.MedicalLabs)
                        .ThenInclude(m => m.Hospital)
                    .Where(x => x.Id == testId)
                    .SelectMany(t => t.MedicalLabs.Select(m => m.Hospital))
                    .Distinct()
                    .ToListAsync();
                }
                else if (typeof(TTest) == typeof(RadiologyTest))
                {
                    hospitals = await _context.RadiologyTests
                    .AsNoTracking()
                    .Include(t => t.RadiologyCenters)
                        .ThenInclude(m => m.Hospital)
                    .Where(x => x.Id == testId)
                    .SelectMany(t => t.RadiologyCenters.Select(m => m.Hospital))
                    .Distinct()
                    .ToListAsync();
                }

                return Result.Ok(hospitals);
            }
            catch (Exception ex)
            {
                return new UnKnownError(ex);
            }
        }
    }
}
