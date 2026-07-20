using GCMS.Models.Entities;
using GCMS.Models.ViewModels;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace GCMS.Repository.Interfaces
{
    public interface ICaseRepository
    {
        //========================
        // CASE
        //========================

        Task<long> SaveCaseAsync(CaseRegistration model);

        Task<bool> UpdateCaseAsync(CaseRegistration model);

        Task<CaseRegistration?> GetCaseAsync(long caseId);

        Task DeleteCaseAsync(long caseId);

        //========================
        // APPELLANT
        //========================

        Task<long> SaveAppellantAsync(CaseAppellant model);
        Task<bool> UpdateAppellantAsync(CaseAppellant model);
        
        //========================
        // RESPONDENT
        //========================

        Task<long> SaveRespondentAsync(CaseRespondent model);
        Task<bool> UpdateRespondentAsync(CaseRespondent model);

        //========================
        // PRIVATE PARTY
        //========================

        Task<long> SavePrivatePartyAsync(CasePrivateParty model);
        Task<bool> UpdatePrivatePartyAsync(CasePrivateParty model);

        //========================
        // DROPDOWNS
        //========================

        Task<IEnumerable<SelectListItem>> GetCaseTypesAsync();

        Task<IEnumerable<SelectListItem>> GetCaseSubjectsAsync();

        Task<IEnumerable<SelectListItem>> GetCasePurposesAsync();

        Task<IEnumerable<SelectListItem>> GetBenchTypesAsync();

        Task<IEnumerable<SelectListItem>> GetDepartmentsAsync();

        Task<IEnumerable<SelectListItem>> GetDistrictsAsync();

        Task<IEnumerable<SelectListItem>> GetDesignationsAsync();

        Task<IEnumerable<SelectListItem>> GetAdvocatesAsync();
    //Task<CaseRegistrationWizardViewModel?> GetCaseRegistrationAsync(long caseId);
    }
}