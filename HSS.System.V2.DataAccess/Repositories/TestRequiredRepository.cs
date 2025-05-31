using FluentResults;

using HSS.System.V2.DataAccess.Contexts;
using HSS.System.V2.DataAccess.Contracts;
using HSS.System.V2.Domain.Enums;
using HSS.System.V2.Domain.Models.Medical;
using HSS.System.V2.Domain.Models.Prescriptions;
using HSS.System.V2.Domain.ResultHelpers.Errors;

using Microsoft.EntityFrameworkCore;

namespace HSS.System.V2.DataAccess.Repositories
{
    public class TestRequiredRepository : ITestRequiredRepository
    {
        private readonly AppDbContext _context;

        public TestRequiredRepository(AppDbContext context)
        {
            _context = context;
        }
        public async Task<Result> CreateTestRequiredAsync(TestRequired model)
        {
            try
            {
                await _context.TestsRequired.AddAsync(model);
                await _context.SaveChangesAsync();
                return Result.Ok();
            }
            catch (Exception ex)
            {
                return new SavingDataInDbContextError(ex);
            }
        }

        public async Task<Result> UpdateTestRequiredAsync(TestRequired model)
        {
            try
            {
                _context.TestsRequired.Update(model);
                await _context.SaveChangesAsync();
                return Result.Ok();
            }
            catch (Exception ex)
            {
                return new SavingDataInDbContextError(ex);
            }
        }

        public async Task<Result> DeleteTestRequiredAsync(string id)
        {
            try
            {
                var testRequired = await _context.TestsRequired.FindAsync(id);
                if (testRequired == null)
                {
                    return EntityNotExistsError.Happen<TestRequired>(id);
                }
                _context.TestsRequired.Remove(testRequired);
                await _context.SaveChangesAsync();
                return Result.Ok(); 
            }
            catch (Exception ex)
            {
                return new SavingDataInDbContextError(ex);
            }
        }

        public async Task<Result<IEnumerable<TestRequired>>> GetNotUsedTestsRequiredAvailableInTicket(string ticketId)
        {
            var ticket = await _context.Tickets
                .Where(t => t.Id == ticketId && t.State == TicketState.Active)
                .Include(t => t.FirstClinicAppointment)
                    .ThenInclude(c => c.TestsRequired)
                .Include(t => t.FirstClinicAppointment)
                    .ThenInclude(c => c.ReExamiationClinicAppointemnt)
                .FirstOrDefaultAsync();
            if (ticket == null)
            {
                return EntityNotExistsError.Happen<Ticket>(ticketId);
            }
            var tests = new List<TestRequired>();
            var clinicAppointment = ticket.FirstClinicAppointment;
            if (clinicAppointment is null)
                return tests;
            while (clinicAppointment is not null)
            {
                tests.AddRange(clinicAppointment.TestsRequired.Where(t => !t.Used));
                clinicAppointment = clinicAppointment.ReExamiationClinicAppointemnt;
            }
            return tests;
        }

        public async Task<Result<IEnumerable<TestRequired>>> GetAllTestsRequiredAvailableInTicket(string ticketId)
        {
            var ticket = await _context.Tickets
                .Where(t => t.Id == ticketId && t.State == TicketState.Active)
                .Include(t => t.FirstClinicAppointment)
                    .ThenInclude(c => c.TestsRequired)
                .Include(t => t.FirstClinicAppointment)
                    .ThenInclude(c => c.ReExamiationClinicAppointemnt)
                .FirstOrDefaultAsync();
            if (ticket == null)
            {
                return EntityNotExistsError.Happen<Ticket>(ticketId);
            }
            var tests = new List<TestRequired>();
            var clinicAppointment = ticket.FirstClinicAppointment;
            if (clinicAppointment is null)
                return tests;
            while (clinicAppointment is not null)
            {
                tests.AddRange(clinicAppointment.TestsRequired);
                clinicAppointment = clinicAppointment.ReExamiationClinicAppointemnt;
            }
            return tests;
        }

        public async Task<Result<IEnumerable<TestRequired>>> GetDoneTestsRequiredInTicket(string ticketId)
        {
            var ticket = await _context.Tickets
               .Where(t => t.Id == ticketId && t.State == TicketState.Active)
               .Include(t => t.FirstClinicAppointment)
                   .ThenInclude(c => c.TestsRequired)
               .Include(t => t.FirstClinicAppointment)
                   .ThenInclude(c => c.ReExamiationClinicAppointemnt)
               .AsNoTracking()
               .FirstOrDefaultAsync();
            if (ticket == null)
            {
                return EntityNotExistsError.Happen<Ticket>(ticketId);
            }
            var tests = new List<TestRequired>();
            var clinicAppointment = ticket.FirstClinicAppointment;
            if (clinicAppointment is null)
                return tests;
            while (clinicAppointment is not null)
            {
                tests.AddRange(clinicAppointment.TestsRequired.Where(t => t.Used));
                clinicAppointment = clinicAppointment.ReExamiationClinicAppointemnt;
            }
            return tests;
        }

        public async Task<Result<IEnumerable<TestRequired>>> GetAllTestsRequiredAvailableForUser(string userId)
        {
            return await _context.TestsRequired.AsNoTracking()
                .Where(x => x.PatientNationalId == userId || x.Used == false).ToListAsync();
        }

        public async Task<Result<TestRequired?>> GetTestRequiredByIdAsync(string? testRequiredId)
        {
            return await _context.TestsRequired.AsNoTracking()
                .Where(t => t.Id == testRequiredId)
                .Include(t => t.Test)
                .Include(t => t.ClinicAppointment)
                    .ThenInclude(t => t.Ticket)
                .FirstOrDefaultAsync();
        }
    }
}
