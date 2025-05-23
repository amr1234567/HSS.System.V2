﻿using FluentResults;

using HSS.System.V2.DataAccess.Contexts;
using HSS.System.V2.DataAccess.Contracts;
using HSS.System.V2.Domain.Models.People;

using Microsoft.EntityFrameworkCore;

using System.Linq.Expressions;

namespace HSS.System.V2.DataAccess.Repositories
{
    public class PatientRepository : IPatientRepository
    {
        private readonly AppDbContext _context;

        public PatientRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Result<Patient?>> GetPatientById(string id, params Expression<Func<Patient, object>>[] includes)
        {
            var query = _context.Patients.AsNoTracking()
                .Where(x => x.Id == id);
            if (includes != null && includes.Length > 0)
            {
                foreach (var include in includes)
                {
                    query = query.Include(include);
                }
            }
            var patient = await query.FirstOrDefaultAsync();
            return patient;
        }

        public async Task<Result<Patient?>> GetPatientByNationalIdOrEmail(string nationalId, string email)
        {
            var patient = await _context.Patients.FirstOrDefaultAsync(p => p.NationalId == nationalId || p.Email == email);
            return patient;
        }

        public Task<Result<Patient>> GetPatientWithMedicalHistoryById(string id)
        {
            throw new NotImplementedException();
        }

        public Task<Result<Patient>> GetPatientWithMedicalHistoryByNationalId(string id)
        {
            throw new NotImplementedException();
        }

        public async Task<Result<Patient?>> GetPatientByNationalId(string nationalId, params Expression<Func<Patient, object>>[] includes)
        {
            var query = _context.Patients.AsNoTracking()
                .Where(x => x.NationalId == nationalId);
            if (includes != null && includes.Length > 0)
            {
                foreach (var include in includes)
                {
                    query = query.Include(include);
                }
            }
            var patient = await query.FirstOrDefaultAsync();
            return Result.Ok(patient);
        }

        public async Task<Result> UpdatePatientPicture(string userId, string imagePath)
        {
            var user = await _context.Patients.FindAsync(userId);
            user!.UrlOfProfilePicutre = imagePath;
            await _context.SaveChangesAsync();
            return Result.Ok();
        }

        public Task<Result> CreateMedicalHistoryRecordFromEndedTicket(string ticketId)
        {
            throw new NotImplementedException();
        }

        public async Task<Result> RegisterPatient(Patient patient)
        {
            await _context.Patients.AddAsync(patient);
            await _context.SaveChangesAsync();
            return Result.Ok();
        }

        public async Task<Result> UpdatePatient(Patient patient)
        {
            _context.Patients.Update(patient);
            await _context.SaveChangesAsync();
            return Result.Ok();
        }
    }
}
