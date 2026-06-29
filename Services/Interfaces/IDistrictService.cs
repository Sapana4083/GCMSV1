using GCMS.Models;

namespace GCMS.Services.Interfaces
{
    public interface IDistrictService
    {
        Task<List<DistrictMaster>> GetAllAsync(
            int pageNo,
            int rowCount);

        Task<DistrictMaster?> GetByIdAsync(
            long id);

        Task AddAsync(
            DistrictMaster model);

        Task UpdateAsync(
            DistrictMaster model);

        Task DeleteAsync(
            long id);
    }
}