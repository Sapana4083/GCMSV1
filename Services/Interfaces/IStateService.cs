using GCMS.Models;

namespace GCMS.Services.Interfaces
{
    public interface IStateService
    {
        Task<List<StateMaster>> GetAllAsync(int pageNo, int rowCnt);

        Task<StateMaster?> GetByIdAsync(long id);

        Task AddAsync(StateMaster state);

        Task UpdateAsync(StateMaster state);

        Task DeleteAsync(long id);
    }
}
