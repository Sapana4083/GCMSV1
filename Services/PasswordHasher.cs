using System.Security.Cryptography;
using System.Text;

namespace GCMS.Web.Services
{
    public static class PasswordHasher
    {
        public static string ComputeSha256(string input)
        {
            using var sha256 = SHA256.Create();

            byte[] hashBytes = sha256.ComputeHash(
                Encoding.UTF8.GetBytes(input));

            return Convert.ToHexString(hashBytes)
                .ToLowerInvariant();
        }
    }
}
