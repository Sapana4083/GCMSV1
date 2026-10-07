using GCMS.Models;

namespace GCMS.Repository.Interfaces
{
    public interface IVcLinkRepository
    {
        Task<List<VcLink>> GetAllAsync(string courtCode);
        Task<VcLink?> GetByIdAsync(long id);
        Task AddAsync(VcLink model, string courtCode, string createdBy);
        Task UpdateAsync(VcLink model, string courtCode, string createdBy);
    }
}