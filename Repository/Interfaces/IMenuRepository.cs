using GCMS.Models;

namespace GCMS.Repository.Interfaces
{
    public interface IMenuRepository
    {
        Task<List<MenuMaster>> GetAllAsync(int pageNo, int rowCnt);

        Task<MenuMaster?> GetByIdAsync(long id);

        Task AddAsync(MenuMaster model);

        Task UpdateAsync(MenuMaster model);
    }
}