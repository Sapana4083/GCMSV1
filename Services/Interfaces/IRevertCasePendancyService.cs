using GCMS.Models;

namespace GCMS.Services.Interfaces
{
    public interface IRevertCasePendancyService
    {
        Task<RevertCasePendancy?> GetByHearingDateAsync(DateTime hearingDate);
        Task<RevertCasePendancy> SaveAsync(RevertCasePendancy model, string createdBy);
    }
}