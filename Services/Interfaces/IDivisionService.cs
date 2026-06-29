using GCMS.Models;

namespace GCMS.Services.Interfaces
{
    public interface IDivisionService
    {
        Task<List<DivisionMaster>> GetAllAsync(
    int pageNo,
    int rowCnt);

        Task<DivisionMaster?> GetByIdAsync(long id);

        Task AddAsync(DivisionMaster model);

        Task UpdateAsync(DivisionMaster model);

        Task DeleteAsync(long id);
    }
}
