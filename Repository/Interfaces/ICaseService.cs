using GCMS.Models.Entities;
using GCMS.Models.ViewModels;

namespace GCMS.Repository.Interfaces
{
    public interface ICaseService
    {
        //==========================
        // Master
        //==========================

        Task<long> SaveCaseAsync(CaseRegistration model);

        Task<CaseRegistration?> GetCaseAsync(long caseId);

        //==========================
        // Appellant
        //==========================

        Task SaveAppellantAsync(CaseAppellant model);

        //==========================
        // Respondent
        //==========================

        Task SaveRespondentAsync(CaseRespondent model);

        //==========================
        // Private Party
        //==========================

        Task SavePrivatePartyAsync(CasePrivateParty model);

        //==========================
        // Delete
        //==========================

        Task DeleteCaseAsync(long caseId);

        Task<long> SaveFullCaseRegistrationAsync(
    CaseRegistrationWizardViewModel model,
    string createdBy);
    }
}