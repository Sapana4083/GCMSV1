using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GCMS.Models
{
    [Table("DIVISION_MAST")]
    public class DivisionMaster
    {
        [Key]
        [Column("DIVISION_MASTID")]
        public long DivisionMastId { get; set; }

        [Column("DIVISION_NAME")]
        public string? DivisionName { get; set; }

        [Column("DIVISION_NAME_HINDI")]
        public string? DivisionNameHindi { get; set; }

        [Column("STATE_NAME")]
        public string? StateName { get; set; }

        [Column("STATE_MASTID")]
        public string? StateMastId { get; set; }

        [Column("INACTIVE")]
        public string? InActive { get; set; }

        [Column("USERNAME")]
        public string? UserName { get; set; }

        [Column("CREATEDBY")]
        public string? CreatedBy { get; set; }

        [Column("CREATEDON")]
        public DateTime? CreatedOn { get; set; }

        [Column("MODIFIEDON")]

        public DateTime? ModifiedOn { get; set; }
        
    }
}