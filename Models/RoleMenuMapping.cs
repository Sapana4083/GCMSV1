using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GCMS.Models
{
    [Table("GCMS_ROLE_MENU_MAPPING")]
    public class RoleMenuMapping
    {
        [Key]
        [Column("ROLE_MENU_ID")]
        public long RoleMenuId { get; set; }

        [Column("ROLE_ID")]
        public long RoleId { get; set; }

        [NotMapped]
        public string? RoleName { get; set; }

        [Column("MENU_ID")]
        public long MenuId { get; set; }

        [NotMapped]
        public string? MenuName { get; set; }

        [Column("PARENT_MENU_ID")]
        public long ParentMenuId { get; set; }

        [Column("IS_ACTV_FLAG")]
        public int IsActvFlag { get; set; }

        [Column("IS_DEL_FLAG")]
        public int IsDelFlag { get; set; }

        [Column("CREATE_BY")]
        public string? CreateBy { get; set; }

        [Column("MODIFY_BY")]
        public string? ModifyBy { get; set; }
    }

    public class RolePermissionVM
    {
        public long MenuId { get; set; }

        public int IsActvFlag { get; set; }

        public int IsDelFlag { get; set; }
    }

    public class SaveRolePermissionVM
    {
        public long RoleId { get; set; }

        public List<RolePermissionVM> Permissions { get; set; } = new();
    }
}
