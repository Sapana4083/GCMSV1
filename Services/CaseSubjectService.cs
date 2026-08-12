using GCMS.Models;
using GCMS.Repository.Interfaces;
using GCMS.Services.Interfaces;

namespace GCMS.Services
{
    public class CaseSubjectService : ICaseSubjectService
    {
        private readonly ICaseSubjectRepository _repository;

        public CaseSubjectService(ICaseSubjectRepository repository)
        {
            _repository = repository;
        }

        public Task<List<CaseSubjectMaster>> GetAllAsync(int pageNo, int rowCnt) =>
            _repository.GetAllAsync(pageNo, rowCnt);

        public Task<CaseSubjectMaster?> GetByIdAsync(long id) =>
            _repository.GetByIdAsync(id);

        public Task AddAsync(CaseSubjectMaster model) =>
            _repository.AddAsync(model);

        public Task UpdateAsync(CaseSubjectMaster model) =>
            _repository.UpdateAsync(model);
    }
}