namespace GCMS.Models;

public class VcLink
{
    public long TrnRcsatVclinkId { get; set; }
    public string? CourtCode { get; set; }
    public DateTime? HearingDate { get; set; }
    public string? Type { get; set; }
    public string? Bench { get; set; }
    public string? VcLinkUrl { get; set; }
    public string? CreatedBy { get; set; }
    public DateTime? CreatedOn { get; set; }
}