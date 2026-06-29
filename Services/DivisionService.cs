using GCMS.Repository.Interfaces;
using GCMS.Services.Interfaces;
using GCMS.Models;



namespace GCMS.Services
{
    public class DivisionService : IDivisionService
    {
        private readonly IDivisionRepository _repository;

        public DivisionService(IDivisionRepository repository)
        {
            _repository = repository;
        }

        public async Task<List<DivisionMaster>> GetAllAsync(
     int pageNo,
     int rowCnt)
        {
            return await _repository.GetAllAsync(
                pageNo,
                rowCnt);
        }

        public async Task<DivisionMaster?> GetByIdAsync(long id)
            => await _repository.GetByIdAsync(id);

        public async Task AddAsync(DivisionMaster model)
            => await _repository.AddAsync(model);

        public async Task UpdateAsync(DivisionMaster model)
            => await _repository.UpdateAsync(model);

        public async Task DeleteAsync(long id)
            => await _repository.DeleteAsync(id);
    }
}
