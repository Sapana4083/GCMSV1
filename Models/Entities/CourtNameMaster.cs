using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GCMS.Models.Entities
{
    [Table("COURT_NAME_MAST")]
    public class CourtNameMaster
    {
        [Key]
        [Column("COURT_NAME_MASTID")]
        public long CourtNameMastId { get; set; }

        [Column("COURT_NAME")]
        public string? CourtName { get; set; }

        [Column("COURT_CODE")]
        public string? CourtCode { get; set; }
    }
}
