using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace GCMS.Models.ViewModels
{
    public class HearingCauseListViewModel
    {
        [Display(Name = "Hearing Date")]
        [DataType(DataType.Date)]
        public DateTime HearingDate { get; set; } = DateTime.Today;

        [Display(Name = "Bench No")]
        public long? BenchTypeId { get; set; }

        [Display(Name = "Bench No")]
        public string? BenchNo { get; set; }

        [Display(Name = "Cause List Type")]
        public string CauseListType { get; set; } = "Regular";

        public IEnumerable<SelectListItem> BenchTypes { get; set; } = new List<SelectListItem>();

        public IEnumerable<SelectListItem> CauseListTypes { get; set; } = new List<SelectListItem>();

        public IEnumerable<SelectListItem> PrivateAdvocates { get; set; } = new List<SelectListItem>();

        public List<HearingCauseListRowViewModel> Rows { get; set; } = new();
    }

    public class HearingCauseListRowViewModel
    {
        public long CauseListId { get; set; }

        public long TcsatMultHerId { get; set; }

        public long CaseId { get; set; }

        public int SerialNo { get; set; }

        public string CaseNumber { get; set; } = string.Empty;

        public string? ParentCaseNumber { get; set; }

        public string? Purpose { get; set; }

        public DateTime? HearingDate { get; set; }

        public string? AppellantName { get; set; }

        public string? AdvocateName { get; set; }

        public DateTime? StayDate { get; set; }

        public DateTime? ReplyDate { get; set; }

        public DateTime? PurchaseDate { get; set; }

        public bool IsStay { get; set; }

        public bool IsReply { get; set; }

        public DateTime? LastUpdatedDate { get; set; }

        public string? Name { get; set; }

        public string? Designation { get; set; }

        public long? PrivateAdvocateId { get; set; }

        public string? PrivateAdvocateName { get; set; }
    }

    public class SaveHearingCauseListRequest
    {
        public DateTime? HearingDate { get; set; }

        public string? BenchNo { get; set; }

        public string? CauseListType { get; set; }

        public List<SaveHearingCauseListRowRequest> Rows { get; set; } = new();
    }

    public class SaveHearingCauseListRowRequest
    {
        public long CauseListId { get; set; }

        public long TcsatMultHerId { get; set; }

        public long CaseId { get; set; }

        public DateTime? HearingDate { get; set; }

        public DateTime? StayDate { get; set; }

        public DateTime? ReplyDate { get; set; }

        public DateTime? PurchaseDate { get; set; }

        public bool IsStay { get; set; }

        public bool IsReply { get; set; }

        public long? PrivateAdvocateId { get; set; }
    }
}
