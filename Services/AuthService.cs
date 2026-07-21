
using GCMS.Services;
using GCMS.Repository.Interfaces;
using GCMS.Services.Interfaces;
using GCMS.Models;
using Microsoft.AspNetCore.Identity;
using System.Text.RegularExpressions;

namespace GCMS.Services
{
    public class AuthService : IAuthService
    {
        private readonly IUsersRepository _repository;
        private readonly IPasswordHasher<Users> _passwordHasher;

        public AuthService(IUsersRepository repository, IPasswordHasher<Users> passwordHasher)
        {
            _repository = repository;
            _passwordHasher = passwordHasher;
        }

        public async Task<bool> ValidateUserAsync(
       string username,
       string password)
        {
            var user = await _repository.GetUserAsync(username);

            if (user == null)
                return false;

            if (string.IsNullOrWhiteSpace(user.Password))
                return false;

            PasswordVerificationResult verificationResult;
            try
            {
                verificationResult = _passwordHasher.VerifyHashedPassword(
                    user,
                    user.Password,
                    password);
            }
            catch (FormatException)
            {
                verificationResult = PasswordVerificationResult.Failed;
            }

            if (verificationResult == PasswordVerificationResult.SuccessRehashNeeded)
            {
                await UpdatePasswordHashAsync(user, password);
                return true;
            }

            if (verificationResult == PasswordVerificationResult.Success)
                return true;

            if (!IsLegacySha256Hash(user.Password))
                return false;

            string legacyPasswordHash =
                LegacyPasswordHasher.ComputeSha256(password);

            if (!string.Equals(
                legacyPasswordHash,
                user.Password,
                StringComparison.OrdinalIgnoreCase))
            {
                return false;
            }

            await UpdatePasswordHashAsync(user, password);
            return true;
        }
        private async Task UpdatePasswordHashAsync(Users user, string password)
        {
            string passwordHash = _passwordHasher.HashPassword(user, password);
            await _repository.UpdatePasswordHashAsync(user.AxUsersId, passwordHash);
        }

        private static bool IsLegacySha256Hash(string passwordHash)
        {
            return Regex.IsMatch(passwordHash, "^[a-fA-F0-9]{64}$");
        }
    }
}
