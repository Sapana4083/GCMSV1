using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace GCMS.Models.Entities
{
    [Table("TRN_CASE_REGISTRATION")]
    public class CaseRegistration
    {
        [Key]
        [Column("CASE_ID")]
        public long CaseId { get; set; }

        [Required]
        [Column("CASE_NUMBER")]
        [StringLength(50)]
        public string CaseNumber { get; set; } = string.Empty;

        [Required]
        [Column("INSTITUTION_DATE")]
        public DateTime InstitutionDate { get; set; }

        [Column("ORDER_ISSUED_BY_ID")]
        public long? OrderIssuedById { get; set; }

        [Required]
        [Column("CASE_TYPE_ID")]
        public long CaseTypeId { get; set; }

        [Required]
        [Column("CASE_PURPOSE_ID")]
        public long CasePurposeId { get; set; }

        [Required]
        [Column("CASE_SUBJECT_ID")]
        public long CaseSubjectId { get; set; }

        [Required]
        [Column("BENCH_TYPE_ID")]
        public long BenchTypeId { get; set; }

        [Column("HEARING_DATE")]
        public DateTime? HearingDate { get; set; }

        [Column("IMPUGNED_FLAG")]
        [StringLength(1)]
        public string? ImpugnedFlag { get; set; }

        [Column("IMPUGNED_DATE")]
        public DateTime? ImpugnedDate { get; set; }

        [Column("OLD_CASE_NUMBER")]
        [StringLength(50)]
        public string? OldCaseNumber { get; set; }

        [Column("LINKED_CASE_NUMBER")]
        [StringLength(50)]
        public string? LinkedCaseNumber { get; set; }

        [Column("STATUS")]
        [StringLength(20)]
        public string Status { get; set; } = "Draft";

        [Column("CREATED_BY")]
        public long CreatedBy { get; set; }

        [Column("CREATED_DATE")]
        public DateTime CreatedDate { get; set; } = DateTime.Now;

        [Column("UPDATED_BY")]
        public long? UpdatedBy { get; set; }

        [Column("UPDATED_DATE")]
        public DateTime? UpdatedDate { get; set; }

        // Navigation Properties
        public virtual ICollection<CaseAppellant> Appellants { get; set; }
            = new List<CaseAppellant>();

        public virtual ICollection<CaseRespondent> Respondents { get; set; }
            = new List<CaseRespondent>();

        public virtual ICollection<CasePrivateParty> PrivateParties { get; set; }
            = new List<CasePrivateParty>();

        public virtual CaseType? CaseType { get; set; }
        public virtual CaseSubject? CaseSubject { get; set; }
        public virtual CasePurpose? CasePurpose { get; set; }
        public virtual BenchType? BenchType { get; set; }
    }
}
