using FluentResults;

using HSS.System.V2.DataAccess.Contexts;
using HSS.System.V2.DataAccess.Contracts;
using HSS.System.V2.DataAccess.Helpers;
using HSS.System.V2.Domain.DTOs;
using HSS.System.V2.Domain.Helpers.Methods;
using HSS.System.V2.Domain.Helpers.Models;
using HSS.System.V2.Domain.Models.Common;
using HSS.System.V2.Domain.Models.Facilities;
using HSS.System.V2.Domain.ResultHelpers.Errors;

using Microsoft.EntityFrameworkCore;

namespace HSS.System.V2.DataAccess.Repositories;

public class HospitalRepository : IHospitalRepository
{
    private readonly AppDbContext _context;

    public HospitalRepository(AppDbContext context)
    {
        _context = context;
    }

    public Task<Result> CreateHospitalItem(Hospital hospital)
    {
        throw new NotImplementedException();
    }

    public Task<Result> DeleteHospitalItem(Hospital hospital)
    {
        throw new NotImplementedException();
    }

    public Task<Result> DeleteHospitalItem(string hospitalId)
    {
        throw new NotImplementedException();
    }

    public async Task<Result> UpdateHospitalItem<TDept>(TDept item) where TDept : class, IHospitalDepartmentItem
    {
        try
        {
            _context.Update(item);
            await _context.SaveChangesAsync();
            return Result.Ok();
        }
        catch (Exception ex)
        {
            return new RetrievingDataFromDbContextError(ex);
        }
    }

    public async Task<Result<PagedResult<Hospital>>> GetAllHospitalItems(int size = 10, int page = 1)
    {
        var hospitals = await _context.Hospitals.GetPagedAsync(page, size);
        if (hospitals is null)
            return Result.Fail<PagedResult<Hospital>>("No hospitals found");
        return Result.Ok(hospitals);
    }

    public Task<Result<PagedResult<Hospital>>> GetAllHospitalItemsInCity(int cityId, int size = 10, int page = 1)
    {
        throw new NotImplementedException();
    }

    public async Task<Result<PagedResult<Hospital>>> GetNearByHospitals(double lat, double lng, int size = 10, int page = 1)
    {
        try
        {
            var hospitals = await _context.Hospitals
                .OrderBy(h => DistanceHelper.GetDistance(lat, lng, h.Lat, h.Lng))
                .AsNoTracking()
                .GetPagedAsync(page, size);
            if (hospitals is null)
                return Result.Fail<PagedResult<Hospital>>("No hospitals found");
            return Result.Ok(hospitals);
        }
        catch (Exception ex)
        {
            return new RetrievingDataFromDbContextError(ex);
        }
    }

    public async Task<Result<Hospital>> GetHospitalById(string hospitalId)
    {
        try
        {
            var hospital = await _context.Hospitals.FindAsync(hospitalId);
            return hospital is null ? EntityNotExistsError.Happen<Hospital>(hospitalId) : Result.Ok(hospital);
        }
        catch (Exception ex)
        {
            return new RetrievingDataFromDbContextError(ex);
        }
    }

    public async Task<Result<Hospital>> GetHospitalByName(string hospitalName)
    {
        try
        {
            var hospital = await _context.Hospitals
              .Where(x => x.Name.Contains(hospitalName))
              .FirstOrDefaultAsync();
            return hospital is null ? EntityNotExistsError.Happen<Hospital>(hospitalName) : Result.Ok(hospital);
        }
        catch (Exception ex)
        {
            return new RetrievingDataFromDbContextError(ex);
        }
    }

