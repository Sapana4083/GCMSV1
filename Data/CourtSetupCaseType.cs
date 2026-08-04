using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

[Table("COURT_SETUP_CASETYPE")]
public class CourtSetupCaseType
{
    [Key]
    public long Id { get; set; }

    public long CaseType { get; set; }

    public string TCourtCode { get; set; }
}