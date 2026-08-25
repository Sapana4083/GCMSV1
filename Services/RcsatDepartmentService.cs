using GCMS.Models;
using GCMS.Repository.Interfaces;
using GCMS.Services.Interfaces;

namespace GCMS.Services
{
    public class RcsatDepartmentService : IRcsatDepartmentService
    {
        private readonly IRcsatDepartmentRepository _repository;

        public RcsatDepartmentService(IRcsatDepartmentRepository repository)
        {
            _repository = repository;
        }

        public Task<List<RcsatDepartmentMaster>> GetAllAsync(int pageNo, int rowCnt) =>
            _repository.GetAllAsync(pageNo, rowCnt);

        public Task<RcsatDepartmentMaster?> GetByIdAsync(long id) =>
            _repository.GetByIdAsync(id);

        public Task AddAsync(RcsatDepartmentMaster model) =>
            _repository.AddAsync(model);

        public Task UpdateAsync(RcsatDepartmentMaster model) =>
            _repository.UpdateAsync(model);

        public Task<List<RcsatDepartmentMaster>> GetDepartmentNameListAsync() =>
    _repository.GetDepartmentNameListAsync();

        public Task<RcsatDepartmentMaster?> GetDepartmentDetailAsync(long id) =>
            _repository.GetDepartmentDetailAsync(id);
    }
}