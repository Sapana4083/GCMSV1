namespace GCMS.Models;

public class RevertCasePendancy
{
    public long RevdPendcyId { get; set; }
    public long? MemberName { get; set; }
    public DateTime? HearingDate { get; set; }

    public List<RevertCaseRow> Cases { get; set; } = new();
}

public class RevertCaseRow
{
    public long TrnRevertCaseId { get; set; }
    public int? SerialNo { get; set; }
    public string? CaseNo { get; set; }
    public string? OfficerName { get; set; }
    public DateTime? DecisionDate { get; set; }
    public string? AppellantNamee { get; set; }
    public string? DeptNameHi { get; set; }
    public DateTime? HDate { get; set; }
}