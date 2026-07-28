using GCMS.Models;
using GCMS.Models.Entities;
using GCMS.Repository.Interfaces;
using GCMS.Services.Interfaces;

namespace GCMS.Services
{
    public class DepartmentService : IDepartmentService
    {
        private readonly IDepartmentRepository _repository;

        public DepartmentService(IDepartmentRepository repository)
        {
            _repository = repository;
        }

        public async Task<List<DepartmentMaster>> GetAllAsync(int pageNo, int rowCnt)
        {
            return await _repository.GetAllAsync(pageNo, rowCnt);
        }

        public async Task<DepartmentMaster?> GetByIdAsync(long id)
        {
            return await _repository.GetByIdAsync(id);
        }

        public async Task<int> SaveAsync(DepartmentMaster model)
        {
            return await _repository.SaveAsync(model);
        }

        public async Task<int> UpdateAsync(DepartmentMaster model)
        {
            return await _repository.UpdateAsync(model);
        }

        public async Task<int> DeleteAsync(long id, string user)
        {
            return await _repository.DeleteAsync(id, user);
        }
    }
}