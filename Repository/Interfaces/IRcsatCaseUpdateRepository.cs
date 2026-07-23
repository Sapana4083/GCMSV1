using System.Threading.Tasks;
using GCMS.Models;

namespace GCMS.Repository.Interfaces
{
    public interface IRcsatCaseUpdateRepository
    {
        Task<long> SaveCaseWithLinkedRowsAsync(RcsatCaseUpdateViewModel model);
        Task<RcsatCaseUpdateViewModel> GetByLinkCaseAsync(string linkCase);
    }
}