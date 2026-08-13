using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GCMS.Models;

[Table("MAST_RCSAT_CSSUBJECT")]
public class CaseSubjectMaster
{
    [Key]
    [Column("MAST_RCSAT_CSSUBJECTID")]
    public long CaseSubjectId { get; set; }

    [Column("CANCEL")]
    public string? Cancel { get; set; }

    [Column("SUBJECT")]
    [Required(ErrorMessage = "Case Subject (In English) is required")]
    [StringLength(300)]
    public string? Subject { get; set; }

    [Column("SUBJECTHI")]
    [Required(ErrorMessage = "Case Subject (In Hindi) is required")]
    [StringLength(300)]
    public string? SubjectHi { get; set; }

    [Column("INACTIVE")]
    public string? InActive { get; set; }

    [Column("SUBJECTENGHI")]
    public string? SubjectEngHi { get; set; }

    [Column("CANCELREMARKS")]
    [StringLength(150)]
    public string? CancelRemarks { get; set; }

    [Column("SOURCEID")]
    public long? SourceId { get; set; }

    [Column("MAPNAME")]
    public string? MapName { get; set; }

    [Column("WKID")]
    public string? WkId { get; set; }

    [Column("APP_LEVEL")]
    public int? AppLevel { get; set; }

    [Column("APP_DESC")]
    public int? AppDesc { get; set; }

    [Column("APP_SLEVEL")]
    public int? AppSLevel { get; set; }

    [Column("WFROLES")]
    public string? WfRoles { get; set; }

    [Column("CREATEDBY")]
    public string? CreatedBy { get; set; }

    [Column("CREATEDON")]
    public DateTime? CreatedOn { get; set; }

    [Column("USERNAME")]
    public string? UserName { get; set; }

    [Column("MODIFIEDON")]
    public DateTime? ModifiedOn { get; set; }
}