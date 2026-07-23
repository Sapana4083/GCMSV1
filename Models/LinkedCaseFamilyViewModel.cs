namespace GCMS.Models
{
    public class LinkedCaseFamilyViewModel
    {
        public long CaseRegId { get; set; }          // TRN_RCSAT_CASEREGID
        public string CaseNo { get; set; }
        public string ConnectedCaseNo { get; set; }   // LINKED_CASE
        public DateTime? HearingDate { get; set; }
        public string CasePurposeMastId { get; set; }
        public string CasePurposeName { get; set; }   // PURPOSEENGHI
        public string PrimaryAppellant { get; set; }  // APPELLANT_NAMEE
        public string PrimaryRespondent { get; set; } // DEPT_NAMEHI
        public string DistrictName { get; set; }
    }
}
