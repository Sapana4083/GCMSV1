using GCMS.Models;

namespace GCMS.Repository.Interfaces
{
    public interface IBenchTypeRepository
    {
        Task<List<BenchTypeMaster>> GetAllAsync(int pageNo, int rowCnt);

        Task<BenchTypeMaster?> GetByIdAsync(long id);

        Task AddAsync(BenchTypeMaster model);

        Task UpdateAsync(BenchTypeMaster model);
    }
}