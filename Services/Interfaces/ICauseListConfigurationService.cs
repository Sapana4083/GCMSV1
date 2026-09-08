using GCMS.Models;

namespace GCMS.Services.Interfaces
{
    public interface ICauseListConfigurationService
    {
        Task<List<CasePurposeMaster>> GetCasePurposesAsync();
    }
}
