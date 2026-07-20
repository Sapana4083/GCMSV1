using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GCMS.Models.Entities
{
    [Table("TRN_RCSAT_RESPONDENT")]
    public class CaseRespondent
    {
        [Key]
        [Column("TRN_RCSAT_RESPONDENTID")]
        public long RespondentId { get; set; }

        [Column("TRN_RCSAT_CASEREGID")]
        public long CaseId { get; set; }

        [Column("RESPONDENT_DEPARTMENT")]
        public long? DepartmentId { get; set; }

        [Column("RESP_ADVOCATE")]
        public long? AdvocateId { get; set; }

        [Column("RESP_ADVEMAIL")]
        public string? AdvocateEmail { get; set; }

        [Column("RESP_ADVMOBILE")]
        public long? AdvocateMobile { get; set; }

        public string? Designation { get; set; }

        [Column("OICNAME")]
        public string? OICName { get; set; }

        [Column("OICMOBILENO")]
        public string? OICMobileNo { get; set; }
        public string? CreatedBy { get; set; }
    }
}