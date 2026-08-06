using GCMS.Models;

namespace GCMS.Repository.Interfaces
{
    public interface ICourtTypeRepository
    {
        Task<List<CourtTypeMaster>> GetAllAsync(int pageNo, int rowCnt);

        Task<List<CaseTypeMaster>> GetCaseTypeAsync(int pageNo, int rowCnt);

        Task<CourtTypeMaster?> GetByIdAsync(long id);

        Task AddAsync(CourtTypeMaster model);

        Task UpdateAsync(CourtTypeMaster model);

        Task<List<LovModel>> GetCourtCategoryAsync();
    }
}