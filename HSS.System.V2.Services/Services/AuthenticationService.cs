using FluentResults;

using HSS.System.V2.DataAccess.Contexts;
using HSS.System.V2.Domain.Common;
using HSS.System.V2.Domain.Constants;
using HSS.System.V2.Domain.Enums;
using HSS.System.V2.Domain.Helpers.Models;
using HSS.System.V2.Domain.People;
using HSS.System.V2.Domain.ResultHelpers.Errors;
using HSS.System.V2.Services.Contracts;
using HSS.System.V2.Services.DTOs.AuthDTOs;

using Microsoft.EntityFrameworkCore;

using System.Security.Claims;

namespace HSS.System.V2.Services.Services
{
    public class AuthenticationService : IAuthService
    {
        private readonly TokenService _tokenService;
        private readonly AccountServiceHelper _accountServiceHelper;
        private readonly AppDbContext _context;
        public AuthenticationService(TokenService tokenService, AccountServiceHelper accountServiceHelper, AppDbContext context)
        {
            _tokenService = tokenService;
            _accountServiceHelper = accountServiceHelper;
            _context = context;
        }

        public async Task<Result> RegisterPatient(PatientDto dto)
        {
            var patient = new Patient
            {
                Name = dto.Name,
                Address = dto.Address,
                BirthOfDate = dto.DateOfBirth,
                Gender = dto.Gender,
                Id = Guid.NewGuid().ToString(),
                Salt = _accountServiceHelper.CreateSalt(),
                HashPassword = null,
                PhoneNumber = dto.PhoneNumber,
                Role = UserRole.Patient,
                NationalId = dto.NationalId,
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now,
            };
            await _context.Patients.AddAsync(patient);
            await _context.SaveChangesAsync();
            return Result.Ok();
        }
        public async Task<Result<TokenModel>> LoginPatient(LoginModelDto model)
        {
            var user = await _context.Patients.FirstOrDefaultAsync(x => x.NationalId == model.NationalId);
            if (user == null)
                return EntityNotExistsError.Happen<Patient>(model.NationalId);

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
                new(CustomClaimTypes.CustomRole, user.Role.ToString()),
                new(ClaimTypes.Role, user.Role.ToString()),
                new(CustomClaimTypes.Name, user.Name)
            };

            var token = _tokenService.GenerateAccessToken(customClaims);
            var refreshToken = _tokenService.GenerateRefreshToken();
            user.RefreshToken = refreshToken;
            user.RefreshTokenExpirationDate = token.RefreshTokenExpirationTime;

            _context.Patients.Update(user);

            await _context.SaveChangesAsync();
            return token;
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

            user.RefreshToken = token.RefreshToken;
            user.RefreshTokenExpirationDate = token.RefreshTokenExpirationTime;

            _context.Employees.Update(user);

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

        public async Task<Result<TokenModel>> RefreshTheTokenForPatient(string refreshToken, string accessToken)
        {
            var user = await _context.Patients.FirstOrDefaultAsync(p => p.RefreshToken == refreshToken);
            if (user == null)
                return EntityNotExistsError.Happen<Patient>();

            if(user.RefreshTokenExpirationDate < DateTime.Now)
                return new BadRequestError("Refresh Token Is Expired");
            var principles = _tokenService.GetPrincipalFromExpiredToken(accessToken);

            var newToken = _tokenService.GenerateAccessToken(principles);
            newToken.RefreshToken = refreshToken;
            newToken.RefreshTokenExpirationTime = user.RefreshTokenExpirationDate ?? DateTime.MinValue;
            
            _context.Patients.Update(user);
            await _context.SaveChangesAsync();

            return newToken;
        }

        public async Task<Result<TokenModel>> RefreshTheTokenForEmployee(string refreshToken, string accessToken)
        {
            var user = await _context.Employees.FirstOrDefaultAsync(p => p.RefreshToken == refreshToken);
            if (user == null)
                return EntityNotExistsError.Happen<Employee>();

            if (user.RefreshTokenExpirationDate < DateTime.Now)
                return new BadRequestError("Refresh Token Is Expired");
            var principles = _tokenService.GetPrincipalFromExpiredToken(accessToken);

            var newToken = _tokenService.GenerateAccessToken(principles);
            newToken.RefreshToken = refreshToken;
            newToken.RefreshTokenExpirationTime = user.RefreshTokenExpirationDate ?? DateTime.MinValue;

            _context.Employees.Update(user);
            await _context.SaveChangesAsync();

            return newToken;
        }

        public async Task<Result> LogoutPatient(string refreshToken)
        {
            // Find the patient by refreshToken.
            var patient = await _context.Patients.FirstOrDefaultAsync(p => p.RefreshToken == refreshToken);
            if (patient == null)
            {
                return EntityNotExistsError.Happen<Patient>();
            }

            // Clear the refresh token values.
            patient.RefreshToken = null;
            patient.RefreshTokenExpirationDate = null;
            _context.Patients.Update(patient);

            await _context.SaveChangesAsync();
            return Result.Ok();
        }

        public async Task<Result> LogoutEmployee(string refreshToken)
        {
            // Find the employee by refreshToken.
            var employee = await _context.Employees.FirstOrDefaultAsync(e => e.RefreshToken == refreshToken);
            if (employee == null)
            {
                return EntityNotExistsError.Happen<Employee>();
            }

            // Clear the refresh token values.
            employee.RefreshToken = null;
            employee.RefreshTokenExpirationDate = null;
            _context.Employees.Update(employee);

            // Record the logout activity.
            await _context.LoginActivities.AddAsync(new LoginActivity
            {
                ActivityType = ActivityType.Logout,
                CreatedAt = DateTime.Now,
                EmployeeId = employee.Id,
                Id = Guid.NewGuid().ToString(),
                UpdatedAt = DateTime.Now,
            });

            await _context.SaveChangesAsync();
            return Result.Ok();
        }

    }
}
