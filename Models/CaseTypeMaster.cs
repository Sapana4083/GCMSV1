using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GCMS.Models;

[Table("CASE_TYPE_MAST")]
public class CaseTypeMaster
{
    [Key]
    [Column("CASE_TYPE_MASTID")]
    public long CaseTypeMastId { get; set; }

    [Column("CASE_CODE")]
    [Required(ErrorMessage = "Case Code is required")]
    [StringLength(10)]
    public string? CaseCode { get; set; }

    [Column("CASE_TYPE")]
    [Required(ErrorMessage = "Case Type is required")]
    [StringLength(100)]
    public string? CaseType { get; set; }

    [Column("CASE_TYPE_ENG")]
    [Required(ErrorMessage = "Case Type (English) is required")]
    [StringLength(100)]
    public string? CaseTypeEng { get; set; }

    [Column("ORDER_LEVEL")]
    [Required(ErrorMessage = "Order Level is required")]
    public long? OrderLevel { get; set; }

    [Column("CTYPE_ABBR")]
    [StringLength(100)]
    public string? CtypeAbbr { get; set; }

    [Column("CASE_GROUP_CODE")]
    [StringLength(10)]
    public string? CaseGroupCode { get; set; }

    [Column("CASE_TYPE_GROUP")]
    [StringLength(300)]
    public string? CaseTypeGroup { get; set; }

    [Column("CASE_GROUP")]
    [StringLength(100)]
    public string? CaseGroup { get; set; }

    [Column("RB_ID")]
    public long? RbId { get; set; }

    [Column("CASE_TYPE_CATID")]
    public long? CaseTypeCatId { get; set; }

    [Column("TAX_ORDER_LEVEL")]
    public int? TaxOrderLevel { get; set; }

    [Column("SHORT_NAME")]
    [StringLength(50)]
    public string? ShortName { get; set; }

    [Column("DISP_ORDER")]
    public long? DispOrder { get; set; }

    [Column("CANCEL")]
    public string? Cancel { get; set; }

    [Column("INACTIVE")]
    public string? InActive { get; set; }

    [Column("CREATEDBY")]
    public string? CreatedBy { get; set; }

    [Column("CREATEDON")]
    public DateTime? CreatedOn { get; set; }

    [Column("MODIFIEDON")]
    public DateTime? ModifiedOn { get; set; }
}