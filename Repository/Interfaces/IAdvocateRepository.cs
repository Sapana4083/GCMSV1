using GCMS.Models;

namespace GCMS.Repository.Interfaces
{
    public interface IAdvocateRepository
    {
        Task<List<AdvocateMaster>> GetAllAsync(int pageNo, int rowCnt);

        Task<AdvocateMaster?> GetByIdAsync(long id);

        Task AddAsync(AdvocateMaster model);

        Task UpdateAsync(AdvocateMaster model);
    }
}