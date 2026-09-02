namespace GCMS.Models.ViewModels
{
    public class CaseRegistrationListItem
    {
        public long RecordId { get; set; }
        public string? MCaseNoo { get; set; }
        public DateTime? InstitutionDate { get; set; }
        public string? CaseType { get; set; }
        public string? CaseSubject { get; set; }
        public string? CasePurposeName { get; set; }
        public DateTime? HearingDate { get; set; }
        public string? BenchType { get; set; }
        public string? CreatedBy { get; set; }
        public DateTime? CreatedOn { get; set; }
        public string? ModifiedBy { get; set; }
        public DateTime? ModifiedOn { get; set; }
    }
}