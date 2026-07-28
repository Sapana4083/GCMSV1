using GCMS.Models.Entities;

namespace GCMS.Services.Interfaces
{
    public interface IDepartmentService
    {
        Task<List<DepartmentMaster>> GetAllAsync(int pageNo, int rowCnt);
        Task<DepartmentMaster?> GetByIdAsync(long id);
        Task<int> SaveAsync(DepartmentMaster model);
        Task<int> UpdateAsync(DepartmentMaster model);
        Task<int> DeleteAsync(long id, string user);
    }
}