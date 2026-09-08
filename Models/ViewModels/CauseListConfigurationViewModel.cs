using Microsoft.AspNetCore.Mvc.Rendering;

namespace GCMS.Models.ViewModels
{
    public class CauseListConfigurationViewModel
    {
        public int? CLPNo { get; set; }

        public string? Remarks { get; set; }

        public string? CauseListType { get; set; }

        public int? CaseCountLimit { get; set; }

        public bool Active { get; set; } = true;

        public List<SelectListItem> CauseListTypes { get; set; }
            = new();

        // All Case Purpose dropdown values
        public List<SelectListItem> CasePurposeList { get; set; }
            = new();

        // Grid rows
        public List<CauseListCasePurposeViewModel> CasePurposes { get; set; }
            = new();

        public List<int> SelectedIds { get; set; }
            = new();
    }


    public class CauseListCasePurposeViewModel
    {
        public int Id { get; set; }

        // Selected Case Purpose
        public long? CasePurposeId { get; set; }

        public int PurposePriority { get; set; }

        public int DBBenchOne { get; set; }

        public int DBBenchTwo { get; set; }
    }
}
