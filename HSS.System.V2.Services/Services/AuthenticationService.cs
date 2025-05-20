using FluentResults;

using HSS.System.V2.DataAccess.Contracts;
using HSS.System.V2.Domain.Constants;
using HSS.System.V2.Domain.Enums;
using HSS.System.V2.Domain.Helpers.Models;
using HSS.System.V2.Domain.Models.People;
using HSS.System.V2.Domain.ResultHelpers.Errors;
using HSS.System.V2.Services.Contracts;
using HSS.System.V2.Services.DTOs.AuthDTOs;
using HSS.System.V2.Services.Helpers;
using HSS.System.V2.Domain.Models.Facilities;

using System.Security.Claims;

namespace HSS.System.V2.Services.Services
{
    public class AuthService : IAuthService
    {
        private readonly TokenService _tokenService;
        private readonly IUserContext _userContext;
        private readonly AccountServiceHelper _accountServiceHelper;
        private readonly IPatientRepository _patientRepository;
        private readonly IEmployeeRepository _employeeRepository;
        private readonly IHospitalRepository _hospitalRepository;

        public AuthService(TokenService tokenService, IUserContext userContext,
            AccountServiceHelper accountServiceHelper,
            IPatientRepository patientRepository, IEmployeeRepository employeeRepository,
            IHospitalRepository hospitalRepository)
        {
            _tokenService = tokenService;
            _userContext = userContext;
            _accountServiceHelper = accountServiceHelper;
            _patientRepository = patientRepository;
            _employeeRepository = employeeRepository;
            _hospitalRepository = hospitalRepository;
        }

        public async Task<Result> RegisterPatient(PatientDto dto)
        {
            var check = await _patientRepository.GetPatientByNationalIdOrEmail(dto.NationalId, dto.Email);
            if (check.IsSuccess && check.Value is not null)
                return new BadRequestError("National Id OR Email are already in the system");
            var salt = _accountServiceHelper.CreateSalt();
            var hashPass = _accountServiceHelper.HashPasswordWithSalt(salt, dto.Password);
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
            await _patientRepository.RegisterPatient(patient);
            return Result.Ok();
        }

        public async Task<Result<ConfirmPatientAccountDto>> ConfirmPatientAccount(string patientNationalId)
        {
            var salt = _accountServiceHelper.CreateSalt();
            var pass = _accountServiceHelper.HashPasswordWithSalt(salt, "@Aa123456789");
            var patientResult = await _patientRepository.GetPatientByNationalId(patientNationalId);
            if (patientResult.IsFailed || patientResult.Value is null)
                return new BadRequestError("no patient with this national id");
            var patient = patientResult.Value;
            patient.Salt = salt;
            patient.HashPassword = pass;
            await _patientRepository.UpdatePatient(patient);

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
            var patientResult = await _patientRepository.GetPatientByNationalId(model.NationalId);
            if (patientResult.IsFailed || patientResult.Value is null)
                return new BadRequestError("no patient with this national id");
            var user = patientResult.Value;

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
                new(CustomClaimTypes.Name, user.Name),
            };
            var token = _tokenService.GenerateAccessToken(customClaims);
            return new UserDetails
            {
                TokenModel = token,
                UserId = user.Id,
                UserName = user.Name,
                UserNationalId = user.NationalId,
                UserRole = user.Role.ToString(),
            };
        }

