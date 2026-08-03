using GCMS.Models;
using GCMS.Repository.Interfaces;
using GCMS.Services.Interfaces;

namespace GCMS.Services
{
    public class CourtGroupService : ICourtGroupService
    {
        private readonly ICourtGroupRepository _repository;

        public CourtGroupService(ICourtGroupRepository repository)
        {
            _repository = repository;
        }

        public Task<List<CourtGroupMaster>> GetAllAsync(int pageNo, int rowCnt)
        {
            return _repository.GetAllAsync(pageNo, rowCnt);
        }

        public Task<CourtGroupMaster?> GetByIdAsync(long id)
        {
            return _repository.GetByIdAsync(id);
        }

        public Task AddAsync(CourtGroupMaster model)
        {
            return _repository.AddAsync(model);
        }

        public Task UpdateAsync(CourtGroupMaster model)
        {
            return _repository.UpdateAsync(model);
        }
    }
}