using GCMS.Models;

namespace GCMS.Services.Interfaces
{
    public interface IDesignationService
    {
        Task<List<DesignationMaster>> GetAllAsync(int pageNo, int rowCnt);

        Task<DesignationMaster?> GetByIdAsync(long id);

        Task AddAsync(DesignationMaster model);

        Task UpdateAsync(DesignationMaster model);
    }
}