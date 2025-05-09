using FluentResults;

using HSS.System.V2.Domain.Helpers.Models;
using HSS.System.V2.Services.DTOs.AuthDTOs;

using System.Security.Claims;

namespace HSS.System.V2.Services.Contracts
{
    public interface IAuthService
    {
        Task<Result> RegisterPatient(PatientDto dto);
        Task<Result<TokenModel>> LoginPatient(LoginModelDto model);
        Task<Result<TokenModel>> LoginEmployee(LoginModelDto model, List<Claim>? claims = null);
        Task<Result<TokenModel>> RefreshTheTokenForPatient(string refreshToken, string accessToken);
        Task<Result<TokenModel>> RefreshTheTokenForEmployee(string refreshToken, string accessToken);
        Task<Result> LogoutPatient(string refreshToken);
        Task<Result> LogoutEmployee(string refreshToken);
    }
}
