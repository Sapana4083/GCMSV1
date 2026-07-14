using GCMS.Models;
using GCMS.Repository;
using GCMS.Repository.Interfaces;
using GCMS.Services.Interfaces;

namespace GCMS.Services
{
    public class TehsilService : ITehsilService
    {
        private readonly ITehsilRepository _repository;

        public TehsilService(ITehsilRepository repository)
        {
            _repository = repository;
        }

        public async Task AddAsync(TehsilMaster model)
        {
            await _repository.AddAsync(model);
        }

        public async Task UpdateAsync(TehsilMaster model)
        {
            await _repository.UpdateAsync(model);
        }

        public async Task<TehsilMaster?> GetByIdAsync(long tehsilMastId)
        {
            return await _repository.GetByIdAsync(tehsilMastId);
        }

        public async Task<List<TehsilMaster>> GetAllAsync(int pageNo, int rowCount)
        {
            return await _repository.GetAllAsync(pageNo, rowCount);
        }
    }
}