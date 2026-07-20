using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GCMS.Models
{
    [Table("SDO_MAST")]
    public class SdoMaster
    {
        [Key]
        [Column("SDO_MASTID")]
        public long SdoMastId { get; set; }

        [Column("CANCEL")]
        public string? Cancel { get; set; }

        [Column("SOURCEID")]
        public long? SourceId { get; set; }

        [Column("MAPNAME")]
        public string? MapName { get; set; }

        [Column("USERNAME")]
        public string? Username { get; set; }

        [Column("MODIFIEDON")]
        public DateTime? ModifiedOn { get; set; }

        [Column("CREATEDBY")]
        public string? CreatedBy { get; set; }

        [Column("CREATEDON")]
        public DateTime? CreatedOn { get; set; }

        [Column("WKID")]
        public string? Wkid { get; set; }

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

        [Column("DISTRICT_NAME")]
        public long? DistrictMastId { get; set; }

        [Column("SDO_NAME")]
        public string? SdoName { get; set; }

        [Column("SDO_CODE")]
        public string? SdoCode { get; set; }

        [Column("INACTIVE")]
        public string? Inactive { get; set; }

        [Column("SDO_NAME_ENG")]
        public string? SdoNameEng { get; set; }

        [Column("PCODE")]
        public string? PCode { get; set; }

        [Column("DISTRICT_NAME1")]
        public string? DistrictName1 { get; set; }
    }
}