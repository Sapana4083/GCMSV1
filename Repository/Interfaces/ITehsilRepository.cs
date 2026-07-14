using GCMS.Models;

namespace GCMS.Repository.Interfaces
{
    public interface ITehsilRepository
    {
        Task AddAsync(TehsilMaster model);

        Task UpdateAsync(TehsilMaster model);

        Task<TehsilMaster?> GetByIdAsync(long id);

        Task<List<TehsilMaster>> GetAllAsync(int pageNo, int rowCount);
    }
}
