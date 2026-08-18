using GCMS.Models;
using GCMS.Models.Entities;

namespace GCMS.Services.Interfaces
{
    public interface IBenchTypeService
    {
        Task<List<BenchTypeMaster>> GetAllAsync(int pageNo, int rowCnt);

        Task<BenchTypeMaster?> GetByIdAsync(long id);

        Task AddAsync(BenchTypeMaster model);

        Task UpdateAsync(BenchTypeMaster model);
        Task<List<BenchTypeMaster>> GetBenchDDL(int pageNo, int rowCnt);
    }
}