using GCMS.Models;
using GCMS.Repository.Interfaces;
using GCMS.Services.Interfaces;

namespace GCMS.Services
{
    public class RoleMenuMappingService : IRoleMenuMappingService
    {
        private readonly IRoleMenuMappingRepository _repository;

        public RoleMenuMappingService(IRoleMenuMappingRepository repository)
        {
            _repository = repository;
        }

        public async Task<List<RoleMenuMapping>> GetAllAsync(int pageNo, int rowCnt)
        {
            return await _repository.GetAllAsync(pageNo, rowCnt);
        }

        public async Task<RoleMenuMapping?> GetByIdAsync(long id)
        {
            return await _repository.GetByIdAsync(id);
        }

        public async Task<List<RoleMenuMapping>> GetByRoleIdAsync(long roleId)
        {
            return await _repository.GetByRoleIdAsync(roleId);
        }

        public async Task AddAsync(RoleMenuMapping model)
        {
            await _repository.AddAsync(model);
        }

        public async Task UpdateAsync(RoleMenuMapping model)
        {
            await _repository.UpdateAsync(model);
        }

        public async Task<RoleMenuMapping?> GetByRoleAndMenuAsync(long roleId, long menuId)
        {
            return await _repository.GetByRoleAndMenuAsync(roleId, menuId);
        }
    }
}