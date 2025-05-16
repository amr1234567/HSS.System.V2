using FluentResults;

using HSS.System.V2.DataAccess.Contexts;
using HSS.System.V2.DataAccess.Contracts;
using HSS.System.V2.Domain.Helpers.Models;
using HSS.System.V2.Domain.ResultHelpers.Errors;
using HSS.System.V2.Domain.Helpers.Methods;

using Microsoft.EntityFrameworkCore;

using System.Linq.Expressions;
using HSS.System.V2.Domain.Models.Common;
using HSS.System.V2.Domain.Models.Appointments;
using HSS.System.V2.Domain.Enums;
using HSS.System.V2.Domain.Models.Medical;
using HSS.System.V2.Domain.Models.Queues;

namespace HSS.System.V2.DataAccess.Repositories
{
    public class AppointmentRepository : IAppointmentRepository
    {
        private readonly AppDbContext _context;

        public AppointmentRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Result> CreateAppointmentAsync(Appointment model)
        {
            try
            {
                await _context.Appointments.AddAsync(model);
                await _context.SaveChangesAsync();
                return Result.Ok();
            }
            catch (Exception ex)
            {
                return new SavingDataInDbContextError(ex);
            }
        }

        public async Task<Result> UpdateAppointmentAsync(Appointment model)
        {
            try
            {
                _context.Appointments.Remove(model);
                await _context.SaveChangesAsync();
                return Result.Ok();
            }
            catch (Exception ex)
            {
                return new SavingDataInDbContextError(ex);
            }
        }

        public async Task<Result> BulkUpdateAppointmentAsync<T>(Expression<Func<T, bool>> condition, Action<T> action) where T : Appointment
        {
            try
            {
                var appointments = await _context.Appointments.OfType<T>().Where(condition).ToListAsync();
                foreach (var app in appointments)
                {
                    action(app);
                    _context.Appointments.Update(app);
                    await _context.SaveChangesAsync();
                }
                return Result.Ok();
            }
            catch (Exception ex)
            {
                return new SavingDataInDbContextError(ex);
            }
        }

        public async Task<Result> DeleteAppointmentAsync(Appointment model)
        {
            try
            {
                _context.Appointments.Remove(model);
                await _context.SaveChangesAsync();
                return Result.Ok();
            }
            catch (Exception ex)
            {
                return new SavingDataInDbContextError(ex);
            }
        }

        public async Task<Result<T>> GetAppointmentByIdAsync<T>(string id) where T : Appointment
        {
            try
            {
                var appointment = await _context.Appointments
                    .Where(a => a.Id == id)
                    .Include(c => c.Ticket)
                    .Include(a => ((ClinicAppointment)a).Clinic)
                        .ThenInclude(c => c.CurrentWorkingDoctor)
                    .Include(a => ((MedicalLabAppointment)a).MedicalLab)
                        .ThenInclude(c => c.CurrentWorkingTester)
                    .Include(a => ((RadiologyCeneterAppointment)a).RadiologyCeneter)
                        .ThenInclude(c => c.CurrentWorkingTester)
                    .OfType<T>()
                    .AsNoTracking()
                    .FirstOrDefaultAsync();
                return appointment is null ? EntityNotExistsError.Happen<Appointment>(id) : appointment;
            }
            catch (Exception ex)
            {
                return new RetrievingDataFromDbContextError(ex);
            }
        }

