using GCMS.Models;

namespace GCMS.Repository.Interfaces
{
    public interface ICourtGroupRepository
    {
        Task<List<CourtGroupMaster>> GetAllAsync(int pageNo, int rowCnt);

        Task<CourtGroupMaster?> GetByIdAsync(long id);

        Task AddAsync(CourtGroupMaster model);

        Task UpdateAsync(CourtGroupMaster model);
    }
}