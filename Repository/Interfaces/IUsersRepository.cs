using GCMS.Web.Models.DTOs;
using GCMS.WEB.Models;

namespace GCMS.WEB.Repository.Interfaces
{
    public interface IUsersRepository
    {
        Task<Users?> GetUserAsync(string username);
        Task<UserDepartmentInfoDTO?> GetDepartmentAndCourtAsync(string username);
    }
}

