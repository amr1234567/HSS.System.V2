using FluentResults;

using HSS.System.V2.DataAccess.Contexts;
using HSS.System.V2.DataAccess.Contracts;
using HSS.System.V2.Domain.Models.Appointments;
using HSS.System.V2.Domain.Models.Prescriptions;
using HSS.System.V2.Domain.ResultHelpers.Errors;

using Microsoft.EntityFrameworkCore;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HSS.System.V2.DataAccess.Repositories
{
    public class PrescriptionRepository(AppDbContext context) : IPrescriptionRepository
    {
        public async Task<Result> CreateMedicalPrescription(Prescription model)
        {
            try
            {
                if (model is null)
                    return Result.Fail("Prescription is requred");

                await context.Prescriptions.AddAsync(model);
                await context.SaveChangesAsync();
                return Result.Ok();
            }
            catch (Exception ex)
            {
                return new UnKnownError(ex);
            }
        }

        public async Task<Result<Prescription>> GetMedicalPrescriptionById(string id)
        {
            try
            {
                if (string.IsNullOrEmpty(id))
                    return Result.Fail("Id is requred");

                var prescriptions = await context.Prescriptions.FindAsync(id);
                
                return prescriptions is null ? Result.Fail("Prescription not found") : Result.Ok(prescriptions);
            }
            catch (Exception ex)
            {
                return new UnKnownError(ex);
            }
        }

        public async Task<Result<PrescriptionMedicineItem>> GetMedicalPrescriptionItemById(string id)
        {
            try
            {
                if (string.IsNullOrEmpty(id))
                    return Result.Fail("Id is requred");

                var medicines = await context.Prescriptions.Where(item => item.Id == id)
                    .SelectMany(p => p.Items).FirstOrDefaultAsync();

                return medicines is null ? Result.Fail("there are not Medicines in this Prescription") : Result.Ok(medicines);
            }
            catch (Exception ex)
            {
                return new UnKnownError(ex);
            }
        }

        public async Task<Result> UpdateMedicalPrescription(Prescription model)
        {
            try
            {
                if (model is null)
                    return Result.Fail("Prescription is requred");

                var prescription = await context.Prescriptions.FindAsync(model.Id);
                if (prescription is null)
                    return Result.Fail("Prescription not found");

                context.Entry(prescription).CurrentValues.SetValues(model);
                await context.SaveChangesAsync();
                return Result.Ok();
            }
            catch (Exception ex)
            {
                return new UnKnownError(ex);
            }
        }

        public async Task<Result<IEnumerable<Prescription>>> GetAllMedicalPrescription(string userId)
        {
            
            return await context.ClinicAppointments.Where(a => a.PatientId == userId && a.Prescription != null)
                .Include(a => a.Prescription)
                    .ThenInclude(p => p.Items)
                .Select(a => a.Prescription)
                .ToListAsync();

            //return await context.Tickets
            //    .Where(x => x.PatientId == userId)
            //    .SelectMany(x => x.Appointments)
            //    .OfType<ClinicAppointment>()
            //    .Select(x => x.Prescription)
            //    .ToListAsync();
        }

        public async Task<Result> DeleteMedicalPrescription(Prescription model)
        {
            try
            {
                context.Prescriptions.Remove(model);
                await context.SaveChangesAsync();
                return Result.Ok();
            }
            catch (Exception ex)
            {
                return new UnKnownError(ex);
            }
        }
    }
}