        public async Task<Result<Appointment>> GetAppointmentByDateTimeInDepartmentAsync<TDept>(string departmentId, DateTime dateTime) where TDept : IHospitalDepartmentItem
        {
            try
            {
                var baseQuery = _context.Appointments
                     .AsNoTracking()                                                  // no-tracking query :contentReference[oaicite:0]{index=0}
                     .Where(a => a.SchaudleStartAt == dateTime);                // filter by date/time :contentReference[oaicite:1]{index=1}

                // Narrow by department type using switch expression on TDept
                IQueryable<Appointment> filteredQuery = typeof(TDept) switch       // switch expression & type pattern :contentReference[oaicite:2]{index=2}
                {
                    Type t when t == typeof(ClinicAppointment) =>
                        baseQuery
                            .OfType<ClinicAppointment>()                            // filter to ClinicAppointment rows :contentReference[oaicite:3]{index=3}
                            .Where(c => c.ClinicId == departmentId),

                    Type t when t == typeof(MedicalLabAppointment) =>
                        baseQuery
                            .OfType<MedicalLabAppointment>()
                            .Where(m => m.MedicalLabId == departmentId),

                    Type t when t == typeof(RadiologyCeneterAppointment) =>
                        baseQuery
                            .OfType<RadiologyCeneterAppointment>()
                            .Where(r => r.RadiologyCeneterId == departmentId),

                    _ => baseQuery.Where(a => false)                               // no matches for other TDept :contentReference[oaicite:4]{index=4}
                };

                // Execute and fetch single result
                var appointment = await filteredQuery
                    .FirstOrDefaultAsync();                                      // async execution :contentReference[oaicite:5]{index=5};

                return appointment is null
                    ? EntityNotExistsError.Happen<Appointment>(departmentId)
                    : appointment;
                    }
            catch (Exception ex)
            {
                return new RetrievingDataFromDbContextError(ex);
            }
        }

        public async Task<Result<List<ClinicAppointment>>> GetAllForClinicAsync(string clinicId, DateFilterationRequest dateFilters)
        {
            var appointments = await _context.ClinicAppointments
                    .Where(a => a.ClinicId == clinicId)
                    .AsNoTracking()
                    .Include(c => c.Clinic)
                    .Include(c => c.Doctor)
                    .Include(c => c.Ticket)
                        .ThenInclude(t => t.Patient)
                    //.FilterByDate(dateFilters.DateFrom, dateFilters.DateTo)
                    .OrderByDescending(a => a.SchaudleStartAt)
                    .ToListAsync();
            return Result.Ok(appointments);
        }

        public async Task<Result<List<RadiologyCeneterAppointment>>> GetAllForRadiologyCenterAsync(string radiologyCenterId, DateFilterationRequest dateFilters)
        {
            try
            {
                var appointments = await _context.RadiologyCeneterAppointments.Where(a => a.RadiologyCeneterId.Equals(radiologyCenterId))
                    .FilterByDate(dateFilters)
                    .ToListAsync();
                return Result.Ok(appointments);
            }
            catch (Exception ex)
            {
                return new RetrievingDataFromDbContextError(ex);
            }
        }

        public async Task<Result<List<MedicalLabAppointment>>> GetAllForMedicalLabAsync(string medicalLabId, DateFilterationRequest dateFilters)
        {
            try
            {
                var appointments = await _context.MedicalLabAppointments.Where(a => a.MedicalLabId.Equals(medicalLabId))
                    .FilterByDate(dateFilters)
                    .ToListAsync();
                return Result.Ok(appointments);
            }
            catch (Exception ex)
            {
                return new RetrievingDataFromDbContextError(ex);
            }
        }

        public async Task<Result<List<Appointment>>> GetAllForHospitalAsync(string hospitalId, DateFilterationRequest dateFilters)
        {
            return await _context.Hospitals
               .Where(c => c.Id == hospitalId)
               .Include(c => c.Appointments)
               .SelectMany(c => c.Appointments)
               .Where(a => a.SchaudleStartAt >= dateFilters.DateFrom && a.SchaudleStartAt <= dateFilters.DateTo)
               .AsNoTracking()
               .OrderByDescending(a => a.SchaudleStartAt)
               .ToListAsync();
        }

        public async Task<Result<List<Appointment>>> GetAllForHospitalAsync
            (string hospitalId, string specializationId, DateFilterationRequest dateFilters)
        {
            return await _context.Clinics
                .Where(c => c.HospitalId == hospitalId && c.SpecializationId == specializationId)
                .Include(c => c.ClinicAppointments)
                .SelectMany(c => c.ClinicAppointments)
                .Where(a => a.SchaudleStartAt >= dateFilters.DateFrom && a.SchaudleStartAt <= dateFilters.DateTo)
                .AsNoTracking()
                .OrderByDescending(a => a.SchaudleStartAt)
                .Cast<Appointment>()
                .ToListAsync();
        }

