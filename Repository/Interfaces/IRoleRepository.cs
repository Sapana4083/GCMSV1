using GCMS.Models;

namespace GCMS.Repository.Interfaces
{
    public interface IRoleRepository 
    {
        Task<List<RoleMaster>> GetAllAsync(int pageNo, int rowCnt);

        Task<RoleMaster?> GetByIdAsync(long id);

        Task AddAsync(RoleMaster model);

        Task UpdateAsync(RoleMaster model);
    }
}
