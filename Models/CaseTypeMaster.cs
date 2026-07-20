using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace GCMS.Models
{
        [Table("CASE_TYPE_MAST")]
        public class CaseTypeMaster
        {
            [Key]
            [Column("CASE_TYPE_MASTID")]
            public long CaseTypeMastId { get; set; }

            [Column("CANCEL")]
            public string? Cancel { get; set; }

            [Column("INACTIVE")]
            public string? Inactive { get; set; }

            [Column("CASE_TYPE")]
            public string? CaseType { get; set; }

            [Column("CASE_CODE")]
            public string? CaseCode { get; set; }

            [Column("CASE_TYPE_ENG")]
            public string? CaseTypeEng { get; set; }

            [Column("CASE_TYPE_CATID")]
            public long? CaseTypeCatId { get; set; }

            [Column("CASE_GROUP_CODE")]
            public string? CaseGroupCode { get; set; }

            [Column("CASE_TYPE_GROUP")]
            public string? CaseTypeGroup { get; set; }

            [Column("CASE_GROUP")]
            public string? CaseGroup { get; set; }

        }
}

