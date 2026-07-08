using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace GCMS.Models.ViewModels
{
    public class CaseRegistrationWizardViewModel
    {      
            public long Id { get; set; }
            public int Step { get; set; } = 1;

            [Required, DataType(DataType.Date), Display(Name = "Institution Date")]
            public DateTime? InstitutionDate { get; set; } = DateTime.Today;

            [Required, StringLength(15), Display(Name = "Case Number")]
            public string CaseNumber { get; set; } = string.Empty;

            [Required, Display(Name = "Impugned Flag")]
            public string? ImpugnedFlag { get; set; }

            [DataType(DataType.Date), Display(Name = "Impugned Date")]
            public DateTime? ImpugnedDate { get; set; }

            [Display(Name = "Order Issued By")]
            public long? OrderIssuedById { get; set; }

            [Required, Display(Name = "Case Type")]
            public long? CaseTypeId { get; set; }

            [StringLength(100), Display(Name = "Old Case Number")]
            public string? OldCaseNumber { get; set; }

            [Required, Display(Name = "Case Subject")]
            public long? CaseSubjectId { get; set; }

            [Required, Display(Name = "Case Purpose")]
            public long? CasePurposeId { get; set; }

            [Required, DataType(DataType.Date), Display(Name = "Hearing Date")]
            public DateTime? HearingDate { get; set; }

            [Required, Display(Name = "Bench Type")]
            public long? BenchTypeId { get; set; }

            [StringLength(50), Display(Name = "Linked Case Number")]
            public string? LinkedCaseNumber { get; set; }

            [Required, StringLength(300), Display(Name = "Appellant Name")]
            public string AppellantName { get; set; } = string.Empty;

            [StringLength(1000)]
            public string? Designation { get; set; }

            [StringLength(100)]
            public string? District { get; set; }

            [RegularExpression(@"^[0-9]{10}$", ErrorMessage = "Enter a valid 10 digit mobile number."), Display(Name = "Mobile Number")]
            public string? MobileNumber { get; set; }

            [Display(Name = "Advocate Name")]
            public long? AdvocateId { get; set; }

            [EmailAddress, StringLength(200), Display(Name = "Advocate Email")]
            public string? AdvocateEmail { get; set; }

            [RegularExpression(@"^[0-9]{10}$", ErrorMessage = "Enter a valid 10 digit mobile number."), Display(Name = "Advocate Mobile")]
            public string? AdvocateMobile { get; set; }

            [Required, Display(Name = "Department")]
            public long? DepartmentId { get; set; }

            [Display(Name = "Respondent Advocate")]
            public long? RespondentAdvocateId { get; set; }

            [EmailAddress, StringLength(100), Display(Name = "Advocate Email")]
            public string? RespondentAdvocateEmail { get; set; }

            [RegularExpression(@"^[0-9]{10}$", ErrorMessage = "Enter a valid 10 digit mobile number."), Display(Name = "Advocate Mobile")]
            public string? RespondentAdvocateMobile { get; set; }

            [StringLength(300), Display(Name = "Private Party Name")]
            public string? PrivatePartyName { get; set; }

            [StringLength(100), Display(Name = "Designation")]
            public string? PrivateDesignation { get; set; }

            [StringLength(10), Display(Name = "Private Advocate Name")]
            public string? PrivateAdvocateName { get; set; }

            public IEnumerable<SelectListItem> CaseTypes { get; set; } = Array.Empty<SelectListItem>();
            public IEnumerable<SelectListItem> CasePurposes { get; set; } = Array.Empty<SelectListItem>();
            public IEnumerable<SelectListItem> CaseSubjects { get; set; } = Array.Empty<SelectListItem>();
            public IEnumerable<SelectListItem> BenchTypes { get; set; } = Array.Empty<SelectListItem>();
            public IEnumerable<SelectListItem> Departments { get; set; } = Array.Empty<SelectListItem>();
    }
}
