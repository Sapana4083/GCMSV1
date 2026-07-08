using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GCMS.Models
{
    [Table("GCMS_ROLES")]
    public class RoleMaster
    {
        [Key]
        [Column("ROLE_ID")]
        public long RoleId { get; set; }

        [Column("ROLE_NAME")]
        [Required(ErrorMessage = "Role Name is required.")]
        public string? RoleName { get; set; }

        [Column("ROLE_TYPE")]
        [Required(ErrorMessage = "Role Type is required.")]
        public string? RoleType { get; set; }

        [Column("IS_ACTV_FLAG")]
        public int? IsActvFlag { get; set; }

        [Column("IS_DEL_FLAG")]
        public int? IsDelFlag { get; set; }

        [Column("CREATE_BY")]
        public string? CreateBy { get; set; }

        [Column("MODIFY_BY")]
        public string? ModifyBy { get; set; }

        [Column("ORDER_NO")]
        [Required(ErrorMessage = "Order Number is required.")]
        public int? OrderNo { get; set; }
    }
}