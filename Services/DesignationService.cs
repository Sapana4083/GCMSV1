using GCMS.Models;
using GCMS.Repository.Interfaces;
using GCMS.Services.Interfaces;

namespace GCMS.Services
{
    public class DesignationService : IDesignationService
    {
        private readonly IDesignationRepository _repository;

        public DesignationService(IDesignationRepository repository)
        {
            _repository = repository;
        }

        public Task<List<DesignationMaster>> GetAllAsync(int pageNo, int rowCnt) =>
            _repository.GetAllAsync(pageNo, rowCnt);

        public Task<DesignationMaster?> GetByIdAsync(long id) =>
            _repository.GetByIdAsync(id);

        public Task AddAsync(DesignationMaster model) =>
            _repository.AddAsync(model);

        public Task UpdateAsync(DesignationMaster model) =>
            _repository.UpdateAsync(model);
        public Task<List<DesignationMaster>> GetDesignationDDL(int pageNo, int rowCnt) =>
           _repository.GetDesignationDDL(pageNo, rowCnt);
     
    }
}