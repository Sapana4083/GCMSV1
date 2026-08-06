using GCMS.Models;
using GCMS.Repository.Interfaces;
using GCMS.Services.Interfaces;

namespace GCMS.Services
{
    public class CasePurposeService : ICasePurposeService
    {
        private readonly ICasePurposeRepository _repository;

        public CasePurposeService(ICasePurposeRepository repository)
        {
            _repository = repository;
        }

        public Task<List<CasePurposeMaster>> GetAllAsync(int pageNo, int rowCnt) =>
            _repository.GetAllAsync(pageNo, rowCnt);

        public Task<CasePurposeMaster?> GetByIdAsync(long id) =>
            _repository.GetByIdAsync(id);

        public Task AddAsync(CasePurposeMaster model) =>
            _repository.AddAsync(model);

        public Task UpdateAsync(CasePurposeMaster model) =>
            _repository.UpdateAsync(model);
    }
}