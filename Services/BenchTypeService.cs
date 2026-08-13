using GCMS.Models;
using GCMS.Repository.Interfaces;
using GCMS.Services.Interfaces;

namespace GCMS.Services
{
    public class BenchTypeService : IBenchTypeService
    {
        private readonly IBenchTypeRepository _repository;

        public BenchTypeService(IBenchTypeRepository repository)
        {
            _repository = repository;
        }

        public Task<List<BenchTypeMaster>> GetAllAsync(int pageNo, int rowCnt) =>
            _repository.GetAllAsync(pageNo, rowCnt);

        public Task<BenchTypeMaster?> GetByIdAsync(long id) =>
            _repository.GetByIdAsync(id);

        public Task AddAsync(BenchTypeMaster model) =>
            _repository.AddAsync(model);

        public Task UpdateAsync(BenchTypeMaster model) =>
            _repository.UpdateAsync(model);
    }
}