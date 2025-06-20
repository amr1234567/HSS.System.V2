﻿using FluentResults;

using HSS.System.V2.DataAccess.Contexts;
using HSS.System.V2.DataAccess.Contracts;
using HSS.System.V2.Domain.Enums;
using HSS.System.V2.Domain.Helpers.Methods;
using HSS.System.V2.Domain.Helpers.Models;
using HSS.System.V2.Domain.Models.Appointments;
using HSS.System.V2.Domain.Models.Prescriptions;
using HSS.System.V2.Domain.ResultHelpers.Errors;

using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography.Xml;

namespace HSS.System.V2.DataAccess.Repositories
{
    public class TicketRepository : ITicketRepository
    {
        private readonly AppDbContext _context;

        public TicketRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Result> CreateTicket(Ticket model)
        {
            try
            {
                await _context.Tickets.AddAsync(model);
                await _context.SaveChangesAsync();
                return Result.Ok();
            }
            catch (Exception ex)
            {
                return new UnKnownError(ex);
            }
        }

        public async Task<Result> UpdateTicket(Ticket model)
        {
            try
            {
                model.UpdatedAt = DateTime.UtcNow;

                _context.Tickets.Update(model);
                return Result.Ok();
            }
            catch (Exception ex)
            {
                return new UnKnownError(ex);
            }
        }

        public async Task<Result> DeleteTicket(Ticket model)
        {
            try
            {
                _context.Tickets.Remove(model);
                await _context.SaveChangesAsync();
                return Result.Ok();
            }
            catch (Exception ex)
            {
                return new UnKnownError(ex);
            }
        }

        public async Task<Result<Ticket>> GetTicketById(string ticketId)
        {
            try
            {
                var ticket = await _context.Tickets
                    .Where(t => t.Id == ticketId)
                    .FirstOrDefaultAsync();
                return ticket is null ? EntityNotExistsError.Happen<Ticket>(ticketId) : ticket;
            }
            catch (Exception ex)
            {
                return new RetrievingDataFromDbContextError(ex);
            }
        }

        public async Task<Result<Ticket>> GetTicketByIdInDetails(string ticketId)
        {
            try
            {
                var ticket = await _context.Tickets
                    .Where(t => t.Id == ticketId)
                    .Include(t => t.Appointments)
                        .ThenInclude(a => ((ClinicAppointment)a).MedicalLabAppointments)
                    .Include(t => t.Appointments)
                        .ThenInclude(a => ((ClinicAppointment)a).RadiologyCeneterAppointments)
                    .FirstOrDefaultAsync();
                return ticket is null ? EntityNotExistsError.Happen<Ticket>(ticketId) : ticket;
            }
            catch (Exception ex)
            {
                return new RetrievingDataFromDbContextError(ex);
            }
        }

        public async Task<Result<PagedResult<Ticket>>> GetOpenTicketsForPatient(string patientId, int size = 10, int page = 1)
        {
            return await _context.Tickets
                .AsNoTracking()
                .Where(t => t.PatientId == patientId && t.State == TicketState.Active)
                .GetPagedAsync(page, size);
        }

        public async Task<Result<IEnumerable<Ticket>>> GetOpenTicketsForPatient(string patientId)
        {
            return await _context.Tickets
                .AsNoTracking()
                .Where(t => t.PatientId == patientId && t.State == TicketState.Active)
                .Include(t => t.Appointments)
                .Include(t => t.FirstClinicAppointment)
                    .ThenInclude(t => t.ReExamiationClinicAppointemnt)
                .Include(t => t.FirstClinicAppointment)
                .OrderBy(t => t.State)
                    .ThenByDescending(t => t.CreatedAt)
                .ToListAsync();
        }

        public async Task<Result<PagedResult<Ticket>>> GetOpenTicketsForPatientByNationalId(string nationalId, int size = 10, int page = 1)
        {
            return await _context.Tickets
                .AsNoTracking()
                .Where(t => t.PatientNationalId == nationalId && t.State == TicketState.Active)
                .OrderBy(t => t.State)
                    .ThenByDescending(t => t.CreatedAt)
                .GetPagedAsync(page, size);
        }

        public async Task<Result<PagedResult<Ticket>>> GetAllTicketForPatient(string patientId, int size = 10, int page = 1)
        {
            return await _context.Tickets
                .AsNoTracking()
                .Where(t => t.PatientId == patientId)
                .OrderBy(t => t.State)
                    .ThenByDescending(t => t.CreatedAt)
                .GetPagedAsync(page, size);
        }

        public async Task<Result<PagedResult<Ticket>>> GetAllTicketForPatientByNationalId(string patientNationalId, int size = 10, int page = 1)
        {
            return await _context.Tickets
                .AsNoTracking()
                .Where(t => t.PatientNationalId == patientNationalId)
                .OrderBy(t => t.State)
                    .ThenByDescending(t => t.CreatedAt)
                .GetPagedAsync(page, size);
        }

        public async Task<Result<PagedResult<Ticket>>> GetAllOpenedTicketInHospitalForPatient(string hospitalId, string patientId, int size = 10, int page = 1)
        {
            return await _context.Tickets.AsNoTracking()
                .Where(x => x.HospitalCreatedInId == hospitalId && x.PatientId == patientId && x.State == TicketState.Active)
                .OrderBy(t => t.State)
                    .ThenByDescending(t => t.CreatedAt)
                .GetPagedAsync(page, size);
        }

        public async Task<Result> DeleteTicket(string ticketId)
        {
            var ticket = await GetTicketById(ticketId);

            if (ticket.IsFailed)
                return Result.Fail(ticket.Errors);
            _context.Tickets.Remove(ticket.Value);
            await _context.SaveChangesAsync();
            return Result.Ok();
        }

        public async Task<Result<bool>> IsTicketHasReExaminationNow(string ticketId)
        {
            var ticket = await _context.Tickets
                    .AsNoTracking()
                    .Where(t => t.Id == ticketId)
                    .Include(t => t.FirstClinicAppointment)
                        .ThenInclude(c => c.ReExamiationClinicAppointemnt)
                    .Include(t => t.Appointments)
                    .FirstOrDefaultAsync();
            if (ticket is null)
                return EntityNotExistsError.Happen<Ticket>(ticketId);

            var clinicAppointment = ticket.FirstClinicAppointment;
            if (clinicAppointment is null)
                return false;
            while(clinicAppointment.ReExamiationClinicAppointemnt is not null)
            {
                clinicAppointment = clinicAppointment.ReExamiationClinicAppointemnt;
            }
            return clinicAppointment.ReExaminationNeeded ?? false;
        }

        public async Task<Result<IEnumerable<Ticket>>> GetAllTicketForPatient(string patientId)
        {
            var tickets = await _context.Tickets.AsNoTracking()
                .Where(x => x.PatientId == patientId)
                .OrderBy(t => t.State)
                    .ThenByDescending(t => t.CreatedAt)
                .ToListAsync();
            return tickets;
        }

        public async Task<Result<IEnumerable<Ticket>>> GetAllOpenedTicketInHospitalForPatient(string hospitalId, string patientId)
        {
            var tickets = await _context.Tickets.AsNoTracking()
                .Where(x => x.HospitalCreatedInId == hospitalId && x.PatientId == patientId && x.State == TicketState.Active)
                .OrderBy(t => t.State)
                    .ThenByDescending(t => t.CreatedAt)
                .ToListAsync();
            return tickets;
        }
    }
}
