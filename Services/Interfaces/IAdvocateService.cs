using GCMS.Models;

namespace GCMS.Services.Interfaces
{
    public interface IAdvocateService
    {
        Task<List<AdvocateMaster>> GetAllAsync(int pageNo, int rowCnt);

        Task<AdvocateMaster?> GetByIdAsync(long id);

        Task AddAsync(AdvocateMaster model);

        Task UpdateAsync(AdvocateMaster model);

        Task<List<AdvocateMaster>> GetAdvocatesByCourtCodeAsync(string courtCode);
    }
}