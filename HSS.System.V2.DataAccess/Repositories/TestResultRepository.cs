using FluentResults;
using HSS.System.V2.DataAccess.Contexts;
using HSS.System.V2.DataAccess.Contracts;
using HSS.System.V2.DataAccess.Migrations;
using HSS.System.V2.Domain.Models.Medical;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace HSS.System.V2.DataAccess.Repositories
{
    public class TestResultRepository : ITestResultRepository
    {
        private readonly AppDbContext _context;
        public TestResultRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Result> AddTestResult(ICollection<MedicalLabTestResult> result)
        {
            await _context.MedicalLabTestResults.AddRangeAsync(result);
            await _context.SaveChangesAsync();
            return Result.Ok();
        }

        public async Task<Result> DeleteTestResult(string appointmentId)
        {
            var result = await _context.MedicalLabTestResults.Where(x => x.AppointmentId == appointmentId).ToListAsync();
            if (result is null || !result.Any())
                return Result.Fail("there are not found");

            _context.MedicalLabTestResults.RemoveRange(result);
            await _context.SaveChangesAsync();
            return Result.Ok();
        }

        public async Task<Result<IEnumerable<MedicalLabTestResultField>>> GetMedicalLabTestResultFieldsAsync(string testId)
        {
            var test = await _context.MedicalLabTests.AsNoTracking()
                .Where(x => x.Id == testId)
                .SelectMany(x => x.Fields)
                .ToListAsync();

            if (test is null || !test.Any())
                return Result.Fail("there are not Fields");

            return Result.Ok<IEnumerable<MedicalLabTestResultField>>(test);
        }

        public async Task<Result<IEnumerable<MedicalLabTestResult>>> GetTestResult(string appointmentId)
        {
            var result = await _context.MedicalLabAppointments.AsNoTracking()
                .Where(x => x.Id == appointmentId)
                .SelectMany(x => x.TestResults)
                .ToListAsync();

            if (result is null || !result.Any())
                return Result.Fail("there are not result");

            return Result.Ok<IEnumerable<MedicalLabTestResult>>(result);
        }

        public async Task<Result> UpdateTestResult(ICollection<MedicalLabTestResult> result, string appointmentId)
        {
            var appointment = await _context.MedicalLabAppointments.Include(x => x.TestResults)
                .FirstOrDefaultAsync(x =>x.Id == appointmentId);
            if (appointment is null)
                return Result.Fail("this appointment not found");

            _context.MedicalLabTestResults.RemoveRange(appointment.TestResults);

            foreach (var testResult in result)
            {
                testResult.AppointmentId = appointmentId;
            }

            await _context.MedicalLabTestResults.AddRangeAsync(result);
            await _context.SaveChangesAsync();
            return Result.Ok();
        }
    }
}
