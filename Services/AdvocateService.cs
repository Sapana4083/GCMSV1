using GCMS.Models;
using GCMS.Repository.Interfaces;
using GCMS.Services.Interfaces;

namespace GCMS.Services
{
    public class AdvocateService : IAdvocateService
    {
        private readonly IAdvocateRepository _repository;

        public AdvocateService(IAdvocateRepository repository)
        {
            _repository = repository;
        }

        public Task<List<AdvocateMaster>> GetAllAsync(int pageNo, int rowCnt) =>
            _repository.GetAllAsync(pageNo, rowCnt);

        public Task<AdvocateMaster?> GetByIdAsync(long id) =>
            _repository.GetByIdAsync(id);

        public Task AddAsync(AdvocateMaster model) =>
            _repository.AddAsync(model);

        public Task UpdateAsync(AdvocateMaster model) =>
            _repository.UpdateAsync(model);

        public Task<List<AdvocateMaster>> GetAdvocatesByCourtCodeAsync(string courtCode) =>
    _repository.GetAdvocatesByCourtCodeAsync(courtCode);

        public Task<List<AdvocateMaster>> GetRespondentAdvocatesAsync(string courtCode, long departmentId) =>
    _repository.GetRespondentAdvocatesAsync(courtCode, departmentId);

        public Task<List<AdvocateMaster>> GetPrivateAdvocatesAsync() =>
            _repository.GetPrivateAdvocatesAsync();
    }
}