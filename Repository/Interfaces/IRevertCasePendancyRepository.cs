using GCMS.Models;

namespace GCMS.Repository.Interfaces
{
    public interface IRevertCasePendancyRepository
    {
        Task<RevertCasePendancy?> GetByHearingDateAsync(DateTime hearingDate);
        Task<RevertCasePendancy> AddAsync(RevertCasePendancy model, string createdBy);
    }
}