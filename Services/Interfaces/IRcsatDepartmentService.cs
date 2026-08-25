using GCMS.Models;

namespace GCMS.Services.Interfaces
{
    public interface IRcsatDepartmentService
    {
        Task<List<RcsatDepartmentMaster>> GetAllAsync(int pageNo, int rowCnt);

        Task<RcsatDepartmentMaster?> GetByIdAsync(long id);

        Task AddAsync(RcsatDepartmentMaster model);

        Task UpdateAsync(RcsatDepartmentMaster model);

        Task<List<RcsatDepartmentMaster>> GetDepartmentNameListAsync();

        Task<RcsatDepartmentMaster?> GetDepartmentDetailAsync(long id);
    }
}