using GCMS.Models.Entities;
using GCMS.Models.ViewModels;
using GCMS.Repository.Interfaces;
using GCMS.Services.Interfaces;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace GCMS.Services
{
    public class CaseService : ICaseService
    {
        private readonly ICaseRepository _repository;

        public CaseService(ICaseRepository repository)
        {
            _repository = repository;
        }

        // STEP-1

        public async Task<long> SaveCaseAsync(CaseRegistration model)
        {
            return await _repository.SaveCaseAsync(model);
        }

        public async Task<bool> UpdateCaseAsync(CaseRegistration model)
        {
            return await _repository.UpdateCaseAsync(model);
        }

        public async Task<CaseRegistration?> GetCaseAsync(long caseId)
        {
            return await _repository.GetCaseAsync(caseId);
        }

        // STEP-2

        public async Task<long> SaveAppellantAsync(CaseAppellant model)
        {
            return await _repository.SaveAppellantAsync(model);
        }
        public async Task UpdateAppellantAsync(CaseAppellant model)
        {
            await _repository.UpdateAppellantAsync(model);
        }
        // STEP-3

        public async Task SaveRespondentAsync(CaseRespondent model)
        {
            await _repository.SaveRespondentAsync(model);
        }
        public async Task UpdateRespondentAsync(CaseRespondent model)
        {
            await _repository.UpdateRespondentAsync(model);
        }
        // STEP-4

        public async Task SavePrivatePartyAsync(CasePrivateParty model)
        {
            await _repository.SavePrivatePartyAsync(model);
        }
        public async Task UpdatePrivatePartyAsync(CasePrivateParty model)
        {
            await _repository.UpdatePrivatePartyAsync(model);
        }

        public async Task DeleteCaseAsync(long caseId)
        {
            await _repository.DeleteCaseAsync(caseId);
        }

        //=========================
        // Dropdowns
        //=========================

        public async Task<IEnumerable<SelectListItem>> GetCaseTypesAsync()
        {
            return await _repository.GetCaseTypesAsync();
        }

        public async Task<IEnumerable<SelectListItem>> GetCaseSubjectsAsync()
        {
            return await _repository.GetCaseSubjectsAsync();
        }

        public async Task<IEnumerable<SelectListItem>> GetCasePurposesAsync()
        {
            return await _repository.GetCasePurposesAsync();
        }

        public async Task<IEnumerable<SelectListItem>> GetBenchTypesAsync()
        {
            return await _repository.GetBenchTypesAsync();
        }

        public async Task<IEnumerable<SelectListItem>> GetDepartmentsAsync()
        {
            return await _repository.GetDepartmentsAsync();
        }

        public async Task<IEnumerable<SelectListItem>> GetDesignationsAsync()
        {
            return await _repository.GetDesignationsAsync();
        }

        public async Task<IEnumerable<SelectListItem>> GetDistrictsAsync()
        {
            return await _repository.GetDistrictsAsync();
        }

        public async Task<IEnumerable<SelectListItem>> GetAdvocatesAsync()
        {
            return await _repository.GetAdvocatesAsync();
        }
        //public async Task<CaseRegistrationWizardViewModel?> GetCaseRegistrationAsync(long caseId)
        //{
        //    return await _repository.GetCaseRegistrationAsync(caseId);
        //}

    }
}