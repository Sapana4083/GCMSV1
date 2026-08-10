using GCMS.Models;

namespace GCMS.Repository.Interfaces
{
    public interface ICaseTypeRepository
    {
        Task<List<CaseTypeMaster>> GetAllAsync(int pageNo, int rowCnt);

        Task<CaseTypeMaster?> GetByIdAsync(long id);

        Task AddAsync(CaseTypeMaster model);

        Task UpdateAsync(CaseTypeMaster model);
    }
}