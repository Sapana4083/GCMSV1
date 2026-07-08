namespace GCMS.Models.DTOs;
public sealed class CaseRegistrationDto
    {

        public long Id { get; set; }
        public DateTime InstitutionDate { get; set; }
        public string CaseNumber { get; set; } = string.Empty;
        public string? ImpugnedFlag { get; set; }
        public DateTime? ImpugnedDate { get; set; }
        public long? OrderIssuedById { get; set; }
        public long CaseTypeId { get; set; }
        public string? OldCaseNumber { get; set; }
        public long CaseSubjectId { get; set; }
        public long CasePurposeId { get; set; }
        public DateTime HearingDate { get; set; }
        public long BenchTypeId { get; set; }
        public string? LinkedCaseNumber { get; set; }
        public string AppellantName { get; set; } = string.Empty;
        public string? AppellantDesignation { get; set; }
        public string? AppellantDistrict { get; set; }
        public string? AppellantMobileNumber { get; set; }
        public long? AppellantAdvocateId { get; set; }
        public string? AppellantAdvocateEmail { get; set; }
        public string? AppellantAdvocateMobile { get; set; }
        public long RespondentDepartmentId { get; set; }
        public long? RespondentAdvocateId { get; set; }
        public string? RespondentAdvocateEmail { get; set; }
        public string? RespondentAdvocateMobile { get; set; }
        public string? PrivatePartyName { get; set; }
        public string? PrivateDesignation { get; set; }
        public string? PrivateAdvocateName { get; set; }
        public string? CaseTypeName { get; set; }
        public string? CasePurposeName { get; set; }
        public string? CaseSubjectName { get; set; }
        public string? BenchTypeName { get; set; }
        public string? DepartmentName { get; set; }
    }

    public sealed record LookupItem(long Id, string Text);
    public sealed record CaseSearchCriteria(string? Search, string SortBy, string SortDirection, int PageNumber, int PageSize);


