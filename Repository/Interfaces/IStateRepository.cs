using GCMS.WEB.Models;

namespace GCMS.Web.Repository.Interfaces
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
