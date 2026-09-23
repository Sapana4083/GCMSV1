using GCMS.Models.ViewModels;
using GCMS.Repository.Interfaces;
using GCMS.Services.Interfaces;

namespace GCMS.Services
{
    public class CaseFeedbackService : ICaseFeedbackService
    {
        private readonly ICaseFeedbackRepository _repository;

        public CaseFeedbackService(ICaseFeedbackRepository repository)
        {
            _repository = repository;
        }

        public Task<List<CaseFeedbackCaseNoItem>> GetCaseNosAsync(long caseTypeId, string? manualCaseNo, string courtCode) =>
            _repository.GetCaseNosAsync(caseTypeId, manualCaseNo, courtCode);

        public Task SaveAsync(CaseFeedbackViewModel model, string createdBy) =>
            _repository.SaveAsync(model, createdBy);
    }
}