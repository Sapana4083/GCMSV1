using System.Collections.Generic;
using System.Threading.Tasks;
using GCMS.Models;

namespace GCMS.Repository.Interfaces
{
    public interface ILinkedCaseRepository
    {
        Task<List<LinkedCaseRowViewModel>> GetLinkedCasesAsync(string mainCase);
        Task<LinkedCaseRowViewModel> GetByCaseNoAsync(string caseNo);
        Task<bool> CaseExistsAsync(string caseNo);
        Task<bool> SaveAsync(LinkedCaseRowViewModel row);
        Task<bool> DeleteAsync(long caseId);
    }
}