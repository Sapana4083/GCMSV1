using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GCMS.Models;

[Table("BENCH_TYPE_MAST")]
public class BenchTypeMaster
{
    [Key]
    [Column("BENCH_TYPE_MASTID")]
    public long BenchTypeMastId { get; set; }

    [Column("UNAME")]
    public string? UName { get; set; }

    [Column("COURT_NAME")]
    public long? CourtName { get; set; }

    [Column("COURT_CODE")]
    [StringLength(10)]
    public string? CourtCode { get; set; }

    [Column("BENCH_TYPE")]
    [Required(ErrorMessage = "Bench Type is required")]
    [StringLength(100)]
    public string? BenchType { get; set; }

    [Column("BENCH_TYPE_CODE")]
    [StringLength(10)]
    public string? BenchTypeCode { get; set; }

    [Column("MINI_LIMIT")]
    public decimal? MiniLimit { get; set; }

    [Column("MAX_LIMIT")]
    public decimal? MaxLimit { get; set; }

    [Column("IS_ACTIVE")]
    public string? IsActive { get; set; }

    [Column("DUPCHECK")]
    public string? DupCheck { get; set; }

    [Column("BENCH_TYPE_ENG")]
    [StringLength(100)]
    public string? BenchTypeEng { get; set; }

    [Column("CANCEL")]
    public string? Cancel { get; set; }

    [Column("CANCELREMARKS")]
    [StringLength(150)]
    public string? CancelRemarks { get; set; }

    [Column("CREATEDBY")]
    public string? CreatedBy { get; set; }

    [Column("CREATEDON")]
    public DateTime? CreatedOn { get; set; }

    [Column("USERNAME")]
    public string? UserName { get; set; }

    [Column("MODIFIEDON")]
    public DateTime? ModifiedOn { get; set; }
}