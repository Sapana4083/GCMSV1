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

        // Display only
        [NotMapped]
        public string? ParentName { get; set; }

        // Bootstrap Icon
        [Column("MENU_ICON")]
        public string? Icon { get; set; }

        [Column("SORT_ORDER")]
        public int SortOrder { get; set; }

        // Display Order
        [NotMapped]
        public int DisplayOrder { get; set; }

        // Child Menus
        [NotMapped]
        public List<MenuMaster> Children { get; set; } = new();

        // Indicates if this menu has children
        [NotMapped]
        public bool HasChildren => Children.Any();
    }
}