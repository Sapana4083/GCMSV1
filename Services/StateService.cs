using GCMS.Repository.Interfaces;
using GCMS.Services.Interfaces;
using GCMS.Models;

namespace GCMS.Services
{
    public class StateService : IStateService
    {
        private readonly IStateRepository _repository;

        public StateService(
            IStateRepository repository)
        {
            _repository = repository;
        }

        public async Task<List<StateMaster>> GetAllAsync(
    int pageNo,
    int rowCnt)
        {
            return await _repository.GetAllAsync(
                pageNo,
                rowCnt);
        }

        public Task<StateMaster?> GetByIdAsync(long id)
            => _repository.GetByIdAsync(id);

        public Task AddAsync(StateMaster state)
            => _repository.AddAsync(state);

        public Task UpdateAsync(StateMaster state)
            => _repository.UpdateAsync(state);

        public Task DeleteAsync(long id)
            => _repository.DeleteAsync(id);
    }
}
