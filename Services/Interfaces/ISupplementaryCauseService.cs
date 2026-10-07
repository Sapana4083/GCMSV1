using GCMS.Models;

namespace GCMS.Services.Interfaces
{
    public interface ISupplementaryCauseService
    {
        Task<List<SupplementaryCause>> GetAllAsync();
        Task<SupplementaryCause?> GetByIdAsync(long supplyCauseListId);
        Task AddAsync(SupplementaryCause model, string courtCode, string createdBy);
        Task UpdateAsync(SupplementaryCause model, string courtCode, string createdBy);
    }
}