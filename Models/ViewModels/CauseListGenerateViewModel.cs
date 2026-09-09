using System.ComponentModel.DataAnnotations;

namespace GCMS.Models.ViewModels
{
    public class CauseListGenerateViewModel
    {
        [Display(Name = "Doc ID")]
        public string? DocId { get; set; }

        [Display(Name = "Hearing Date")]
        [Required(ErrorMessage = "Hearing Date is required")]
        [DataType(DataType.Date)]
        public DateTime? HearingDate { get; set; }

        [Display(Name = "Cause List Type")]
        [Required(ErrorMessage = "Cause List Type is required")]
        public string? CauseListType { get; set; }

        [Display(Name = "Doc Date")]
        [Required(ErrorMessage = "Doc Date is required")]
        [DataType(DataType.Date)]
        public DateTime? DocDate { get; set; }

        [Display(Name = "Court Code")]
        public string? CourtCode { get; set; }
    }
}