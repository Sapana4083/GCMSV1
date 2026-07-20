using GCMS.Models.Entities;

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
    }
}