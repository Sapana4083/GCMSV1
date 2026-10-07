using GCMS.Models;
using GCMS.Repository.Interfaces;
using GCMS.Services.Interfaces;

namespace GCMS.Services
{
    public class VcLinkService : IVcLinkService
    {
        private readonly IVcLinkRepository _repository;

        public VcLinkService(IVcLinkRepository repository)
        {
            _repository = repository;
        }

        public Task<List<VcLink>> GetAllAsync(string courtCode) => _repository.GetAllAsync(courtCode);

        public Task<VcLink?> GetByIdAsync(long id) => _repository.GetByIdAsync(id);

        public Task AddAsync(VcLink model, string courtCode, string createdBy) =>
            _repository.AddAsync(model, courtCode, createdBy);

        public Task UpdateAsync(VcLink model, string courtCode, string createdBy) =>
            _repository.UpdateAsync(model, courtCode, createdBy);
    }
}