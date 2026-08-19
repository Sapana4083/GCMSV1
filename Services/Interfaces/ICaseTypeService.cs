using GCMS.Models;

namespace GCMS.Services.Interfaces
{
    public interface ICaseTypeService
    {
        Task<List<CaseTypeMaster>> GetAllAsync(int pageNo, int rowCnt);

        Task<CaseTypeMaster?> GetByIdAsync(long id);

        Task AddAsync(CaseTypeMaster model);

        Task UpdateAsync(CaseTypeMaster model);
        Task<List<CaseTypeMaster>> GetCaseTypeAsync(int pageNo, int rowCnt);
    }
}