using System.Security.Cryptography;
using System.Text;

using Microsoft.AspNetCore.Cryptography.KeyDerivation;
namespace HSS.System.V2.Services.Helpers
{
    public partial class AccountServiceHelper
    {
        public string HashPasswordWithSalt(string salt, string password)
        {
            byte[] saltBytes = Convert.FromBase64String(salt);

            // Derive a 256-bit subkey (use HMACSHA256 with 100,000 iterations)
            var hashed = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                password: password,
                salt: saltBytes,
                prf: KeyDerivationPrf.HMACSHA256,
                iterationCount: 100000,
                numBytesRequested: 256 / 8));

            return hashed;
        }

        public string CreateVerificationCode(int length)
        {
            string Characters = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            if (length <= 0)
                throw new ArgumentException("Length must be a positive integer.");

            Random random = new();
            StringBuilder verificationCode = new(length);

            for (int i = 0; i < length; i++)
            {
                int index = random.Next(Characters.Length);
                verificationCode.Append(Characters[index]);
            }

            return verificationCode.ToString();
        }

        public string CreateSalt()
        {
            byte[] saltBytes = new byte[128 / 8];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(saltBytes);
            }
            return Convert.ToBase64String(saltBytes);
        }

        public string HashString(string input)
        {
            byte[] bytes = SHA256.HashData(Encoding.UTF8.GetBytes(input));

            StringBuilder builder = new();
            foreach (byte b in bytes)
            {
                builder.Append(b.ToString("x2"));
            }

            return builder.ToString();
        }

    }
}