        public async Task<Result> DeleteAppointmentAsync(string appointmentId)
        {
            try
            {
                var appointment = await _context.Appointments
               .Where(a => a.Id == appointmentId)
               .FirstOrDefaultAsync();
                if (appointment is null)
                    return EntityNotExistsError.Happen<Appointment>(appointmentId);
                _context.Appointments.Remove(appointment);
                await _context.SaveChangesAsync();
                return Result.Ok();
            }
            catch (Exception ex)
            {
                return new SavingDataInDbContextError(ex);
            }
        }

        public async Task<Result> SwapAppointmentsAsync<T>(string appointmentId1, string appointmentId2) where T: Appointment
        {
            try
            {
                var appointment1Result = await GetAppointmentByIdAsync<T>(appointmentId1);
                if (appointment1Result.IsFailed)
                    return Result.Fail(appointment1Result.Errors);

                var appointment2Result = await GetAppointmentByIdAsync<T>(appointmentId2);
                if (appointment2Result.IsFailed)
                    return Result.Fail(appointment2Result.Errors);

                (appointment2Result.Value.SchaudleStartAt, appointment1Result.Value.SchaudleStartAt) = (appointment1Result.Value.SchaudleStartAt, appointment2Result.Value.SchaudleStartAt);

                _context.Appointments.UpdateRange(appointment1Result.Value, appointment2Result.Value);
                await _context.SaveChangesAsync();
                return Result.Ok();
            }
            catch (Exception ex)
            {
                return new SavingDataInDbContextError(ex);
            }
        }

        public async Task<Result<Appointment?>> GetAppointmentByIdAsync(string id)
        {
            return await _context.Appointments
                .Where(a => a.Id == id)
                .Include(a => a.Hospital)
                .Include(a => a.Ticket)
                .FirstOrDefaultAsync();
        }

        public async Task<Result<PagedResult<Appointment>>> GetAllAppointmentsForUser(string apiUserId, PaginationRequest pagination)
        {
            return await _context.Appointments
                .Where(a => a.PatientNationalId == apiUserId)
                .AsNoTracking()
                .OrderByDescending(a => a.SchaudleStartAt)
                .GetPagedAsync(pagination);
        }

        public async Task<Result<PagedResult<Appointment>>> GetAllAppointmentsForUser(string apiUserId, PaginationRequest pagination, AppointmentState state)
        {
            return await _context.Appointments
                .Where(a => a.PatientNationalId == apiUserId && a.State == state)
                .AsNoTracking()
                .OrderByDescending(a => a.SchaudleStartAt)
                .GetPagedAsync(pagination);
        }

        public async Task<Result<Appointment?>> GetAppointmentByIdBlukIncludeAsync(string appointmentId)
        {
            return await _context.Appointments
                .Where(a => a.Id == appointmentId)
                .Include(a => a.Hospital)
                .Include(a => a.Ticket)
                .Include(a => ((ClinicAppointment)a).Clinic)
                    .ThenInclude(c => c.CurrentWorkingDoctor)
                .Include(a => ((MedicalLabAppointment)a).MedicalLab)
                    .ThenInclude(c => c.CurrentWorkingTester)
                .Include(a => ((RadiologyCeneterAppointment)a).RadiologyCeneter)
                    .ThenInclude(c => c.CurrentWorkingTester)
                .FirstOrDefaultAsync();
        }

        public async Task<Result<IEnumerable<Appointment>>> GetAllAppointmentsForUser(string apiUserId, AppointmentState state)
        {
            return await _context.Tickets.AsNoTracking()
                .Where(t => t.PatientId == apiUserId)
                .Include(t => t.Appointments)
                .SelectMany(t => t.Appointments)
                .Where(t => t.State == state)
                .Include(a => a.Hospital)
                .OrderByDescending(a => a.SchaudleStartAt)
                .ToListAsync();
        }

        public async Task<Result<IEnumerable<Appointment>>> GetAllAppointmentsForUser(string apiUserId)
        {
            return await _context.Appointments
                .Where(a => a.PatientNationalId == apiUserId)
                .AsNoTracking()
                .OrderByDescending(a => a.SchaudleStartAt)
                .ToListAsync();
        }
    }
}
