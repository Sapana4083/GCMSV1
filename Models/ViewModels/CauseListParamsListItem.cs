namespace GCMS.Models.ViewModels
{
    public class CauseListParamsListItem
    {
        public long TrnRcsatClpParamsId { get; set; }
        public string? DocNo { get; set; }
        public string? Remarks { get; set; }
        public string? CauseListType { get; set; }
        public int? CaseCountLimit { get; set; }
        public string? IsActive { get; set; }
        public DateTime? DocDt { get; set; }
    }
}