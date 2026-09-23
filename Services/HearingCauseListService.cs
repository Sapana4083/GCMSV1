using GCMS.Models.ViewModels;
using GCMS.Repository.Interfaces;
using GCMS.Services.Interfaces;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace GCMS.Services
{
    public class HearingCauseListService : IHearingCauseListService
    {
        private static readonly SelectListItem[] CauseListTypes =
        {
            new() { Value = "Regular", Text = "Regular" },
            new() { Value = "Supplementary", Text = "Supplementary" },
            new() { Value = "Urgent", Text = "Urgent" }
        };

        private static readonly SelectListItem[] BenchNumbers =
        {
            new() { Value = "1", Text = "1" },
            new() { Value = "2", Text = "2" },
            new() { Value = "R", Text = "R" }
        };

        private readonly IHearingCauseListRepository _repository;
        private readonly IAdvocateService _advocateService;

        public HearingCauseListService(
            IHearingCauseListRepository repository,
            IAdvocateService advocateService)
        {
            _repository = repository;
            _advocateService = advocateService;
        }

        public async Task<HearingCauseListViewModel> BuildViewModelAsync(
            DateTime? hearingDate, string? benchTypeId, string? causeListType)
        {
            long? selectedBench = null;
            if (long.TryParse(benchTypeId, out var numericBench))
            {
                selectedBench = numericBench;
            }

            return new HearingCauseListViewModel
            {
                HearingDate = (hearingDate ?? DateTime.Today).Date,
                BenchTypeId = selectedBench,
                BenchNo = benchTypeId,
                CauseListType = string.IsNullOrWhiteSpace(causeListType) ? "Regular" : causeListType,
                BenchTypes = BenchNumbers,
                CauseListTypes = CauseListTypes,
                PrivateAdvocates = await GetPrivateAdvocatesAsync()
            };
        }

        public Task<List<HearingCauseListRowViewModel>> GetRowsAsync(
            DateTime hearingDate, string benchTypeId, string causeListType, string courtCode, string createdBy)
        {
            return _repository.GetRowsAsync(hearingDate, benchTypeId, causeListType, courtCode, createdBy);
        }

        public async Task SaveRowsAsync(SaveHearingCauseListRequest request, string courtCode, string createdBy)
        {
            foreach (var row in request.Rows)
            {
                await _repository.SaveRowAsync(request, row, courtCode, createdBy);
            }
        }

        private async Task<List<SelectListItem>> GetPrivateAdvocatesAsync()
        {
            var privateAdvocates = await _advocateService.GetPrivateAdvocatesAsync();

            return privateAdvocates
                .Select(advocate => new SelectListItem
                {
                    Value = advocate.AdvEngHi ?? advocate.AdvName ?? advocate.AdvNameHi,
                    Text = advocate.AdvEngHi ?? advocate.AdvName ?? advocate.AdvNameHi
                })
                .ToList();
        }
    }
}