using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GCMS.Models;

[Table("MAST_RCSAT_ADVOCATE")]
public class AdvocateMaster
{
    [Key]
    [Column("MAST_RCSAT_ADVOCATEID")]
    public long MastRcsatAdvocateId { get; set; }

    [Column("ADVNAME")]
    [Required(ErrorMessage = "Name (In English) is required")]
    [StringLength(150)]
    public string? AdvName { get; set; }

    [Column("ADVNAMEHI")]
    [Required(ErrorMessage = "Name (In Hindi) is required")]
    [StringLength(150)]
    public string? AdvNameHi { get; set; }

    [NotMapped]
    public string? DepEngHi { get; set; }

    [Column("ADVENGHI")]
    public string? AdvEngHi { get; set; }

    [Column("ADVEMAIL")]
    [StringLength(100)]
    public string? AdvEmail { get; set; }

    [Column("ADVMOBILE")]
    public long? AdvMobile { get; set; }

    [Column("BARCOUNCILNO")]
    [StringLength(50)]
    public string? BarCouncilNo { get; set; }

    [Column("INACTIVE")]
    public string? InActive { get; set; }

    [Column("CREATEDBY")]
    public string? CreatedBy { get; set; }

    [Column("CREATEDON")]
    public DateTime? CreatedOn { get; set; }

    [Column("MODIFIEDON")]
    public DateTime? ModifiedOn { get; set; }

    // ─── Not DB columns — form binding only ───
    [NotMapped]
    public string? DepartmentIds { get; set; }   // comma-separated, e.g. "3,5,9"

    [NotMapped]
    public string? CourtIds { get; set; }        // comma-separated, e.g. "12,18"
}