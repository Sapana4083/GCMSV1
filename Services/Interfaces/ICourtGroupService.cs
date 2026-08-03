using GCMS.Models;

namespace GCMS.Services.Interfaces
{
    public interface ICourtGroupService
    {
        Task<List<CourtGroupMaster>> GetAllAsync(int pageNo, int rowCnt);

        Task<CourtGroupMaster?> GetByIdAsync(long id);

        Task AddAsync(CourtGroupMaster model);

        Task UpdateAsync(CourtGroupMaster model);
    }
}