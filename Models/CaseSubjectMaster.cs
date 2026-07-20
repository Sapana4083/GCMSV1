using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GCMS.Models.Entities
{
    [Table("MAST_RCSAT_CSSUBJECT")]
    public class CaseSubjectMaster
    {
        [Key]
        [Column("MAST_RCSAT_CSSUBJECTID")]
        public long CaseSubjectId { get; set; }

        [Column("CANCEL")]
        public string? Cancel { get; set; }

        [Column("SUBJECT")]
        public string? Subject { get; set; }

        [Column("SUBJECTHI")]
        public string? SubjectHi { get; set; }

        [Column("INACTIVE")]
        public string? Inactive { get; set; }

        [Column("SUBJECTENGHI")]
        public string? SubjectEngHi { get; set; }
    }
}