using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GCMS.Models
{
    [Table("TEHSIL_MAST")]
    public class TehsilMaster
    {
        [Key]
        [Column("TEHSIL_MASTID")]
        public long TehsilMastId { get; set; }

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

        [Column("DISTRICT_NAME")]
        public long? DistNameId { get; set; }

        [Column("WFROLES")]
        public string? WfRoles { get; set; }

        [Column("STATE_NAME")]
        public long? StateId { get; set; }

        [Column("DISTRICT_NAME_OLD")]
        public long? DistIdOld { get; set; }

        [Column("SDO_NAME")]
        public long? SdoNameId { get; set; }

        [Column("TEHSIL_NAME")]
        public string? TehsilName { get; set; }

        [Column("TEHSIL_CODE")]
        public string? TehsilCode { get; set; }

        [Column("INACTIVE")]
        public string? Inactive { get; set; }

        [Column("ISAUTOCREATED")]
        public long? IsAutoCreated { get; set; }

        [Column("TEHSIL_ENG")]
        public string? TehsilEng { get; set; }

        [Column("GIS_TEHSILID")]
        public long? GisTehsilId { get; set; }

        [Column("APNAKHATA_ID")]
        public string? ApnaKhataId { get; set; }

        [Column("C_CODE")]
        public string? CCode { get; set; }

        [Column("PANCHAYAT_CODE")]
        public string? PanchayatCode { get; set; }

        [Column("RB_ID")]
        public string? RbId { get; set; }
    }
}