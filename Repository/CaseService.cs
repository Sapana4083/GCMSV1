using GCMS.Models.Entities;
using GCMS.Repository.Interfaces;

namespace GCMS.Repository
{
    public class CaseService : ICaseService
    {
        private readonly ICaseRepository _repository;

        public CaseService(ICaseRepository repository)
        {
            _repository = repository;
        }

        public async Task<long> SaveCaseAsync(CaseRegistration model)
        {
            return await _repository.SaveCaseAsync(model);
        }

        public async Task<CaseRegistration?> GetCaseAsync(long caseId)
        {
            return await _repository.GetCaseAsync(caseId);
        }

        public async Task SaveAppellantAsync(CaseAppellant model)
        {
            await _repository.SaveAppellantAsync(model);
        }

        public async Task SaveRespondentAsync(CaseRespondent model)
        {
            await _repository.SaveRespondentAsync(model);
        }

        public async Task SavePrivatePartyAsync(CasePrivateParty model)
        {
            await _repository.SavePrivatePartyAsync(model);
        }

        public async Task DeleteCaseAsync(long caseId)
        {
            await _repository.DeleteCaseAsync(caseId);
        }
    }
}