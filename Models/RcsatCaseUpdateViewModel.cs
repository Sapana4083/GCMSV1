using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace GCMS.Models
{
    public class RcsatCaseUpdateViewModel
    {
        public long TotalCount { get; set; }
        public long? CaseUpdateId { get; set; }   // TRN_RCSAT_CASEUPDATEID - PK
        //public string CourtName { get; set; }
        public string CourtCode { get; set; }
        public string CaseType { get; set; }
       // public string LinkCase { get; set; }
        public string ParentCaseNo { get; set; }
       // public string ParentChildChk { get; set; }
       // public string ConnectedCaseNo { get; set; }
        public string AppellantName { get; set; }
        public string RespondentName { get; set; }
        public DateTime? InstitutionDate { get; set; }
        public DateTime? HearingDate { get; set; }
        public string LinkCaseNo { get; set; }
        public string District { get; set; }
        public string PurposeName { get; set; }
        public string PurposeId { get; set; }

        public string ChildCase { get; set; }

        public string TRN_RCSAT_CASEREGID { get; set; }

        [Display(Name = "Case Type")]
        [Required]
        public long? CaseTypeId { get; set; }
        //public string SubCaseType { get; set; }

        public List<LinkedCaseRowViewModel> LinkedCases { get; set; } = new();
        public IEnumerable<SelectListItem> CaseTypes { get; set; } = new List<SelectListItem>();

    }

   
    public class LinkedCaseFamilyViewModel
    {
        public long CaseRegId { get; set; }
        //public string CaseNo { get; set; }
        //public string ConnectedCaseNo { get; set; }
        //public DateTime? HearingDate { get; set; }
        //public string CasePurposeMastId { get; set; }
        //public string CasePurposeName { get; set; }
        //public string PrimaryAppellant { get; set; }
        //public string PrimaryRespondent { get; set; }
        //public string DistrictName { get; set; }


        public long? CaseUpdateId { get; set; }   // TRN_RCSAT_CASEUPDATEID - PK
        public string TRN_RCSAT_CASEREGID { get; set; }
        public string CourtCode { get; set; }
        public string CaseType { get; set; }
         public string ChildCase { get; set; }
        public string ParentCaseNo { get; set; }
        // public string ParentChildChk { get; set; }
        // public string ConnectedCaseNo { get; set; }
        public string AppellantName { get; set; }
        public string RespondentName { get; set; }
        public DateTime? InstitutionDate { get; set; }
        public DateTime? HearingDate { get; set; }
        public string LinkCaseNo { get; set; }
        public string District { get; set; }
        public string PurposeName { get; set; }
        public string PurposeId { get; set; }

        [Display(Name = "Case Type")]
        [Required]
        public long? CaseTypeId { get; set; }
        //public string SubCaseType { get; set; }

        public List<LinkedCaseRowViewModel> LinkedCases { get; set; } = new();
        public IEnumerable<SelectListItem> CaseTypes { get; set; } = new List<SelectListItem>();
    }
}