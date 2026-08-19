using GCMS.Models;

namespace GCMS.Repository.Interfaces
{
    public interface IDesignationRepository
    {
        Task<List<DesignationMaster>> GetAllAsync(int pageNo, int rowCnt);

        Task<DesignationMaster?> GetByIdAsync(long id);

        Task AddAsync(DesignationMaster model);

        Task UpdateAsync(DesignationMaster model);
    }
}