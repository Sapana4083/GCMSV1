using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace GCMS.Models.Entities
{
    [Table("TRN_CASE_APPELLANT")]
    public class CaseAppellant
    {
        [Key]
        [Column("APPELLANT_ID")]
        public long AppellantId { get; set; }

        [Column("CASE_ID")]
        public long CaseId { get; set; }

        [Required]
        [Column("APPELLANT_NAME")]
        public string AppellantName { get; set; } = string.Empty;

        [Column("DESIGNATION")]
        public string? Designation { get; set; }

        [Column("DISTRICT_ID")]
        public long? DistrictId { get; set; }

        [Column("MOBILE_NO")]
        public string? MobileNo { get; set; }

        [Column("ADVOCATE_ID")]
        public long? AdvocateId { get; set; }

        [ForeignKey(nameof(CaseId))]
        public virtual CaseRegistration? CaseRegistration { get; set; }
    }
}
