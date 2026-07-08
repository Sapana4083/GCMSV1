using GCMS.Models;

namespace GCMS.Services.Interfaces
{
    public interface IRoleMenuMappingService
    {
        Task<List<RoleMenuMapping>> GetAllAsync(int pageNo, int rowCnt);

        Task<RoleMenuMapping?> GetByIdAsync(long id);

        Task<List<RoleMenuMapping>> GetByRoleIdAsync(long roleId);

        Task<RoleMenuMapping?> GetByRoleAndMenuAsync(long roleId, long menuId);

        Task AddAsync(RoleMenuMapping model);

        Task UpdateAsync(RoleMenuMapping model);
    }
}