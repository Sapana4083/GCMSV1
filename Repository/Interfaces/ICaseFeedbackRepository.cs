using GCMS.Models.ViewModels;

namespace GCMS.Repository.Interfaces
{
    public interface ICaseFeedbackRepository
    {
        Task<List<CaseFeedbackCaseNoItem>> GetCaseNosAsync(long caseTypeId, string? manualCaseNo, string courtCode);

        Task SaveAsync(CaseFeedbackViewModel model, string createdBy);
    }
}