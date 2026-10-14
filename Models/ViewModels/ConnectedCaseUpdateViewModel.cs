namespace GCMS.Models.ViewModels
{
    public class ConnectedCaseUpdateViewModel
    {
        public long? CaseUpdateId { get; set; }
        public string? CourtName { get; set; }
        public string? CourtCode { get; set; }
        public long? CaseTypeId { get; set; }
        public string? CaseTypeName { get; set; }
        public string? LinkCase { get; set; }          // main case no
        public string? AppellantName { get; set; }
        public string? RespondentName { get; set; }
        public DateTime? InstitutionDate { get; set; }
        public DateTime? HearingDate { get; set; }
        public string? District { get; set; }
        public string? PurposeName { get; set; }
        public List<ConnectedCaseRowViewModel> Rows { get; set; } = new();
    }

    public class ConnectedCaseRowViewModel
    {
        public long? LinkedCaseId { get; set; }
        public long? CaseUpdateId { get; set; }
        public int? RowNo { get; set; }
        public long? CaseTypeId { get; set; }
        public string? CaseTypeName { get; set; }
        public string? CaseNo { get; set; }            // child case
        public string? MainCase { get; set; }          // parent case
        public DateTime? HearingDate { get; set; }
        public string? AppellantName { get; set; }
        public string? RespondentName { get; set; }
        public string? District { get; set; }
        public string? Purpose { get; set; }
        public string? CourtCode { get; set; }
    }
}