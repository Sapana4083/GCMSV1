using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace GCMS.Models
{

    [Table("ADVOCATE_MAST")]
    public class AdvocateMaster
    {
        [Key]
        [Column("ADVOCATE_MASTID")]
        public long AdvocateMastId { get; set; }

        [Column("ADVOCATE_NAME")]
        public string? AdvocateName { get; set; }

        [Column("EMAIL_ID")]
        public string? EmailId { get; set; }

        [Column("MOBILENO")]
        public long? MobileNo { get; set; }
        [Column("REG_NO")]
        public string? RegistrationNo { get; set; }

        [Column("COURT_CODE")]
        public string? CourtCode { get; set; }

        [Column("DISTRICT_NAME")]
        public long? DistrictId { get; set; }

        [Column("STATE_NAME")]
        public long? StateId { get; set; }

        [Column("COURT_NAME")]
        public long? CourtNameId { get; set; }

        [Column("INACTIVE")]
        public string? InActive { get; set; }

    }
}
