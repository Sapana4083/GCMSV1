using GCMS.Models;

namespace GCMS.Services.Interfaces
{
    public interface ITehsilService
    {
        Task AddAsync(TehsilMaster model);

        Task UpdateAsync(TehsilMaster model);

        Task<TehsilMaster?> GetByIdAsync(long id);

        Task<List<TehsilMaster>> GetAllAsync(int pageNo, int rowCount);
    }
}
