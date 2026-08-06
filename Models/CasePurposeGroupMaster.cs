using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GCMS.Models
{
    [Table("CASE_PURPOSE_GROUP_MAST")]
    public class CasePurposeGroupMaster
    {
        [Key]
        [Column("CASE_PURPOSE_GROUP_MASTID")]
        public long CasePurposeGroupMastId { get; set; }

        [Column("CASE_CODE")]
        public string? CaseCode { get; set; }

        [Column("CASE_PURPOSE_GROUP")]
        public string? CasePurposeGroup { get; set; }

        [Column("CASE_PURPOSE_GROUP_ENG")]
        public string? CasePurposeGroupEng { get; set; }

        [Column("DASHBOARDFLAG")]
        public string? DashboardFlag { get; set; }

        [Column("ORDER_LEVEL")]
        public int? OrderLevel { get; set; }

        [Column("RB_ID")]
        public string? RbId { get; set; }

        [Column("RB_CLPRIORITY")]
        public int? RbClPriority { get; set; }

        [Column("INACTIVE")]
        public string? InActive { get; set; }

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
    }
}