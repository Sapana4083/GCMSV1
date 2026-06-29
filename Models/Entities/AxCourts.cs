
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GCMS.Web.Models.Entities;

[Table("AXCOURTS")]
public class AxCourts
{
    [Key]
    [Column("AXCOURTSID")]
    public long AxCourtsId { get; set; }

    [Column("AXUSERSID")]
    public long AxUsersId { get; set; }

    [Column("DEPTNAME")]
    public long DeptName { get; set; }

    [Column("COURT_NAME")]
    public long CourtName { get; set; }

    [Column("ISDEFAULT")]
    public string? IsDefault { get; set; }
}
