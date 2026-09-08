using GCMS.Models;
using GCMS.Repository.Interfaces;
using GCMS.Services.Interfaces;

namespace GCMS.Services
{
    public class CauseListConfigurationService : ICauseListConfigurationService
    {
        private readonly ICauseListConfigurationRepository _repository;

        public CauseListConfigurationService(
            ICauseListConfigurationRepository repository)
        {
            _repository = repository;
        }


        public async Task<List<CasePurposeMaster>> GetCasePurposesAsync()
        {
            return await _repository.GetCasePurposesAsync();
        }

    }
}
