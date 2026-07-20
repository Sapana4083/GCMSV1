using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GCMS.Models.Entities
{
    [Table("BENCH_TYPE_MAST")]
    public class BenchTypeMaster
    {
        [Key]
        [Column("BENCH_TYPE_MASTID")]
        public long BenchTypeMastId { get; set; }

        [Column("CANCEL")]
        public string? Cancel { get; set; }

        [Column("COURT_NAME")]
        public long? CourtName { get; set; }

        [Column("COURT_CODE")]
        public string? CourtCode { get; set; }

        [Column("BENCH_TYPE")]
        public string? BenchType { get; set; }

        [Column("BENCH_TYPE_CODE")]
        public string? BenchTypeCode { get; set; }

        [Column("IS_ACTIVE")]
        public string? IsActive { get; set; }

        [Column("BENCH_TYPE_ENG")]
        public string? BenchTypeEng { get; set; }
    }
}