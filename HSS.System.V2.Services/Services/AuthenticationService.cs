using FluentResults;

using HSS.System.V2.DataAccess.Contexts;
using HSS.System.V2.Domain.Constants;
using HSS.System.V2.Domain.Enums;
using HSS.System.V2.Domain.Helpers.Models;
using HSS.System.V2.Domain.Models.Common;
using HSS.System.V2.Domain.Models.People;
using HSS.System.V2.Domain.ResultHelpers.Errors;
using HSS.System.V2.Services.Contracts;
using HSS.System.V2.Services.DTOs.AuthDTOs;
using HSS.System.V2.Services.Helpers;

using Microsoft.EntityFrameworkCore;

using System.Security.Claims;

namespace HSS.System.V2.Services.Services
{
    public class AuthService : IAuthService
    {
        private readonly TokenService _tokenService;
        private readonly IUserContext _userContext;
        private readonly AccountServiceHelper _accountServiceHelper;
        private readonly AppDbContext _context;
        public AuthService(TokenService tokenService, IUserContext userContext, AccountServiceHelper accountServiceHelper, AppDbContext context)
        {
            _tokenService = tokenService;
            _userContext = userContext;
            _accountServiceHelper = accountServiceHelper;
            _context = context;
        }

        public async Task<Result> RegisterPatient(PatientDto dto)
        {
            var check = await _context.Patients.FirstOrDefaultAsync(p => p.NationalId == dto.NationalId || p.Email == dto.Email);
            if (check is not null)
                return new BadRequestError("National Id OR Email are already in the system");
            var salt = _accountServiceHelper.CreateSalt();
            var hashPass = _accountServiceHelper.HashPasswordWithSalt(salt, dto.Passowrd);
            var patient = new Patient
            {
                Id = Guid.NewGuid().ToString(),
                Name = dto.Name,
                Gender = dto.Gender,
                Role = UserRole.Patient,
                NationalId = dto.NationalId,
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now,
                Email = dto.Email,
                Salt = salt,
                HashPassword = hashPass
            };

            await _context.Patients.AddAsync(patient);
            await _context.SaveChangesAsync();
            return Result.Ok();
        }

        public async Task<Result<ConfirmPatientAccountDto>> ConfirmPatientAccount(string patientNationalId)
        {
            var salt = _accountServiceHelper.CreateSalt();
            var pass = _accountServiceHelper.HashPasswordWithSalt(salt, "@Aa123456789");
            var patient = await _context.Patients.FirstOrDefaultAsync(p => p.NationalId == patientNationalId);
            if (patient is null)
                return new BadRequestError("no patient with this national id");
            patient.Salt = salt;
            patient.HashPassword = pass;
            await _context.SaveChangesAsync();
            var customClaims = new List<Claim>
            {
                new(CustomClaimTypes.NationalId, patient.NationalId),
                new(ClaimTypes.NameIdentifier, patient.Id),
                new(ClaimTypes.Name, patient.Name),
                new(CustomClaimTypes.Gender, patient.Gender.ToString()),
                new(ClaimTypes.MobilePhone, patient.PhoneNumber),
                new(CustomClaimTypes.CustomRole, patient.Role.ToString()),
                new(ClaimTypes.Role, patient.Role.ToString()),
                new(CustomClaimTypes.Name, patient.Name)
            };
            var token = _tokenService.GenerateAccessToken(customClaims);
            return new ConfirmPatientAccountDto()
            {
                NationalId = patientNationalId,
                Password = "@Aa123456789",
                Token = token
            };
        }

