using GCMS.Models;

namespace GCMS.Services.Interfaces
{
    public interface IVcLinkService
    {
        Task<List<VcLink>> GetAllAsync(string courtCode);
        Task<VcLink?> GetByIdAsync(long id);
        Task AddAsync(VcLink model, string courtCode, string createdBy);
        Task UpdateAsync(VcLink model, string courtCode, string createdBy);
    }
}