using GCMS.Models;

namespace GCMS.Services.Interfaces
{
    public interface ICasePurposeService
    {
        Task<List<CasePurposeMaster>> GetAllAsync(int pageNo, int rowCnt);
        Task<CasePurposeMaster?> GetByIdAsync(long id);
        Task AddAsync(CasePurposeMaster model);
        Task UpdateAsync(CasePurposeMaster model);
    }
}