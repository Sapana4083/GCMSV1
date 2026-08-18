using GCMS.Models;
using GCMS.Repository.Interfaces;
using GCMS.Services.Interfaces;

namespace GCMS.Services
{
    public class CaseTypeService : ICaseTypeService
    {
        private readonly ICaseTypeRepository _repository;

        public CaseTypeService(ICaseTypeRepository repository)
        {
            _repository = repository;
        }

        public Task<List<CaseTypeMaster>> GetAllAsync(int pageNo, int rowCnt) =>
            _repository.GetAllAsync(pageNo, rowCnt);

        public Task<CaseTypeMaster?> GetByIdAsync(long id) =>
            _repository.GetByIdAsync(id);

        public Task AddAsync(CaseTypeMaster model) =>
            _repository.AddAsync(model);

        public Task UpdateAsync(CaseTypeMaster model) =>
            _repository.UpdateAsync(model);

        public async Task<List<CaseTypeMaster>> GetCaseTypeAsync(int pageNo, int rowCnt)
        {
            return await _repository.GetCaseTypeAsync(pageNo, rowCnt);
        }
    }
}