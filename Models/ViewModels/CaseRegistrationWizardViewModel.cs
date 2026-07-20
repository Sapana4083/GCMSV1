using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace GCMS.Models.ViewModels
{
    public class CaseRegistrationWizardViewModel
    {
        public long Id { get; set; }

        #region ===== STEP 1 : BASIC DETAILS =====

        [Display(Name = "Institution Date")]
        [Required(ErrorMessage = "Institution Date is required")]
        [DataType(DataType.Date)]
        public DateTime? InstitutionDate { get; set; }

        [Display(Name = "Case Number")]
        public string? CaseNumber { get; set; }

        [Display(Name = "Manual Case Number")]
        public string? ManualCaseNumber { get; set; }

        [Display(Name = "Order Number")]
        public string? OrderNumber { get; set; }

        [Display(Name = "Date Of Order")]
        [DataType(DataType.Date)]
        public DateTime? DateOfOrder { get; set; }

        [Display(Name = "Order Issued By")]
        public long? OrderIssuedById { get; set; }

        [Display(Name = "Court Code")]
        public string? CourtCode { get; set; }

        public DateTime? ImpugnedDate { get; set; }

        [Display(Name = "Case Type")]
        [Required]
        public long? CaseTypeId { get; set; }

        [Display(Name = "Case Subject")]
        [Required]
        public long? CaseSubjectId { get; set; }

        [Display(Name = "Case Purpose")]
        [Required]
        public long? CasePurposeId { get; set; }

        [Display(Name = "Case Presented By")]
        public string? CasePresentedBy { get; set; }

        [Display(Name = "Next Hearing Date")]
        [DataType(DataType.Date)]
        public DateTime? HearingDate { get; set; }

        [Display(Name = "Bench Type")]
        public long? BenchTypeId { get; set; }

        [Display(Name = "Linked Case")]
        public string? LinkedCaseNumber { get; set; }

        [Display(Name = "Old Case Number")]
        public string? OldCaseNumber { get; set; }

        #endregion

        #region ===== STEP 2 : APPELLANT =====

        [Display(Name = "Appellant Name")]
        public string? AppellantName { get; set; }

        [Display(Name = "Designation")]
        public long? DesignationId { get; set; }

        [Display(Name = "District")]
        public long? DistrictId { get; set; }

        [Display(Name = "Mobile Number")]
        public long? MobileNumber { get; set; }

        [Display(Name = "Employee ID")]
        public string? EmployeeId { get; set; }

        [Display(Name = "Appellant Advocate")]
        public long? AdvocateId { get; set; }

        [Display(Name = "Advocate Email")]
        [EmailAddress]
        public string? AdvocateEmail { get; set; }

        [Display(Name = "Advocate Mobile")]
        public long? AdvocateMobile { get; set; }

        #endregion

        #region ===== STEP 3 : RESPONDENT =====

        [Display(Name = "Respondent Department")]
        public long? DepartmentId { get; set; }

        [Display(Name = "Respondent Advocate")]
        public long? RespondentAdvocateId { get; set; }

        [Display(Name = "Respondent Advocate Email")]
        [EmailAddress]
        public string? RespondentAdvocateEmail { get; set; }

        [Display(Name = "Respondent Advocate Mobile")]
        public long? RespondentAdvocateMobile { get; set; }

        #endregion

        #region ===== STEP 4 : PRIVATE PARTY =====

        [Display(Name = "Private Party Name")]
        public string? PrivatePartyName { get; set; }

        [Display(Name = "Private Designation")]
        public string? PrivateDesignation { get; set; }

        [Display(Name = "Private Advocate")]
        public string? PrivateAdvocateName { get; set; }

        #endregion

        #region ===== COMMON =====

        public int CurrentStep { get; set; } = 1;

        public bool IsDraft { get; set; }

        #endregion

        #region ===== DROPDOWNS =====

        public IEnumerable<SelectListItem> CaseTypes { get; set; } = new List<SelectListItem>();

        public IEnumerable<SelectListItem> CaseSubjects { get; set; } = new List<SelectListItem>();

        public IEnumerable<SelectListItem> CasePurposes { get; set; } = new List<SelectListItem>();

        public IEnumerable<SelectListItem> BenchTypes { get; set; } = new List<SelectListItem>();

        public IEnumerable<SelectListItem> OrderIssuedByList { get; set; } = new List<SelectListItem>();

        public IEnumerable<SelectListItem> Designations { get; set; } = new List<SelectListItem>();

        public IEnumerable<SelectListItem> Districts { get; set; } = new List<SelectListItem>();

        public IEnumerable<SelectListItem> Advocates { get; set; } = new List<SelectListItem>();

        public IEnumerable<SelectListItem> Departments { get; set; } = new List<SelectListItem>();

        #endregion
    }
}