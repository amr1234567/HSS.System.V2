using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

using HSS.System.V2.Domain.Helpers.Methods;
using HSS.System.V2.Domain.Helpers.Models;

using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace HSS.System.V2.Services.Helpers
{
    public class TokenService(IOptions<JwtHelper> options)
    {
        private readonly JwtHelper _jwtHelper = options.Value;

        public TokenModel GenerateAccessToken(List<Claim>? customClaims = null)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtHelper.JwtKey));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            List<Claim> claims = [new(ClaimTypes.Rsa, Guid.NewGuid().ToString())];

            if (customClaims != null)
                claims.AddRange(customClaims);

            var expirationDate = DateTime.UtcNow.AddMinutes(_jwtHelper.JwtExpireMinutes);

            var token = new JwtSecurityToken(
                claims: claims,
                expires: expirationDate,
                signingCredentials: credentials
            );

            return new TokenModel
            {
                Token = new JwtSecurityTokenHandler().WriteToken(token),
                TokenExpirationDate = expirationDate
            };
        }

        public TokenModel GenerateAccessToken(ClaimsPrincipal principal)
        {
            // Extract existing claims from the provided ClaimsPrincipal.
            // You might want to filter out claims that you don't want to include.
            var claims = principal.Claims.ToList();

            // Optionally add a new claim (for example, a unique identifier)
            claims.Add(new Claim(ClaimTypes.Rsa, Guid.NewGuid().ToString()));

            // Create the security key and signing credentials.
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtHelper.JwtKey));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            // Set the token's expiration time.
            var expirationDate = DateGenerator.GetCurrentDateTimeByRegion(Region.Egypt).AddMinutes(_jwtHelper.JwtExpireMinutes);

            // Create the JWT token.
            var token = new JwtSecurityToken(
                claims: claims,
                expires: expirationDate,
                signingCredentials: credentials
            );

            // Generate and return the TokenModel.
            return new TokenModel
            {
                Token = new JwtSecurityTokenHandler().WriteToken(token),
                TokenExpirationDate = expirationDate
            };
        }
    }
}
