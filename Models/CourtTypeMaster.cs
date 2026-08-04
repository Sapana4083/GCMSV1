using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GCMS.Models
{
    [Table("COURT_TYPE_MAST")]
    public class CourtTypeMaster
    {
        [Key]
        [Column("COURT_TYPE_MASTID")]
        public long CourtTypeMastId { get; set; }

        [Column("COURT_TYPE")]
        public string? CourtType { get; set; }

        [Column("COURT_TYPE_NAME")]
        public string? CourtTypeName { get; set; }

       

        public long? CourtGroupId { get; set; }

        [Column("COURTDETAIL")]
        public string? CourtDetail { get; set; }

        [Column("COURT_GROUP_CODE")]
        public string? CourtGroupCode { get; set; }

        [Column("COURT_CATEGORY")]
        public long? CourtCategory { get; set; }

        [Column("DEPARTID")]
        public long? DepartId { get; set; }

        [Column("HIERARCHY_LEVEL")]
        public int? HierarchyLevel { get; set; }

        [Column("DISP_ORDER")]
        public int? DispOrder { get; set; }

        [Column("INACTIVE")]
        public string? InActive { get; set; }

        [Column("CASE_GROUP")]
        public string? CaseGroup { get; set; }

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

        [Column("SOURCEID")]
        public long? SourceId { get; set; }

        [Column("MAPNAME")]
        public string? MapName { get; set; }

        [Column("USERNAME")]
        public string? UserName { get; set; }

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
    }

    public class LovModel
    {
        public long Id { get; set; }
        public string? Name { get; set; }
    }
}