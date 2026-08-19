using GCMS.Models;
using GCMS.Repository.Interfaces;
using GCMS.Services.Interfaces;

namespace GCMS.Services
{
    public class CourtTypeService : ICourtTypeService
    {
        private readonly ICourtTypeRepository _repository;

        public CourtTypeService(ICourtTypeRepository repository)
        {
            _repository = repository;
        }

        public async Task<List<CourtTypeMaster>> GetAllAsync(int pageNo, int rowCnt)
        {
            return await _repository.GetAllAsync(pageNo, rowCnt);
        }

        //public async Task<List<CaseTypeMaster>> GetCaseTypeAsync(int pageNo, int rowCnt)
        //{
        //    return await _repository.GetCaseTypeAsync(pageNo, rowCnt);
        //}
        public async Task<CourtTypeMaster?> GetByIdAsync(long id)
        {
            return await _repository.GetByIdAsync(id);
        }

        public async Task AddAsync(CourtTypeMaster model)
        {
            await _repository.AddAsync(model);
        }

        public async Task UpdateAsync(CourtTypeMaster model)
        {
            await _repository.UpdateAsync(model);
        }

        public async Task<List<LovModel>> GetCourtCategoryAsync()
        {
            return await _repository.GetCourtCategoryAsync();
        }
    }
}