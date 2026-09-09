using GCMS.Models;
using GCMS.Models.ViewModels;

namespace GCMS.Repository.Interfaces
{
    public interface ICauseListConfigurationRepository
    {
        Task<List<CasePurposeMaster>> GetCasePurposesAsync();

        Task<List<CauseListParamsListItem>> GetCauseListAsync();

        Task<CauseListConfigurationViewModel?> GetCauseListByIdAsync(long id);
    }
}
