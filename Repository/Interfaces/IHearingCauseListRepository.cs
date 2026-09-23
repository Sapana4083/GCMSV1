using GCMS.Models.ViewModels;

namespace GCMS.Repository.Interfaces
{
    public interface IHearingCauseListRepository
    {
        Task<List<HearingCauseListRowViewModel>> GetRowsAsync(
            DateTime hearingDate, string benchTypeId, string causeListType, string courtCode, string createdBy);

        Task SaveRowAsync(
            SaveHearingCauseListRequest request, SaveHearingCauseListRowRequest row, string courtCode, string createdBy);
    }
}