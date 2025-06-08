using FluentResults;

using HSS.System.V2.Domain.Helpers.Models;
using HSS.System.V2.Services.DTOs;
using HSS.System.V2.Services.DTOs.AuthDTOs;

using System.Security.Claims;

namespace HSS.System.V2.Services.Contracts
{
    public interface IAuthService
    {
        Task<Result> RegisterPatient(PatientDto dto);
        Task<Result<UserDetails>> LoginPatient(LoginModelDto model);
        Task<Result<UserDetails>> LoginEmployee(LoginModelDto model, List<Claim>? claims = null);
        Task<Result> LogoutEmployee();
        Task<Result<ConfirmPatientAccountDto>> ConfirmPatientAccount(string patientNationalId);
    }
}
