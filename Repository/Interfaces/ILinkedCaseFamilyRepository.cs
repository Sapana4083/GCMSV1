using GCMS.Models;

namespace GCMS.Repository.Interfaces
{
    public interface ILinkedCaseFamilyRepository
    {
        Task<List<LinkedCaseFamilyViewModel>> GetCaseFamilyAsync(
        string linkCase, string parentCaseNo, string courtCode);
    }
}
