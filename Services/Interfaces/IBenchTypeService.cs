using GCMS.Models;

namespace GCMS.Services.Interfaces
{
    public interface IBenchTypeService
    {
        Task<List<BenchTypeMaster>> GetAllAsync(int pageNo, int rowCnt);

        Task<BenchTypeMaster?> GetByIdAsync(long id);

        Task AddAsync(BenchTypeMaster model);

        Task UpdateAsync(BenchTypeMaster model);
    }
}