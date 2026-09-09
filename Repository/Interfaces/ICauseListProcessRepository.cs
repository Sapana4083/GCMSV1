using GCMS.Models.ViewModels;

namespace GCMS.Repository.Interfaces
{
    public interface ICauseListProcessRepository
    {
        Task GenerateCauseListAsync(CauseListGenerateViewModel model);

        Task<List<CauseListGeneratedRow>> GetGeneratedListAsync(DateTime hearingDate, string courtCode);
    }
}