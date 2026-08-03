using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GCMS.Models.Entities
{
    [Table("DEPARTMENT_MAST")]
    public class DepartmentMaster
    {
        [Key]
        [Column("DEPARTMENT_MASTID")]
        public long DepartmentId { get; set; }

        [Column("DEPTNAMEEN")]
        public string? DepartmentName { get; set; }

        [Column("DEPTNAMEHI")]
        public string? DepartmentNameHindi { get; set; }

        [Column("COURTCODE")]
        public string? CourtCode { get; set; }

        [Column("TITLE")]
        public string? Title { get; set; }

        [Column("DESCRIPTION")]
        public string? Description { get; set; }

        [Column("ISACTIVE")]
        public string? IsActive { get; set; }

        [Column("CREATEDBY")]
        public string? CreatedBy { get; set; }

        [Column("CREATEDON")]
        public DateTime? CreatedOn { get; set; }

        [Column("MODIFIEDBY")]
        public string? ModifiedBy { get; set; }

        [Column("MODIFIEDON")]
        public DateTime? ModifiedOn { get; set; }

        [NotMapped]
        public string? EmailId { get; set; }
    }
}
