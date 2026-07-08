using GCMS.Models;
using GCMS.Repository.Interfaces;
using GCMS.Services.Interfaces;

namespace GCMS.Services
{
    public class RoleService : IRoleService
    {
        private readonly IRoleRepository _repository;

        public RoleService(IRoleRepository repository)
        {
            _repository = repository;
        }

        public async Task<List<RoleMaster>> GetAllAsync(int pageNo, int rowCnt)
        {
            return await _repository.GetAllAsync(pageNo, rowCnt);
        }

        public async Task<RoleMaster?> GetByIdAsync(long id)
        {
            return await _repository.GetByIdAsync(id);
        }

        public async Task AddAsync(RoleMaster model)
        {
            await _repository.AddAsync(model);
        }

        public async Task UpdateAsync(RoleMaster model)
        {
            await _repository.UpdateAsync(model);
        }
    }
}