    public async Task<Result<HospitalDepartments>> GetAllHospitalDepartments(string hospitalId)
    {
        var result = new HospitalDepartments
        {
            NumberOfReceptions = await _context.Receptions
                .Where(x => x.HospitalId == hospitalId)
                .CountAsync(),
            NumberOfClinics = await _context.Clinics
                .Where(x => x.HospitalId == hospitalId)
                .CountAsync(),
            NumberOfPharmacies = await _context.Pharmacies
                .Where(x => x.HospitalId == hospitalId)
                .CountAsync(),
            NumberOfRadiologyCenters = await _context.RadiologyCenters
                .Where(x => x.HospitalId == hospitalId)
                .CountAsync(),
            NumberOfMedicalLabs = await _context.MedicalLabs
                .Where(x => x.HospitalId == hospitalId)
                .CountAsync()
        };
        return result;
    }

    public async Task<Result<PagedResult<TDept>>> GetHospitalDepartmentItems<TDept>(string hospitalId, PaginationRequest pagination) where TDept : class, IHospitalDepartmentItem
    {
        try
        {
            IQueryable<TDept> filteredQuery = typeof(TDept) switch
            {
                Type t when t == typeof(Clinic) =>
                    _context.Clinics.Where(c => c.HospitalId == hospitalId).Cast<TDept>(),

                Type t when t == typeof(Reception) =>
                    _context.Receptions.Where(c => c.HospitalId == hospitalId).Cast<TDept>(),

                Type t when t == typeof(Pharmacy) =>
                    _context.Pharmacies.Where(c => c.HospitalId == hospitalId).Cast<TDept>(),

                Type t when t == typeof(RadiologyCenter) =>
                    _context.RadiologyCenters.Where(c => c.HospitalId == hospitalId).Cast<TDept>(),

                Type t when t == typeof(MedicalLab) =>
                    _context.MedicalLabs.Where(c => c.HospitalId == hospitalId).Cast<TDept>(),

                _ => Enumerable.Empty<TDept>().AsQueryable()
            };
            return await filteredQuery.GetPagedAsync(pagination);
        }
        catch (Exception ex)
        {
            return new RetrievingDataFromDbContextError(ex);
        }
    }

    public async Task<Result<IEnumerable<Hospital>>> GetHospitalsBySpecificationId(string specializationId)
    {
        return await _context.Specializations.AsNoTracking()
            .Include(s => s.Clinics)
                .ThenInclude(c => c.Hospital)
            .Where(h => h.Clinics.Any(s => s.SpecializationId == specializationId))
            .SelectMany(s => s.Clinics)
            .Select(c => c.Hospital)
            .ToListAsync();
    }

    public async Task<Result<IEnumerable<TDept>>> GetHospitalDepartmentItems<TDept>(string hospitalId) where TDept : class, IHospitalDepartmentItem
    {
        return typeof(TDept) switch
        {
            Type t when t == typeof(Clinic) =>
                await _context.Clinics
                    .Where(c => c.HospitalId == hospitalId)
                    .Include(c => c.CurrentWorkingDoctor)
                    .Include(c => c.Queue)
                    .Cast<TDept>()
                    .ToListAsync(),
            Type t when t == typeof(Reception) =>
                await _context.Receptions
                    .Where(c => c.HospitalId == hospitalId)
                    .Include(c => c.Receptionists)
                    .Cast<TDept>()
                    .ToListAsync(),
            Type t when t == typeof(Pharmacy) =>
                await _context.Pharmacies
                    .Where(c => c.HospitalId == hospitalId)
                    .Cast<TDept>()
                    .ToListAsync(),
            Type t when t == typeof(RadiologyCenter) =>
                await _context.RadiologyCenters
                    .Where(c => c.HospitalId == hospitalId)
                    .Include(t => t.CurrentWorkingTester)
                    .Include(c => c.Tests)
                    .Cast<TDept>()
                    .ToListAsync(),
            Type t when t == typeof(MedicalLab) =>
                await _context.MedicalLabs
                    .Where(c => c.HospitalId == hospitalId)
                    .Include(c => c.Tests)
                    .Include(t => t.CurrentWorkingTester)
                    .Cast<TDept>()
                    .ToListAsync(),
            _ => Result.Fail(new Error(""))
        };
    }

