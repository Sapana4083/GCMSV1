using GCMS.Models.Entities;
using GCMS.Models.ViewModels;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace GCMS.Services.Interfaces
{
    public interface ICaseService
    {
        // STEP-1
        Task<long> SaveCaseAsync(CaseRegistration model);

        Task<bool> UpdateCaseAsync(CaseRegistration model);

        Task<CaseRegistration?> GetCaseAsync(long caseId);

        // STEP-2
        Task<long> SaveAppellantAsync(CaseAppellant model);
        Task UpdateAppellantAsync(CaseAppellant model);
      
        // STEP-3
        Task SaveRespondentAsync(CaseRespondent model);
        Task UpdateRespondentAsync(CaseRespondent model);       

        // STEP-4
        Task SavePrivatePartyAsync(CasePrivateParty model);
        Task UpdatePrivatePartyAsync(CasePrivateParty model);

        Task DeleteCaseAsync(long caseId);

        // Dropdowns
        Task<IEnumerable<SelectListItem>> GetCaseTypesAsync();

        Task<IEnumerable<SelectListItem>> GetCaseSubjectsAsync();

        Task<IEnumerable<SelectListItem>> GetCasePurposesAsync();

        Task<IEnumerable<SelectListItem>> GetBenchTypesAsync();

        Task<IEnumerable<SelectListItem>> GetDepartmentsAsync();

        Task<IEnumerable<SelectListItem>> GetDesignationsAsync();

        Task<IEnumerable<SelectListItem>> GetDistrictsAsync();

        Task<IEnumerable<SelectListItem>> GetAdvocatesAsync();
        //Task<CaseRegistrationWizardViewModel?> GetCaseRegistrationAsync(long caseId);

       
    }
}