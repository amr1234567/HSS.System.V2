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

    public Task<Result> UpdateHospitalItem(Hospital hospital)
    {
        throw new NotImplementedException();
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
                    .Include(c => c.CurrentWorkingDoctor)
                    .Include(c => c.Queue)
                    .Include(c => c.Hospital)
                    .Cast<TDept>()
                    .FirstOrDefaultAsync(),
            Type t when t == typeof(Reception) =>
                await _context.Receptions
                    .Where(c => c.Id == departmentId)
                    .Include(c => c.Receptionists)
                    .Include(c => c.Hospital)
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
                    .Include(t => t.CurrentWorkingTester)
                    .Include(c => c.Tests)
                    .Include(c => c.Hospital)
                    .Cast<TDept>()
                    .FirstOrDefaultAsync(),
            Type t when t == typeof(MedicalLab) =>
                await _context.MedicalLabs
                    .Where(c => c.Id == departmentId)
                    .Include(c => c.Tests)
                    .Include(t => t.CurrentWorkingTester)
                    .Include(c => c.Hospital)
                    .Cast<TDept>()
                    .FirstOrDefaultAsync(),
            _ => Result.Fail(new Error(""))
        };
    }
}