    public async Task<Result<TDept?>> GetHospitalDepartmentItem<TDept>(string departmentId) where TDept : BaseClass, IHospitalDepartmentItem
    {
        return typeof(TDept) switch
        {
            Type t when t == typeof(Clinic) =>
                await _context.Clinics
                    .Where(c => c.Id == departmentId)
                    .Cast<TDept>()
                    .FirstOrDefaultAsync(),
            Type t when t == typeof(Reception) =>
                await _context.Receptions
                    .Where(c => c.Id == departmentId)
                    .Cast<TDept>()
                    .FirstOrDefaultAsync(),
            Type t when t == typeof(Pharmacy) =>
                await _context.Pharmacies
                    .Where(c => c.Id == departmentId)
                    .Cast<TDept>()
                    .FirstOrDefaultAsync(),
            Type t when t == typeof(RadiologyCenter) =>
                await _context.RadiologyCenters
                    .Where(c => c.Id == departmentId)
                    .Cast<TDept>()
                    .FirstOrDefaultAsync(),
            Type t when t == typeof(MedicalLab) =>
                await _context.MedicalLabs
                    .Where(c => c.Id == departmentId)
                    .Cast<TDept>()
                    .FirstOrDefaultAsync(),
            _ => Result.Fail(new Error(""))
        };
    }

    public async Task<Result<List<DateTime>>> GetAvailableTimeSlotsAsync<TDept>(string departmentId, DateTime dateFrom, DateTime dateTo) where TDept : BaseClass, IHospitalDepartmentItem
    {
        dateFrom = dateFrom < DateTime.MinValue.AddHours(1) ?
                DateTime.MinValue.AddHours(1) : dateFrom;
        dateTo = dateTo > DateTime.MaxValue.AddHours(-1) ?
            DateTime.MaxValue.AddHours(-1) : dateTo;
        // 1. Load department & relevant appointments
        var (department, existingAppointments) =
            await LoadDepartmentAndAppointmentsAsync<TDept>(departmentId, dateFrom, dateTo);

        var resultSlots = new List<DateTime>();
        var currentDay = dateFrom.Date;

        while (currentDay <= dateTo.Date)
        {
            var dailyStart = SafeAddTime(currentDay, department.StartAt);
            var dailyEnd = SafeAddTime(currentDay, department.EndAt);

            // Clamp to global dateFrom/dateTo
            dailyStart = dailyStart < dateFrom ? dateFrom : dailyStart;
            dailyEnd = dailyEnd > dateTo ? dateTo : dailyEnd;

            // Generate available slots using safe arithmetic
            var slotCursor = dailyStart;
            while ((dailyEnd - slotCursor) >= department.PeriodPerAppointment)
            {
                var slotEnd = slotCursor + department.PeriodPerAppointment;
                bool isOverlap = existingAppointments.Any(a =>
                {
                    var apptStart = a.Start;
                    var apptEnd = a.Start + a.Duration;
                    return !(slotEnd <= apptStart || slotCursor >= apptEnd);
                });

                if (!isOverlap)
                    resultSlots.Add(slotCursor);

                slotCursor = slotCursor.Add(department.PeriodPerAppointment);
            }

            // Move to next day
            currentDay = SafeAddTime(currentDay, TimeSpan.FromDays(1));
        }

        return resultSlots.Where(d => HelperDate.GetCurrentDate() <= d).ToList();
    }

