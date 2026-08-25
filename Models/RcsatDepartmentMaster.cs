using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GCMS.Models;

[Table("CM_RCSAT_DEPT")]
public class RcsatDepartmentMaster
{
    [Key]
    [Column("CM_RCSAT_DEPTID")]
    public long CmRcsatDeptId { get; set; }

    [Column("CANCEL")]
    public string? Cancel { get; set; }

    [Column("DEPT_NAME")]
    [Required(ErrorMessage = "Department Name in English is required")]
    [StringLength(200)]
    public string? DeptName { get; set; }

    [Column("DEPT_NAMEHI")]
    [StringLength(500)]
    public string? DeptNameHi { get; set; }

    [Column("DEPT_CODE")]
    [StringLength(100)]
    public string? DeptCode { get; set; }

    [Column("INACTIVE")]
    public string? InActive { get; set; }

    [Column("DEPENGHI")]
    [StringLength(1000)]
    public string? DepEngHi { get; set; }

    [Column("EMAIL")]
    [StringLength(200)]
    public string? Email { get; set; }

    [Column("CREATEDBY")]
    public string? CreatedBy { get; set; }

    [Column("CREATEDON")]
    public DateTime? CreatedOn { get; set; }

    [Column("USERNAME")]
    public string? ModifiedBy { get; set; }

    [Column("MODIFIEDON")]
    public DateTime? ModifiedOn { get; set; }
}