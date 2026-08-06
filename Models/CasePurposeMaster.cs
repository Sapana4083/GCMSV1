using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GCMS.Models
{
    [Table("CASE_PURPOSE_MAST")]
    public class CasePurposeMaster
    {
        [Key]
        [Column("CASE_PURPOSE_MASTID")]
        public long CasePurposeMastId { get; set; }

        [Column("CANCEL")]
        public string? Cancel { get; set; }

        [Column("SOURCEID")]
        public long? SourceId { get; set; }

        [Column("MAPNAME")]
        public string? MapName { get; set; }

        [Column("USERNAME")]
        public string? UserName { get; set; }

        [Column("MODIFIEDON")]
        public DateTime? ModifiedOn { get; set; }

        [Column("CREATEDBY")]
        public string? CreatedBy { get; set; }

        [Column("CREATEDON")]
        public DateTime? CreatedOn { get; set; }

        [Column("WKID")]
        public string? WkId { get; set; }

        [Column("APP_LEVEL")]
        public int? AppLevel { get; set; }

        [Column("APP_DESC")]
        public int? AppDesc { get; set; }

        [Column("APP_SLEVEL")]
        public int? AppSLevel { get; set; }

        [Column("CANCELREMARKS")]
        public string? CancelRemarks { get; set; }

        [Column("WFROLES")]
        public string? WfRoles { get; set; }

        [Column("CASE_PURPOSE_GROUP")]
        public long? CasePurposeGroup { get; set; }

        [Column("CASE_PURPOSE_CODE")]
        public string? CasePurposeCode { get; set; }

        [Column("CASE_PURPOSE_NAME")]
        public string? CasePurposeName { get; set; }

        [Column("INACTIVE")]
        public string InActive { get; set; } = "F";

        [Column("CASE_PURPOSE_DESCRIPTION")]
        public string? CasePurposeDescription { get; set; }

        [Column("DASHBOARDFLAG")]
        public string? DashboardFlag { get; set; }

        [Column("ISAUTOCREATED")]
        public int? IsAutoCreated { get; set; }

        [Column("CASE_PURPOSE_ENG")]
        public string? CasePurposeEng { get; set; }

        [Column("ORDER_LEVEL")]
        public int? OrderLevel { get; set; }

        [Column("COURTCODE")]
        public string? CourtCode { get; set; }

        [Column("DISP_ORDER")]
        public int? DispOrder { get; set; }

        [Column("PURPOSE_SUB_GROUP")]
        public string? PurposeSubGroup { get; set; }

        [Column("ISCOMPLETE")]
        public string? IsComplete { get; set; }

        [Column("RB_ID")]
        public int? RbId { get; set; }
    }
}