using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GCMS.Models.Entities
{
    [Table("DEPARTMENT_MAST")]
    public class DepartmentMaster
    {
        [Key]
        [Column("DEPARTMENT_MASTID")]
        public long DepartmentMastId { get; set; }

        [Column("DEPTNAMEEN")]
        public string? DeptNameEn { get; set; }
        
        [Column("ISACTIVE")]
        public string? IsActive { get; set; }
    }
}
