using GCMS.Models;

namespace GCMS.Services.Interfaces
{
    public interface ICourtTypeService
    {
        Task<List<CourtTypeMaster>> GetAllAsync(int pageNo, int rowCnt);

      

        Task<CourtTypeMaster?> GetByIdAsync(long id);

        Task AddAsync(CourtTypeMaster model);

        Task UpdateAsync(CourtTypeMaster model);

        Task<List<LovModel>> GetCourtCategoryAsync();
    }
}