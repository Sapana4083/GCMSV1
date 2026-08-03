using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GCMS.Models
{
    [Table("COURT_GROUP_MASTER")]
    public class CourtGroupMaster
    {
        [Key]
        [Column("COURT_GROUPID")]
        public long CourtGroupId { get; set; }

        [Column("COURT_GROUP")]
        public string? CourtGroup { get; set; }

        [Column("COURT_GROUP_CODE")]
        public string? CourtGroupCode { get; set; }

        [Column("CANCEL")]
        public string? Cancel { get; set; }

        [Column("CREATEDBY")]
        public string? CreatedBy { get; set; }

        [Column("CREATEDON")]
        public DateTime? CreatedOn { get; set; }

        [Column("MODIFIEDBY")]
        public string? ModifiedBy { get; set; }

        [Column("MODIFIEDON")]
        public DateTime? ModifiedOn { get; set; }

        [Column("INACTIVE")]
        public int InActive { get; set; }
    }
}