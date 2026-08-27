using GCMS.Models.Entities;
using GCMS.Models.ViewModels;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace GCMS.Repository.Interfaces
{
    public interface ICaseRepository
    {
        // FINAL SUBMIT
        // Saves Step 1 + Step 2 + Step 3 + Step 4 using one Oracle stored procedure.
        Task<long> SaveFullCaseRegistrationAsync(
            CaseRegistrationWizardViewModel model,
            string createdBy);

        // CASE READ / DELETE
        Task<CaseRegistration?> GetCaseAsync(long caseId);

        Task DeleteCaseAsync(long caseId);

        // DROPDOWNS
        Task<IEnumerable<SelectListItem>> GetCaseTypesAsync();

        Task<IEnumerable<SelectListItem>> GetCaseSubjectsAsync();

        Task<IEnumerable<SelectListItem>> GetCasePurposesAsync();

        Task<IEnumerable<SelectListItem>> GetBenchTypesAsync();

        Task<IEnumerable<SelectListItem>> GetDepartmentsAsync();

        Task<IEnumerable<SelectListItem>> GetDistrictsAsync();

        Task<IEnumerable<SelectListItem>> GetDesignationsAsync();

        Task<IEnumerable<SelectListItem>> GetAdvocatesAsync();

        Task<IEnumerable<SelectListItem>> GetOrderTypesAsync();
    }
}