using GCMS.Repository.Interfaces;
using GCMS.Services.Interfaces;
using GCMS.Models;

namespace GCMS.Services
{
    public class DistrictService : IDistrictService
    {
        private readonly IDistrictRepository _repository;

        public DistrictService(IDistrictRepository repository)
        {
            _repository = repository;
        }

        public async Task<List<DistrictMaster>>
            GetAllAsync(
                int pageNo,
                int rowCount)
        {
            return await _repository.GetAllAsync(
                pageNo,
                rowCount);
        }

        public async Task<DistrictMaster?>
            GetByIdAsync(long id)
        {
            return await _repository.GetByIdAsync(id);
        }

        public async Task AddAsync(
            DistrictMaster model)
        {
            await _repository.AddAsync(model);
        }

        public async Task UpdateAsync(
            DistrictMaster model)
        {
            await _repository.UpdateAsync(model);
        }

        public async Task DeleteAsync(
            long id)
        {
            await _repository.DeleteAsync(id);
        }
    }
}