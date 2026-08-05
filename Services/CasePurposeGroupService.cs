using GCMS.Models;
using GCMS.Repository.Interfaces;
using GCMS.Services.Interfaces;

namespace GCMS.Services
{
    public class CasePurposeGroupService : ICasePurposeGroupService
    {
        private readonly ICasePurposeGroupRepository _repository;

        public CasePurposeGroupService(ICasePurposeGroupRepository repository)
        {
            _repository = repository;
        }

        public async Task<List<CasePurposeGroupMaster>> GetAllAsync(int pageNo, int rowCnt)
        {
            return await _repository.GetAllAsync(pageNo, rowCnt);
        }

        public async Task<CasePurposeGroupMaster?> GetByIdAsync(long id)
        {
            return await _repository.GetByIdAsync(id);
        }

        public async Task AddAsync(CasePurposeGroupMaster model)
        {
            await _repository.AddAsync(model);
        }

        public async Task UpdateAsync(CasePurposeGroupMaster model)
        {
            await _repository.UpdateAsync(model);
        }

        public async Task DeleteAsync(long id, string modifiedBy)
        {
            await _repository.DeleteAsync(id, modifiedBy);
        }
    }
}