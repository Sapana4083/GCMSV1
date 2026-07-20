using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GCMS.Models.Entities
{
    [Table("TRN_RCSAT_CASEREG")]
    public class CaseRegistration
    {
        [Key]
        [Column("TRN_RCSAT_CASEREGID")]
        public long CaseId { get; set; }

        //========================
        // System Columns
        //========================

        [Column("CANCEL")]
        public string? Cancel { get; set; }

        [Column("SOURCEID")]
        public long? SourceId { get; set; }

        [Column("MAPNAME")]
        public string? MapName { get; set; }

        [Column("USERNAME")]
        public string? UserName { get; set; }

        [Column("CREATEDBY")]
        public string? CreatedBy { get; set; }

        [Column("CREATEDON")]
        public DateTime? CreatedOn { get; set; }

        [Column("MODIFIEDON")]
        public DateTime? ModifiedOn { get; set; }

        [Column("WKID")]
        public string? WorkFlowId { get; set; }

        [Column("APP_LEVEL")]
        public int? AppLevel { get; set; }

        [Column("APP_DESC")]
        public int? AppDesc { get; set; }

        [Column("APP_SLEVEL")]
        public int? AppSubLevel { get; set; }

        //========================
        // Step 1 : Basic Details
        //========================

        [Column("INSTITUTIONDATE")]
        public DateTime? InstitutionDate { get; set; }

        [Column("CASE_NO")]
        public string? CaseNo { get; set; }

        [Column("MCASE_NOO")]
        public string? ManualCaseNo { get; set; }

        [Column("ORDER_NO")]
        public string? OrderNo { get; set; }
        [Column("Impugned_Flag")]
        public string? ImpugnedFlag { get; set; }
        [Column("Impugned_Date")]
        public DateTime? ImpugnedDate { get; set; }

        [Column("DATE_OF_ORDER")]
        public DateTime? DateOfOrder { get; set; }

        [Column("DESIOFFORDER")]
        public long? OrderIssuedById { get; set; }

        [Column("COURT_CODE")]
        public string? CourtCode { get; set; }

        [Column("CASETYPE")]
        public long? CaseTypeId { get; set; }

        [Column("CASESUBJECT")]
        public long? CaseSubjectId { get; set; }

        [Column("CASE_TYPE_CODE")]
        public string? CaseTypeCode { get; set; }

        [Column("CASE_PURPOSE_NAME")]
        public long? CasePurposeId { get; set; }

        [Column("CASE_PURPOSE_CODE")]
        public string? CasePurposeCode { get; set; }

        [Column("CASE_PRESENTBY")]
        public string? CasePresentedBy { get; set; }

        [Column("HEARINGDATE")]
        public DateTime? HearingDate { get; set; }

        [Column("BENCH_TYPE")]
        public long? BenchTypeId { get; set; }

        [Column("LINKED_CASE")]
        public string? LinkedCase { get; set; }

        [Column("OLDCASNO")]
        public string? OldCaseNo { get; set; }

        //========================
        // Decision
        //========================

        [Column("CASE_DECISION_DATE")]
        public DateTime? DecisionDate { get; set; }

        [Column("DECISION_TYPE")]
        public long? DecisionTypeId { get; set; }

        //========================
        // Duplicate Check
        //========================

        [Column("DUPALLOW")]
        public string? DuplicateAllowed { get; set; }

        [Column("CNT_P")]
        public int? DuplicateCount { get; set; }

        //========================
        // Previous Case
        //========================

        [Column("PRVCASENO")]
        public string? PreviousCaseNo { get; set; }

        [Column("LCASNO")]
        public string? LowerCourtCaseNo { get; set; }

        [Column("PRECASENOCHK")]
        public string? PreviousCaseCheck { get; set; }

        //========================
        // UI Only
        //========================

        [NotMapped]
        public int CurrentStep { get; set; }

        [NotMapped]
        public bool IsDraft { get; set; }

        //========================
        // Navigation Properties
        //========================

        public virtual ICollection<CaseAppellant>? Appellants { get; set; }

        public virtual ICollection<CaseRespondent>? Respondents { get; set; }

        public virtual ICollection<CasePrivateParty>? PrivateParties { get; set; }
    }
}