
using GCMS.Services;
using GCMS.Repository.Interfaces;
using GCMS.Services.Interfaces;
using GCMS.Models;
using Microsoft.AspNetCore.Identity;
using System.Text.RegularExpressions;
using GCMS.Repository;

namespace GCMS.Services
{
    public class AuthService : IAuthService
    {
        private readonly IUserRepository _repository;
        //private readonly IPasswordHasher<Users> _passwordHasher;

        public AuthService(IUserRepository repository)
        {
            _repository = repository;

        }

        public async Task<bool> ValidateUserAsync(string username, string password)
        {
            var user = await _repository.GetUserAsync(username);

            if (user == null)
                return false;

            if (string.IsNullOrWhiteSpace(user.Password))
                return false;

            string passwordHash = LegacyPasswordHasher.ComputeSha256(password);

            return string.Equals(
                passwordHash,
                user.Password,
                StringComparison.OrdinalIgnoreCase);
        }
      

    }
}