    public async Task<Result<List<DateTime>>> GetAvailableTimeSlotsAsync<TDept>(string departmentId, DateTime date) where TDept : BaseClass, IHospitalDepartmentItem
    {
        var dates = new List<DateTime>();
        if (typeof(TDept) == typeof(Clinic))
        {
            var clinic = await _context.Clinics
            .FirstOrDefaultAsync(c => c.Id == departmentId);

            if (clinic == null)
                throw new Exception("Clinic not found");

            // Get all appointments for the specified date
            var existingAppointments = await _context.ClinicAppointments
            .Where(a => a.ClinicId == departmentId
                    && a.SchaudleStartAt.Date == date.Date)
                .Select(a => new { a.SchaudleStartAt, a.ExpectedDuration })
            .ToListAsync();

            var baseDate = date.Date; // Use the input date as base
            var currentDateTime = baseDate.Add(clinic.StartAt);
            var endDateTime = baseDate.Add(clinic.EndAt);

            while (currentDateTime.Add(clinic.PeriodPerAppointment) <= endDateTime)
            {
                var slotEndTime = currentDateTime.Add(clinic.PeriodPerAppointment);

                // Check if the time slot overlaps with any existing appointment
                var isSlotAvailable = !existingAppointments.Any(a =>
                    (a.SchaudleStartAt <= currentDateTime && a.SchaudleStartAt.Add(a.ExpectedDuration) > currentDateTime) ||
                    (a.SchaudleStartAt < slotEndTime && a.SchaudleStartAt.Add(a.ExpectedDuration) >= slotEndTime));

                if (isSlotAvailable)
                {
                    dates.Add(currentDateTime);
                }

                currentDateTime = currentDateTime.Add(clinic.PeriodPerAppointment);
            }
        }
        else if (typeof(TDept) == typeof(MedicalLab))
        {
            var medicalLab = await _context.MedicalLabs
            .FirstOrDefaultAsync(c => c.Id == departmentId);

            if (medicalLab == null)
                throw new Exception("Clinic not found");

            // Get all appointments for the specified date
            var existingAppointments = await _context.MedicalLabAppointments
            .Where(a => a.MedicalLabId == departmentId
                    && a.SchaudleStartAt.Date == date.Date)
                .Select(a => new { a.SchaudleStartAt, a.ExpectedDuration })
                .ToListAsync();

            // Generate all possible time slots
            var baseDate = date.Date; // Use the input date as base
            var currentDateTime = baseDate.Add(medicalLab.StartAt);
            var endDateTime = baseDate.Add(medicalLab.EndAt);

            while (currentDateTime.Add(medicalLab.PeriodPerAppointment) <= endDateTime)
            {
                var slotEndTime = currentDateTime.Add(medicalLab.PeriodPerAppointment);

                // Check if the time slot overlaps with any existing appointment
                var isSlotAvailable = !existingAppointments.Any(a =>
                    (a.SchaudleStartAt <= currentDateTime && a.SchaudleStartAt.Add(a.ExpectedDuration) > currentDateTime) ||
                    (a.SchaudleStartAt < slotEndTime && a.SchaudleStartAt.Add(a.ExpectedDuration) >= slotEndTime));

                if (isSlotAvailable)
                {
                    dates.Add(currentDateTime);
                }

                currentDateTime = currentDateTime.Add(medicalLab.PeriodPerAppointment);
            }
        }
        else if (typeof(TDept) == typeof(RadiologyCenter))
        {
            var radiologyCenter = await _context.RadiologyCenters
            .FirstOrDefaultAsync(c => c.Id == departmentId);

            if (radiologyCenter == null)
                throw new Exception("Clinic not found");

            // Get all appointments for the specified date
            var existingAppointments = await _context.RadiologyCeneterAppointments
            .Where(a => a.RadiologyCeneterId == departmentId
                     && a.SchaudleStartAt.Date == date.Date)
                .Select(a => new { a.SchaudleStartAt, a.ExpectedDuration })
                .ToListAsync();

            // Generate all possible time slots
            var baseDate = date.Date; // Use the input date as base
            var currentDateTime = baseDate.Add(radiologyCenter.StartAt);
            var endDateTime = baseDate.Add(radiologyCenter.EndAt);

            while (currentDateTime.Add(radiologyCenter.PeriodPerAppointment) <= endDateTime)
            {
                var slotEndTime = currentDateTime.Add(radiologyCenter.PeriodPerAppointment);

                // Check if the time slot overlaps with any existing appointment
                var isSlotAvailable = !existingAppointments.Any(a =>
                    (a.SchaudleStartAt <= currentDateTime && a.SchaudleStartAt.Add(a.ExpectedDuration) > currentDateTime) ||
                    (a.SchaudleStartAt < slotEndTime && a.SchaudleStartAt.Add(a.ExpectedDuration) >= slotEndTime));

                if (isSlotAvailable)
                {
                    dates.Add(currentDateTime);
                }

                currentDateTime = currentDateTime.Add(radiologyCenter.PeriodPerAppointment);
            }
        }
        return dates.Where(d => HelperDate.GetCurrentDate() <= d).ToList();
    }