        public async Task<Result<UserDetails>> LoginEmployee(LoginModelDto model, List<Claim>? claims = null)
        {
            var userResult = await _employeeRepository.GetEmployeeByNationalId(model.NationalId);
            if (userResult.IsFailed || userResult.Value is null)
                return EntityNotExistsError.Happen<Employee>(model.NationalId);
            var user = userResult.Value;

            var hashed = _accountServiceHelper.HashPasswordWithSalt(user.Salt, model.Password);
            if (hashed != user.HashPassword)
                return new BadRequestError("National Id or Password Is Not Valid");

            if (user is Doctor doctor)
            {
                var clinicResult = await _hospitalRepository.GetHospitalDepartmentItem<Clinic>(doctor.ClinicId);
                if (clinicResult.IsSuccess && clinicResult.Value != null)
                {
                    clinicResult.Value.CurrentWorkingDoctorId = doctor.Id;
                    await _hospitalRepository.UpdateHospitalDepartmentItem(clinicResult.Value);
                }
            }
            else if (user is RadiologyTester radiologyTester)
            {
                var radiologyCenterResult = await _hospitalRepository.GetHospitalDepartmentItem<RadiologyCenter>(radiologyTester.RadiologyCenterId);
                if (radiologyCenterResult.IsSuccess && radiologyCenterResult.Value != null)
                {
                    radiologyCenterResult.Value.CurrentWorkingTesterId = radiologyTester.Id;
                    await _hospitalRepository.UpdateHospitalDepartmentItem(radiologyCenterResult.Value);
                }
            }
            else if (user is MedicalLabTester medicalLabTester)
            {
                var medicalLabResult = await _hospitalRepository.GetHospitalDepartmentItem<MedicalLab>(medicalLabTester.MedicalLabId);
                if (medicalLabResult.IsSuccess && medicalLabResult.Value != null)
                {
                    medicalLabResult.Value.CurrentWorkingTesterId = medicalLabTester.Id;
                    await _hospitalRepository.UpdateHospitalDepartmentItem(medicalLabResult.Value);
                }
            }

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
                new(CustomClaimTypes.Name, user.Name),
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

            await _employeeRepository.CreateLoginActivity(user, ActivityType.Login);

            return new UserDetails
            {
                TokenModel = token,
                UserId = user.Id,
                UserName = user.Name,
                UserNationalId = user.NationalId,
                UserRole = user.Role.ToString(),
            };
        }

        public async Task<Result> LogoutEmployee()
        {
            // Find the employee by refreshToken.
            var userResult = await _employeeRepository.GetEmployeeById(_userContext.ApiUserId);
            if (userResult.IsFailed || userResult.Value is null)
                return EntityNotExistsError.Happen<Employee>(_userContext.ApiUserId);
            var user = userResult.Value;

            // إزالة الموظف من CurrentWorkingEmployee
            if (user is Doctor doctor)
            {
                var clinicResult = await _hospitalRepository.GetHospitalDepartmentItem<Clinic>(doctor.ClinicId);
                if (clinicResult.IsSuccess && clinicResult.Value != null)
                {
                    clinicResult.Value.CurrentWorkingDoctorId = null;
                    await _hospitalRepository.UpdateHospitalDepartmentItem(clinicResult.Value);
                }
            }
            else if (user is RadiologyTester radiologyTester)
            {
                var radiologyCenterResult = await _hospitalRepository.GetHospitalDepartmentItem<RadiologyCenter>(radiologyTester.RadiologyCenterId);
                if (radiologyCenterResult.IsSuccess && radiologyCenterResult.Value != null)
                {
                    radiologyCenterResult.Value.CurrentWorkingTesterId = null;
                    await _hospitalRepository.UpdateHospitalDepartmentItem(radiologyCenterResult.Value);
                }
            }
            else if (user is MedicalLabTester medicalLabTester)
            {
                var medicalLabResult = await _hospitalRepository.GetHospitalDepartmentItem<MedicalLab>(medicalLabTester.MedicalLabId);
                if (medicalLabResult.IsSuccess && medicalLabResult.Value != null)
                {
                    medicalLabResult.Value.CurrentWorkingTesterId = null;
                    await _hospitalRepository.UpdateHospitalDepartmentItem(medicalLabResult.Value);
                }
            }

            await _employeeRepository.CreateLoginActivity(user, ActivityType.Logout);

            return Result.Ok();
        }

    }

    public class UserDetails
    {
        public string UserName { get; set; }
        public string UserNationalId { get; set; }
        public string UserId { get; set; }
        public string UserRole { get; set; }
        public TokenModel TokenModel { get; set; }
    }
}
