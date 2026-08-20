using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GCMS.Models;

[Table("CM_RCSAT_DESIGN_TMP")]
public class DesignationMaster
{
    [Key]
    [Column("CM_RCSAT_DESIGN_TMPID")]
    public long CmRcsatDesignTmpId { get; set; }

    [Column("CANCEL")]
    public string? Cancel { get; set; }

    [Column("DESG_NAME")]
    [Required(ErrorMessage = "Designation Name is required")]
    [StringLength(100)]
    public string? DesgName { get; set; }

    [Column("DESG_NAMEHI")]
    [Required(ErrorMessage = "Designation Name Hindi is required")]
    [StringLength(100)]
    public string? DesgNameHi { get; set; }

    [Column("DESGENGHI")]
    public string? DesgEngHi { get; set; }

    [Column("DESG_CODE")]
    [StringLength(10)]
    public string? DesgCode { get; set; }

    [Column("INACTIVE")]
    public string? InActive { get; set; }

    [Column("INCATIVE")]
    public string? InCative { get; set; }

    [Column("CREATEDBY")]
    public string? CreatedBy { get; set; }

    [Column("CREATEDON")]
    public DateTime? CreatedOn { get; set; }

    [Column("MODIFIEDON")]
    public DateTime? ModifiedOn { get; set; }
}