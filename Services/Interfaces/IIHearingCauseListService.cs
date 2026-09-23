using GCMS.Models.ViewModels;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace GCMS.Services.Interfaces
{
    public interface IHearingCauseListService
    {
        Task<HearingCauseListViewModel> BuildViewModelAsync(
            DateTime? hearingDate, string? benchTypeId, string? causeListType);

        Task<List<HearingCauseListRowViewModel>> GetRowsAsync(
            DateTime hearingDate, string benchTypeId, string causeListType, string courtCode, string createdBy);

        Task SaveRowsAsync(SaveHearingCauseListRequest request, string courtCode, string createdBy);
    }
}