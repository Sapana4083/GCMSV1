namespace GCMS.Models.Entities
{
    public sealed class CaseType
    {
        public long Id { get; set; }
        public string? Code { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? EnglishName { get; set; }
        public string? Inactive { get; set; }
        public int? DisplayOrder { get; set; }
    }

    public sealed class CasePurpose
    {
        public long Id { get; set; }
        public string? Code { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? EnglishName { get; set; }
        public string? Inactive { get; set; }
        public int? DisplayOrder { get; set; }
    }

    public sealed class CaseSubject
    {
        public long Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? HindiName { get; set; }
        public string? Inactive { get; set; }
    }

    public sealed class BenchType
    {
        public long Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Code { get; set; }
        public string? EnglishName { get; set; }
        public string? IsActive { get; set; }
    }

    public sealed class Department
    {
        public long Id { get; set; }
        public string NameEnglish { get; set; } = string.Empty;
        public string? NameHindi { get; set; }
        public string? IsActive { get; set; }
    }
}