    private DateTime SafeAddTime(DateTime date, TimeSpan time)
    {
        try
        {
            return date.Add(time);
        }
        catch (ArgumentOutOfRangeException)
        {
            return time.Ticks > 0 ? DateTime.MaxValue : DateTime.MinValue;
        }
    }

    private async Task<(IHospitalDepartmentItem Dept, List<AppointmentInfo> Appointments)>
                LoadDepartmentAndAppointmentsAsync<TDept>(string departmentId, DateTime dateFrom, DateTime dateTo)
                where TDept : BaseClass, IHospitalDepartmentItem 
    {
        // 1. Load the department
        var department = await _context.Set<TDept>()
            .FirstOrDefaultAsync(d => d.Id == departmentId);

        if (department == null)
            throw new Exception($"{typeof(TDept).Name} not found");

        // 2. Based on TDept, load the relevant appointments
        List<AppointmentInfo> existingAppointments;
        if (typeof(TDept) == typeof(Clinic))
        {
            existingAppointments = await _context.ClinicAppointments
                    .Where(a => a.ClinicId == departmentId
                        && a.SchaudleStartAt  >= dateFrom
                        && a.SchaudleStartAt <= dateTo)
                    .Select(a => new AppointmentInfo
                    {
                        Start = a.SchaudleStartAt,
                        Duration = a.ExpectedDuration
                    })
                    .ToListAsync();
        }
        else if (typeof(TDept) == typeof(MedicalLab))
        {
            existingAppointments = await _context.MedicalLabAppointments
                .Where(a => a.MedicalLabId == departmentId
                            && a.SchaudleStartAt >= dateFrom
                            && a.SchaudleStartAt <= dateTo)
                .Select(a => new AppointmentInfo
                {
                    Start = a.SchaudleStartAt,
                    Duration = a.ExpectedDuration
                })
                    .ToListAsync();
        }
        else if (typeof(TDept) == typeof(RadiologyCenter))
        {
            existingAppointments = await _context.RadiologyCeneterAppointments
                .Where(a => a.RadiologyCeneterId == departmentId
                            && a.SchaudleStartAt >= dateFrom
                            && a.SchaudleStartAt <= dateTo)
                .Select(a => new AppointmentInfo
                {
                    Start = a.SchaudleStartAt,
                    Duration = a.ExpectedDuration
                })
                .ToListAsync();
        }
        else
        {
            throw new NotSupportedException($"{typeof(TDept).Name} is not a supported department type.");
        }

        return (department, existingAppointments);
    }

    public async Task<Result> UpdateHospitalDepartmentItem<TDept>(TDept dept) where TDept : BaseClass, IHospitalDepartmentItem
    {
        _context.Set<TDept>().Update(dept);
        await _context.SaveChangesAsync();
        return Result.Ok();
    }

    public async Task<Result<IEnumerable<Clinic>>> GetAllClinicsBySpecilizationId(string specializationId, string hospitalId)
    {
        return await _context.Clinics
            .Where(c => c.SpecializationId == specializationId && c.HospitalId == hospitalId)
            .Include(c => c.Hospital)
            .Include(c => c.CurrentWorkingDoctor)
            .ToListAsync();
    }
}
public class AppointmentInfo
{
    public DateTime Start { get; set; }
    public TimeSpan Duration { get; set; }
}