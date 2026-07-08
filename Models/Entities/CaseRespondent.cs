using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace GCMS.Models.Entities
{
    [Table("TRN_CASE_RESPONDENT")]
    public class CaseRespondent
    {
        [Key]
        public long RespondentId { get; set; }

        public long CaseId { get; set; }

        public long DepartmentId { get; set; }

        public long? AdvocateId { get; set; }

        public string? Email { get; set; }

        public string? MobileNo { get; set; }

        [ForeignKey(nameof(CaseId))]
        public virtual CaseRegistration? CaseRegistration { get; set; }
    }
}
