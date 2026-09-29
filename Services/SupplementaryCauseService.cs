using GCMS.Models;
using GCMS.Repository.Interfaces;
using GCMS.Services.Interfaces;

namespace GCMS.Services
{
    public class SupplementaryCauseService : ISupplementaryCauseService
    {
        private readonly ISupplementaryCauseRepository _repository;

        public SupplementaryCauseService(ISupplementaryCauseRepository repository)
        {
            _repository = repository;
        }

        public Task<List<SupplementaryCause>> GetAllAsync() => _repository.GetAllAsync();

        public Task<SupplementaryCause?> GetByIdAsync(long id) => _repository.GetByIdAsync(id);

        public Task AddAsync(SupplementaryCause model, string courtCode, string createdBy) =>
            _repository.AddAsync(model, courtCode, createdBy);

        public Task UpdateAsync(SupplementaryCause model, string courtCode, string createdBy) =>
            _repository.UpdateAsync(model, courtCode, createdBy);
    }
}