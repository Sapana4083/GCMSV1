
using GCMS.Web.Services;
using GCMS.WEB.Repository.Interfaces;
using GCMS.WEB.Services.Interfaces;

namespace GCMS.WEB.Services
{
    public class AuthService : IAuthService
    {
        private readonly IUsersRepository _repository;

        public AuthService(IUsersRepository repository)
        {
            _repository = repository;
        }

        public async Task<bool> ValidateUserAsync(
       string username,
       string password)
        {
            var user = await _repository.GetUserAsync(username);

            if (user == null)
                return false;

            string passwordHash =
                PasswordHasher.ComputeSha256(password);

            return string.Equals(
                passwordHash,
                user.Password,
                StringComparison.OrdinalIgnoreCase);
        }
    }
}
