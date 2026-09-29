namespace GCMS.Models;

public class SupplementaryCause
{
    public long TblSupltyCauseId { get; set; }
    public long SupplyCauseListId { get; set; }
    public DateTime? HearingDate { get; set; }
    public long? CaseTypeId { get; set; }
    public string? CaseTypeName { get; set; }
    public string? CaseNo { get; set; }
    public long? CaseId { get; set; }
    public long? PurposeId { get; set; }
    public string? PurposeName { get; set; }
    public string? BenchType { get; set; }
}