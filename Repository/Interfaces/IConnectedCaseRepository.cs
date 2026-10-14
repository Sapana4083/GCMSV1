using GCMS.Models.ViewModels;

namespace GCMS.Repository.Interfaces
{
    public interface IConnectedCaseRepository
    {
        Task<ConnectedCaseRowViewModel?> GetRowByCaseNoAsync(string caseNo);
        Task<ConnectedCaseUpdateViewModel?> GetByIdAsync(long caseUpdateId);
        Task<long> SaveAsync(ConnectedCaseUpdateViewModel model, string userName);

        Task<List<ConnectedCaseRowViewModel>> GetRowsByMainCaseAsync(string mainCase);

        Task<ConnectedCaseRowViewModel?> GetCaseRegDetailsAsync(string caseNo, long? caseTypeId);
    }
}