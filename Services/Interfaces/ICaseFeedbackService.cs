using GCMS.Models.ViewModels;

namespace GCMS.Services.Interfaces
{
    public interface ICaseFeedbackService
    {
        Task<List<CaseFeedbackCaseNoItem>> GetCaseNosAsync(long caseTypeId, string? manualCaseNo, string courtCode);

        Task SaveAsync(CaseFeedbackViewModel model, string createdBy);
    }
}