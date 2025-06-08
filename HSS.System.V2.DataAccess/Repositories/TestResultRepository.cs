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

        public async Task<Result> AddTestResult(IEnumerable<MedicalLabTestResultFieldValue> result)
        {
            await _context.MedicalLabTestResultFieldValues.AddRangeAsync(result);
            await _context.SaveChangesAsync();
            return Result.Ok();
        }

        public async Task<Result> DeleteTestResult(string appointmentId)
        {
            var result = await _context.MedicalLabTestResultFieldValues.Where(x => x.AppointmentId == appointmentId).ToListAsync();
            if (result is null || !result.Any())
                return Result.Fail("there are not found");

            _context.MedicalLabTestResultFieldValues.RemoveRange(result);
            await _context.SaveChangesAsync();
            return Result.Ok();
        }

        public async Task<Result<IEnumerable<MedicalLabTestResultField>>> GetMedicalLabTestResultFieldsAsync(string testId)
        {
            return await _context.MedicalLabTests
                .Where(x => x.Id == testId)
                .SelectMany(x => x.Fields)
                .ToListAsync();
        }

        public async Task<Result<IEnumerable<MedicalLabTestResultFieldValue>>> GetTestResult(string appointmentId)
        {
            var result = await _context.MedicalLabAppointments
                .Where(x => x.Id == appointmentId)
                .SelectMany(x => x.TestResultFieldValues)
                .ToListAsync();

            if (result is null || !result.Any())
                return Result.Fail("there are not result");

            return Result.Ok<IEnumerable<MedicalLabTestResultFieldValue>>(result);
        }

        public async Task<Result> UpdateTestResult(IEnumerable<MedicalLabTestResultFieldValue> result, string appointmentId)
        {
            var appointment = await _context.MedicalLabAppointments.Include(x => x.TestResultFieldValues)
                .FirstOrDefaultAsync(x =>x.Id == appointmentId);
            if (appointment is null)
                return Result.Fail("this appointment not found");

            _context.MedicalLabTestResultFieldValues.RemoveRange(appointment.TestResultFieldValues);

            foreach (var testResult in result)
            {
                testResult.AppointmentId = appointmentId;
            }

            await _context.MedicalLabTestResultFieldValues.AddRangeAsync(result);
            await _context.SaveChangesAsync();
            return Result.Ok();
        }

        public async Task<Result<MedicalLabTestResultField>> GetTestFieldById(string fieldId)
        {
            return await _context.MedicalLabTestResultFields.Where(f => f.Id == fieldId).FirstOrDefaultAsync();
        }
    }
}
