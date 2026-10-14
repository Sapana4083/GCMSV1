using GCMS.Models.ViewModels;
using GCMS.Repository.Interfaces;
using GCMS.Services.Interfaces;

namespace GCMS.Services
{
    public class ConnectedCaseService : IConnectedCaseService
    {
        private readonly IConnectedCaseRepository _repository;

        public ConnectedCaseService(IConnectedCaseRepository repository)
        {
            _repository = repository;
        }

        // Main case no -> SP action 4 se parent id -> action 3 se poora data
        public async Task<ConnectedCaseUpdateViewModel?> GetByMainCaseAsync(string caseNo)
        {
            var row = await _repository.GetRowByCaseNoAsync(caseNo);
            if (row?.CaseUpdateId == null) return null;

            return await _repository.GetByIdAsync(row.CaseUpdateId.Value);
        }

        public Task<ConnectedCaseRowViewModel?> GetRowByCaseNoAsync(string caseNo) =>
            _repository.GetRowByCaseNoAsync(caseNo);

        public Task<long> SaveAsync(ConnectedCaseUpdateViewModel model, string userName) =>
            _repository.SaveAsync(model, userName);

        public Task<ConnectedCaseRowViewModel?> GetCaseRegDetailsAsync(string caseNo, long? caseTypeId) =>
    _repository.GetCaseRegDetailsAsync(caseNo, caseTypeId);
        public Task<List<ConnectedCaseRowViewModel>> GetRowsByMainCaseAsync(string mainCase) =>
    _repository.GetRowsByMainCaseAsync(mainCase);
    }
}