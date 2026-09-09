using GCMS.Models;
using GCMS.Models.ViewModels;

namespace GCMS.Services.Interfaces
{
    public interface ICauseListConfigurationService
    {
        Task<List<CasePurposeMaster>> GetCasePurposesAsync();

        Task<List<CauseListParamsListItem>> GetCauseListAsync();

        Task<CauseListConfigurationViewModel?> GetCauseListByIdAsync(long id);

        Task<long> SaveCauseListAsync(CauseListConfigurationViewModel model, string createdBy);
    }
}
