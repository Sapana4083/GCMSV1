using GCMS.Models.ViewModels;

namespace GCMS.Services.Interfaces
{
    public interface IConnectedCaseService
    {
        Task<ConnectedCaseUpdateViewModel?> GetByMainCaseAsync(string caseNo);
        Task<ConnectedCaseRowViewModel?> GetRowByCaseNoAsync(string caseNo);
        Task<long> SaveAsync(ConnectedCaseUpdateViewModel model, string userName);
        Task<ConnectedCaseRowViewModel?> GetCaseRegDetailsAsync(string caseNo, long? caseTypeId);
        Task<List<ConnectedCaseRowViewModel>> GetRowsByMainCaseAsync(string mainCase);
    }
}