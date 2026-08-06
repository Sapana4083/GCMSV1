using GCMS.Models;

namespace GCMS.Services.Interfaces
{
    public interface ICasePurposeGroupService
    {
        Task<List<CasePurposeGroupMaster>> GetAllAsync(int pageNo, int rowCnt);

        Task<CasePurposeGroupMaster?> GetByIdAsync(long id);

        Task AddAsync(CasePurposeGroupMaster model);

        Task UpdateAsync(CasePurposeGroupMaster model);

        Task DeleteAsync(long id, string modifiedBy);
    }
}