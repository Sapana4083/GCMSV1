using GCMS.Models;

namespace GCMS.Repository.Interfaces
{
    public interface ICauseListConfigurationRepository
    {
        Task<List<CasePurposeMaster>> GetCasePurposesAsync();
    }
}
