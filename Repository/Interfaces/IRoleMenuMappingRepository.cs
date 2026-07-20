using GCMS.Models;

namespace GCMS.Repository.Interfaces
{
    public interface IRoleMenuMappingRepository
    {
        Task<List<RoleMenuMapping>> GetAllAsync(int pageNo, int rowCnt);

        Task<RoleMenuMapping?> GetByIdAsync(long id);

        Task AddAsync(RoleMenuMapping model);

        Task UpdateAsync(RoleMenuMapping model);

        Task<List<RoleMenuMapping>> GetByRoleIdAsync(long roleId);

        Task<RoleMenuMapping?> GetByRoleAndMenuAsync(long roleId, long menuId);
    }
}