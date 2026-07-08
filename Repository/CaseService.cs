using GCMS.Models.Entities;
using GCMS.Repository.Interfaces;
using GCMS.Services.Interfaces;

namespace GCMS.Repository
{
    public class CaseService : ICaseService
    {
        private readonly ICaseRepository _repo;

        public CaseService(ICaseRepository repo)
        {
            _repo = repo;
        }

        public async Task<List<CaseRegistration>> GetAllAsync()
        {
            return await _repo.GetAllAsync();
        }

        public async Task<CaseRegistration?> GetByIdAsync(long caseId)
        {
            return await _repo.GetByIdAsync(caseId);
        }

        public async Task<long> AddAsync(CaseRegistration model)
        {
            return await _repo.AddAsync(model);
        }

        public async Task UpdateAsync(CaseRegistration model)
        {
            await _repo.UpdateAsync(model);
        }

        public async Task DeleteAsync(long caseId)
        {
            await _repo.DeleteAsync(caseId);
        }
    }
}
