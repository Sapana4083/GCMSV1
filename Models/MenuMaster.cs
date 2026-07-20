using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GCMS.Models
{
    [Table("GCMS_MENU")]
    public class MenuMaster
    {
        [Key]
        [Column("MENU_ID")]
        public long MenuId { get; set; }

        [Column("MENU_NAME")]
        public string? MenuName { get; set; }

        [Column("PARENT_ID")]
        public long? ParentId { get; set; }

        [Column("MENU_URL")]
        public string? MenuUrl { get; set; }

        [Column("IS_ACTV_FLAG")]
        public int IsActvFlag { get; set; }

        [Column("IS_DEL_FLAG")]
        public int IsDelFlag { get; set; }

        [Column("CREATE_BY")]
        public string? CreateBy { get; set; }

        [Column("MODIFY_BY")]
        public string? ModifyBy { get; set; }

        // Not mapped to database - used only for display
        [NotMapped]
        public string? ParentName { get; set; }
    }
}