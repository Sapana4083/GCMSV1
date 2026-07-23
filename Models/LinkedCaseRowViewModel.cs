using System;

namespace GCMS.Models   // <-- match this to your project's actual Models namespace
{
    public class LinkedCaseRowViewModel
    {
        public long? CaseId { get; set; }            // TRN_RCSAT_CASEID (PK)
        public long? CaseUpdateId { get; set; }
        public string CourtCode { get; set; }
        public string CaseTypee { get; set; }         // CASE_TYPEE
        public string CaseNo { get; set; }            // CASE_NO (child case)
        public string CalCase { get; set; }           // CALCASE
        public string MainCase { get; set; }          // MINCASE (parent/link case)
        public DateTime? InstitutionDate { get; set; } // INSDT
        public DateTime? HearingDate { get; set; }    // HDT
        public string AppellantName { get; set; }     // APPNAME
        public string RespondentName { get; set; }    // RESPNAME
        public string CType { get; set; }             // CTYPE
        public string Purpose { get; set; }
        public string DistrictName { get; set; }      // DNAME
        public string AppellantAdvocate { get; set; } // APPADV
        public string RespondentAdvocate { get; set; } // RESPAD
        public string Connected { get; set; }         // CONECT
    }
}