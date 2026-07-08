using GCMS.Models;

namespace GCMS.Services.Interfaces
{
    public interface IRoleService
    {
        Task<List<RoleMaster>> GetAllAsync(int pageNo, int rowCnt);

        Task<RoleMaster?> GetByIdAsync(long id);

        Task AddAsync(RoleMaster model);

        Task UpdateAsync(RoleMaster model);
    }
}
