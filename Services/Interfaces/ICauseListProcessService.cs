using GCMS.Models.ViewModels;

namespace GCMS.Services.Interfaces
{
    public interface ICauseListProcessService
    {
        Task GenerateCauseListAsync(CauseListGenerateViewModel model);

        Task<List<CauseListGeneratedRow>> GetGeneratedListAsync(DateTime hearingDate, string courtCode);
    }
}