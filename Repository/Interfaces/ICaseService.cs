using GCMS.Models.Entities;

namespace GCMS.Repository.Interfaces
{
    public interface ICaseService
    {
        Task<List<CaseRegistration>> GetAllAsync();

        Task<CaseRegistration?> GetByIdAsync(long caseId);

        Task<long> AddAsync(CaseRegistration model);

        Task UpdateAsync(CaseRegistration model);

        Task DeleteAsync(long caseId);
    }
}
