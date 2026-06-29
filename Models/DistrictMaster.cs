using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GCMS.WEB.Models;

[Table("DISTRICT_MAST")]
public class DistrictMaster
{
    [Key]
    [Column("DISTRICT_MASTID")]
    public long DistrictMastId { get; set; }

    [Column("DISTRICT_NAME")]
    [Required(ErrorMessage = "District Name is required")]
    public string? DistrictName { get; set; }

    [Column("DISTRICT_CODE")]
    [Required(ErrorMessage = "District Code is required")]
    public string? DistrictCode { get; set; }

    [Column("STATE_NAME")]
    [Required(ErrorMessage = "State is required")]
    public long? StateName { get; set; }

    [Column("DIVISION_NAME")]
    [Required(ErrorMessage = "Division is required")]
    public long? DivisionName { get; set; }

    [Column("DISTRICT_NAME_EMG")]
    public string? DistrictNameEmg { get; set; }

    [Column("DIST_ABR")]
    [Required(ErrorMessage = "District Abbreviation is required")]
    public string? DistAbr { get; set; }

    [Column("DIST_NAME_HINENG")]
    public string? DistNameHinEng { get; set; }

    [Column("INACTIVE")]
    public string? InActive { get; set; }

    [Column("CREATEDBY")]
    public string? CreatedBy { get; set; }
}
