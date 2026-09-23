using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace GCMS.Models.ViewModels
{
    public class CaseFeedbackViewModel
    {
        [Required(ErrorMessage = "Case Type is required")]
        [Display(Name = "Case Type")]
        public long? CaseTypeId { get; set; }

        [Required(ErrorMessage = "Case No is required")]
        [Display(Name = "Case No")]
        public string? CaseNo { get; set; }

        public long? CaseRegId { get; set; }

        [Display(Name = "Stay Date")]
        [DataType(DataType.Date)]
        public DateTime StayDate { get; set; } = DateTime.Today;

        [Display(Name = "Stay Check")]
        public bool StayCheck { get; set; }

        [Display(Name = "Case Details")]
        public string? CaseDetails { get; set; }

        [Display(Name = "Remark")]
        public string? Remark { get; set; }

        [Required(ErrorMessage = "Revise Case Purpose is required")]
        [Display(Name = "Revise Case Purpose")]
        public long? ReviseCasePurposeId { get; set; }

        [Required(ErrorMessage = "Next Hearing Date is required")]
        [Display(Name = "Next Hearing Date")]
        [DataType(DataType.Date)]
        public DateTime? NextHearingDate { get; set; }

        [Display(Name = "Upload File")]
        public List<IFormFile> UploadFiles { get; set; } = new();

        // ── Controller-populated (file save ke baad) — form input nahi ──
        public string? SavedFileName { get; set; }

        public string? SavedFilePath { get; set; }
    }
}