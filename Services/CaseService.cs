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

        // FINAL SUBMIT
        // FIX: Previously this method manually built 4 separate EF entities
        // (CaseRegistration, CaseAppellant, CaseRespondent, CasePrivateParty)
        // and called 4 separate EF SaveChangesAsync inserts. That path hit
        // ORA-00904 because the CaseRegistration entity's [Column] mapping for
        // the "Impugned" flag didn't match the real Oracle column name, and it
        // did not correctly handle MULTIPLE private party rows (Step 4).
        //
        // Now delegates the entire save (Step 1 + 2 + 3 + 4) to the single
        // Oracle stored procedure PROC_TRN_RCSAT_CASEREG_FULL, which:
        //   - inserts into TRN_RCSAT_CASEREG / TRN_RCSAT_APPELLANT /
        //     TRN_RCSAT_RESPONDENT with its own known-good column names
        //     (sidesteps the EF column-mapping bug entirely), and
        //   - natively supports multiple private parties via comma-separated
        //     p_private_name / p_private_designation / p_privadvocatee params
        //     (built from model.PrivateParties in the repository).
        public async Task<long> SaveFullCaseRegistrationAsync(
            CaseRegistrationWizardViewModel model,
            string createdBy)
        {
            return await _repository.SaveFullCaseRegistrationAsync(model, createdBy);
        }

        // CASE READ / DELETE
        public async Task<CaseRegistration?> GetCaseAsync(long caseId)
        {
            return await _repository.GetCaseAsync(caseId);
        }

        public async Task DeleteCaseAsync(long caseId)
        {
            await _repository.DeleteCaseAsync(caseId);
        }

        // DROPDOWNS
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

        public async Task<IEnumerable<SelectListItem>> GetOrderTypesAsync()
        {
            return await _repository.GetOrderTypesAsync();
        }
    }
}