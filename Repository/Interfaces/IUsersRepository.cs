using GCMS.Models.DTOs;
using GCMS.Models;

namespace GCMS.Repository.Interfaces
{
    public interface IUsersRepository
    {
        Task<Users?> GetUserAsync(string username);
        Task UpdatePasswordHashAsync(long userId, string passwordHash);
        Task<UserDepartmentInfoDTO?> GetDepartmentAndCourtAsync(string username);
    }
}