        public async Task<Result<UserDetails>> LoginPatient(LoginModelDto model)
        {
            var user = await _context.Patients.FirstOrDefaultAsync(x => x.NationalId == model.NationalId);
            if (user == null)
                return EntityNotExistsError.Happen<Patient>(model.NationalId);

            if (string.IsNullOrEmpty(user.HashPassword))
                return new BadRequestError("patient must confirm his account first");

            var hashed = _accountServiceHelper.HashPasswordWithSalt(user.Salt, model.Password);
            if (hashed != user.HashPassword)
                return new BadRequestError("National Id or Password Is Not Valid");

            var customClaims = new List<Claim>
            {
                new(CustomClaimTypes.NationalId, user.NationalId),
                new(ClaimTypes.NameIdentifier, user.Id),
                new(ClaimTypes.Name, user.Name),
                new(CustomClaimTypes.Gender, user.Gender.ToString()),
                new(CustomClaimTypes.CustomRole, user.Role.ToString()),
                new(ClaimTypes.Role, user.Role.ToString()),
                new(CustomClaimTypes.Name, user.Name)
            };
            var token = _tokenService.GenerateAccessToken(customClaims);
            return new UserDetails
            {
                TokenModel = token,
                UserId = user.Id,
                UserName = user.Name,
            };
        }

        public async Task<Result<TokenModel>> LoginEmployee(LoginModelDto model, List<Claim>? claims = null)
        {
            var user = await _context.Employees.FirstOrDefaultAsync(x => x.NationalId == model.NationalId);
            if (user == null)
                return EntityNotExistsError.Happen<Employee>(model.NationalId);

            var hashed = _accountServiceHelper.HashPasswordWithSalt(user.Salt, model.Password);
            if (hashed != user.HashPassword)
                return new BadRequestError("National Id or Password Is Not Valid");

            var customClaims = new List<Claim>
            {
                new(CustomClaimTypes.NationalId, user.NationalId),
                new(ClaimTypes.NameIdentifier, user.Id),
                new(ClaimTypes.Name, user.Name),
                new(CustomClaimTypes.Gender, user.Gender.ToString()),
                new(ClaimTypes.MobilePhone, user.PhoneNumber),
                new(ClaimTypes.Role, user.Role.ToString()),
                new(CustomClaimTypes.HospitalId, user.HospitalId),
                new(CustomClaimTypes.CustomRole, user.Role.ToString()),
                new(CustomClaimTypes.Name, user.Name)
            };

            if (claims != null)
                customClaims.AddRange(claims);

            if (user is Doctor dd)
                customClaims.Add(new(CustomClaimTypes.DepartmentItemId, dd.ClinicId));
            else if (user is Receptionist r)
                customClaims.Add(new(CustomClaimTypes.DepartmentItemId, r.ReceptionId));
            else if (user is Pharmacist p)
                customClaims.Add(new(CustomClaimTypes.DepartmentItemId, p.PharmacyId));
            else if (user is RadiologyTester rt)
                customClaims.Add(new(CustomClaimTypes.DepartmentItemId, rt.RadiologyCenterId));
            else if (user is MedicalLabTester l)
                customClaims.Add(new(CustomClaimTypes.DepartmentItemId, l.MedicalLabId));

            var token = _tokenService.GenerateAccessToken(customClaims);

            await _context.LoginActivities.AddAsync(new()
            {
                ActivityType = ActivityType.Login,
                CreatedAt = DateTime.Now,
                EmployeeId = user.Id,
                Id = Guid.NewGuid().ToString(),
                UpdatedAt = DateTime.Now,
            });

            await _context.SaveChangesAsync();
            return token;
        }

        public async Task<Result> LogoutEmployee()
        {
            // Find the employee by refreshToken.
            var employee = await _context.Employees.FirstOrDefaultAsync(e => e.Id == _userContext.ApiUserId);
            if (employee == null)
                return EntityNotExistsError.Happen<Employee>();

            await _context.LoginActivities.AddAsync(new LoginActivity
            {
                ActivityType = ActivityType.Logout,
                CreatedAt = DateTime.Now,
                EmployeeId = _userContext.ApiUserId,
                Id = Guid.NewGuid().ToString(),
                UpdatedAt = DateTime.Now,
            });

            await _context.SaveChangesAsync();
            return Result.Ok();
        }

    }

    public class UserDetails
    {
        public string UserName { get; set; }
        public string UserId { get; set; }
        public TokenModel TokenModel { get; set; }
    }
}
