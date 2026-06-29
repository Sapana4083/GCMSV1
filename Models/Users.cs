
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GCMS.WEB.Models
{
    [Table("AXUSERS")]
    public class Users
    {
        [Key]
        [Column("AXUSERSID")]
        public long AxUsersId { get; set; }

        [Column("USERNAME")]
        public string? UserName { get; set; }

        [Column("PASSWORDSHA")]
        public string? Password { get; set; }

        [Column("USERGROUP")]
        public string? UserGroup { get; set; }

        [Column("GROUPNO")]
        public string? GroupNo { get; set; }

        [Column("EMAIL")]
        public string? Email { get; set; }

        [Column("ACTIVE")]
        public string? Active { get; set; }

        [Column("LOGINTRY")]
        public int? LoginTry { get; set; }

        [Column("ISFIRSTTIME")]
        public string? IsFirstTime { get; set; }

        [Column("USERTYPE")]
        public string? UserType { get; set; }

        [Column("NICKNAME")]
        public string? NickName { get; set; }
       
    }
}