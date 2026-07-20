using GCMS.Models;

namespace GCMS.Services.Interfaces
{
    public interface IMenuService
    {
        Task<List<MenuMaster>> GetAllAsync(int pageNo, int rowCnt);

        Task<MenuMaster?> GetByIdAsync(long id);

        Task AddAsync(MenuMaster model);

        Task UpdateAsync(MenuMaster model);
    }
}