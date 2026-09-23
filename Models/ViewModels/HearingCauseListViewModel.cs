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

        public IEnumerable<SelectListItem> BenchTypes { get; set; }
            = new List<SelectListItem>();

        public IEnumerable<SelectListItem> CauseListTypes { get; set; }
            = new List<SelectListItem>();

        public IEnumerable<SelectListItem> PrivateAdvocates { get; set; }
            = new List<SelectListItem>();

        public List<HearingCauseListRowViewModel> Rows { get; set; }
            = new();
    }


    public class HearingCauseListRowViewModel
    {
        // ==========================================
        // PRIMARY IDS
        // ==========================================

        /// <summary>
        /// CAUSE_LIST.CAUSE_LISTID
        /// Required for UPDATE.
        /// </summary>
        public long CauseListId { get; set; }

        /// <summary>
        /// TCSAT_MULT_HER.TCSAT_MULT_HERID
        /// Required for UPDATE.
        /// </summary>
        public long TcsatMultHerId { get; set; }

        /// <summary>
        /// CAUSE_LIST.CASE_REGID
        /// </summary>
        public long CaseId { get; set; }


        // ==========================================
        // CASE INFORMATION
        // ==========================================

        /// <summary>
        /// CAUSE_LIST.CAUSE_LISTROW
        /// </summary>
        public int SerialNo { get; set; }

        /// <summary>
        /// CAUSE_LIST.CASE_NO
        /// </summary>
        public string CaseNumber { get; set; } = string.Empty;

        /// <summary>
        /// CAUSE_LIST.LINKED_CASENO
        /// </summary>
        public string? ParentCaseNumber { get; set; }

        /// <summary>
        /// CAUSE_LIST.PURPOSE
        /// </summary>
        public string? Purpose { get; set; }


        // ==========================================
        // HEARING INFORMATION
        // ==========================================

        /// <summary>
        /// CAUSE_LIST.H_DATE
        /// </summary>
        public DateTime? HearingDate { get; set; }

        /// <summary>
        /// CAUSE_LIST.APPELLANT_NAMEE
        /// </summary>
        public string? AppellantName { get; set; }

        /// <summary>
        /// CAUSE_LIST.ADVOCATE
        /// </summary>
        public string? AdvocateName { get; set; }


        // ==========================================
        // STAY / REPLY / PURCHASE
        // ==========================================

        /// <summary>
        /// CAUSE_LIST.S_DATE
        /// </summary>
        public DateTime? StayDate { get; set; }

        /// <summary>
        /// CAUSE_LIST.R_DATE
        /// </summary>
        public DateTime? ReplyDate { get; set; }

        /// <summary>
        /// CAUSE_LIST.CDT
        /// </summary>
        public DateTime? PurchaseDate { get; set; }

        /// <summary>
        /// CAUSE_LIST.LSSTAY
        /// </summary>
        public bool IsStay { get; set; }

        /// <summary>
        /// CAUSE_LIST.LSREPLY
        /// </summary>
        public bool IsReply { get; set; }


        // ==========================================
        // RESPONDENT INFORMATION
        // ==========================================

        /// <summary>
        /// RESPONDENT_NAME
        /// </summary>
        public string? Name { get; set; }

        /// <summary>
        /// RESPONDENT_DESIGNATION
        /// </summary>
        public string? Designation { get; set; }

        /// <summary>
        /// Private Advocate Name — ab dropdown me naam hi ID/Value dono ke roop me use hota hai
        /// </summary>
        public string? PrivateAdvocateName { get; set; }


        // ==========================================
        // MULTIPLE HEARING INFORMATION
        // ==========================================

        /// <summary>
        /// TCSAT_MULT_HER.HDATE
        /// </summary>
        public DateTime? MultipleHearingDate { get; set; }

        /// <summary>
        /// TCSAT_MULT_HER.BENCH_NO
        /// </summary>
        [Display(Name = "Bench Number")]
        public string? BenchNumber { get; set; }

        /// <summary>
        /// TCSAT_MULT_HER.CL_TYPE
        /// </summary>
        [Display(Name = "Cause List Type")]
        public string? ClType { get; set; }


        // ==========================================
        // OTHER INFORMATION
        // ==========================================

        [Display(Name = "Last Updated Date")]
        public DateTime? LastUpdatedDate { get; set; }

        [Display(Name = "Court Code")]
        public string? CourtCode { get; set; }

        [Display(Name = "Priority")]
        public int? Priority { get; set; }

        [Display(Name = "Connected Case No")]
        public string? ConnectedCaseNo { get; set; }
    }


    // ==============================================
    // SAVE REQUEST
    // ==============================================

    public class SaveHearingCauseListRequest
    {
        public DateTime? HearingDate { get; set; }

        public string? BenchNo { get; set; }

        public string? CauseListType { get; set; }

        public List<SaveHearingCauseListRowRequest> Rows { get; set; }
            = new();
    }


    public class SaveHearingCauseListRowRequest
    {
        // ==========================================
        // IDS
        // ==========================================

        /// <summary>
        /// CAUSE_LIST.CAUSE_LISTID
        /// </summary>
        public long CauseListId { get; set; }

        /// <summary>
        /// TCSAT_MULT_HER.TCSAT_MULT_HERID
        /// </summary>
        public long TcsatMultHerId { get; set; }

        /// <summary>
        /// CAUSE_LIST.CASE_REGID
        /// </summary>
        public long CaseId { get; set; }


        // ==========================================
        // HEARING
        // ==========================================

        public DateTime? HearingDate { get; set; }


        // ==========================================
        // STAY / REPLY / PURCHASE
        // ==========================================

        public DateTime? StayDate { get; set; }

        public DateTime? ReplyDate { get; set; }

        public DateTime? PurchaseDate { get; set; }

        public bool IsStay { get; set; }

        public bool IsReply { get; set; }


        // ==========================================
        // ADVOCATE
        // ==========================================

        /// <summary>
        /// Private advocate ka naam (dropdown ka value ab ID nahi, naam hai).
        /// Repository mein p_advocate ko ye value jayegi.
        /// </summary>
        public string? PrivateAdvocateName { get; set; }
    }
}