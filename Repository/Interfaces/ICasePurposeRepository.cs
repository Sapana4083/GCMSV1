using GCMS.Models;

namespace GCMS.Repository.Interfaces
{
    public interface ICasePurposeRepository
    {
        Task<List<CasePurposeMaster>> GetAllAsync(int pageNo, int rowCnt);

        Task<CasePurposeMaster?> GetByIdAsync(long id);

        Task AddAsync(CasePurposeMaster model);

        Task UpdateAsync(CasePurposeMaster model);
    }
}
