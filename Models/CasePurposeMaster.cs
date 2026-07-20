using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GCMS.Models.Entities
{
    [Table("CASE_PURPOSE_MAST")]
    public class CasePurposeMaster
    {
        [Key]
        [Column("CASE_PURPOSE_MASTID")]
        public long CasePurposeMastId { get; set; }

        [Column("CANCEL")]
        public string? Cancel { get; set; }

        [Column("CASE_PURPOSE_GROUP")]
        public long? CasePurposeGroup { get; set; }

        [Column("CASE_PURPOSE_CODE")]
        public string? CasePurposeCode { get; set; }

        [Column("CASE_PURPOSE_NAME")]
        public string? CasePurposeName { get; set; }

        [Column("INACTIVE")]
        public string? Inactive { get; set; }

        [Column("CASE_PURPOSE_DESCRIPTION")]
        public string? CasePurposeDescription { get; set; }

        [Column("CASE_PURPOSE_ENG")]
        public string? CasePurposeEng { get; set; }
    }
}