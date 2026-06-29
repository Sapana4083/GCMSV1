using GCMS.Models;

namespace GCMS.Repository.Interfaces
{
    public interface IStateRepository
    {
        Task<List<StateMaster>> GetAllAsync(int pageNo, int rowCnt);

        Task<StateMaster?> GetByIdAsync(long id);

        Task AddAsync(StateMaster state);

        Task UpdateAsync(StateMaster state);

        Task DeleteAsync(long id);
    }
}
