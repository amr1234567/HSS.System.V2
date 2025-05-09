using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HSS.System.V2.Domain.Helpers.Models
{
    public class JwtHelper
    {
        public string JwtKey { get; set; }
        public string JwtIssuer { get; set; }
        public string JwtAudience { get; set; }
        public int JwtExpireMinutes { get; set; }
        public int RefreshTokenExpireDays { get; set; }

        public override string ToString()
        {
            return $"Expire minutes for short token: {JwtExpireMinutes}, Expire days for long token: {RefreshTokenExpireDays}, key: {JwtKey}";
        }
    }
}
