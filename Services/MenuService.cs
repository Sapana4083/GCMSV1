using GCMS.Models;
using GCMS.Repository.Interfaces;
using GCMS.Services.Interfaces;

namespace GCMS.Services
{
    public class MenuService : IMenuService
    {
        private readonly IMenuRepository _repository;

        public MenuService(IMenuRepository repository)
        {
            _repository = repository;
        }

        public async Task<List<MenuMaster>> GetAllAsync(int pageNo, int rowCnt)
        {
            return await _repository.GetAllAsync(pageNo, rowCnt);
        }

        public async Task<MenuMaster?> GetByIdAsync(long id)
        {
            return await _repository.GetByIdAsync(id);
        }

        public async Task AddAsync(MenuMaster model)
        {
            await _repository.AddAsync(model);
        }

        public async Task UpdateAsync(MenuMaster model)
        {
            await _repository.UpdateAsync(model);
        }
    }
}