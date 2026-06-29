using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GCMS.Models;

[Table("STATE_MAST")]
public class StateMaster
{
    [Key]
    [Column("STATE_MASTID")]
    public long StateMastId { get; set; }

    [Column("STATE_NAME")]
    [Required(ErrorMessage = "State Name is required")]
    [StringLength(100)]
    public string? StateName { get; set; }

    [Column("STATE_NAME_HINDI")]
    [StringLength(100)]
    public string? StateNameHindi { get; set; }

    [Column("STATE_CODE")]
    [Required(ErrorMessage = "State Code is required")]
    [StringLength(2, MinimumLength = 2, ErrorMessage = "State Code must be 2 characters")]
    public string? StateCode { get; set; }

    [Column("CAPITAL")]
    [Required(ErrorMessage = "Capital is required")]
    public string? Capital { get; set; }

    [Column("INACTIVE")]
    public string? InActive { get; set; }

    [Column("CREATEDBY")]
    public string? CreatedBy { get; set; }

    [Column("CREATEDON")]
    public DateTime? CreatedOn { get; set; }

    [Column("MODIFIEDON")]
    public DateTime? ModifiedOn { get; set; }
}
