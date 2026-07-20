using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GCMS.Models.Entities
{
    [Table("TRN_RCSAT_APPELLANT")]
    public class CaseAppellant
    {
        [Key]
        [Column("TRN_RCSAT_APPELLANTID")]
        public long AppellantId { get; set; }

        [Column("TRN_RCSAT_CASEREGID")]
        public long CaseId { get; set; }

        [Column("APPELLANT_NAME")]
        public string? AppellantName { get; set; }

        [Column("DESIGNATION")]
        public string? Designation { get; set; }

        [Column("ADISTRICT_NAME")]
        public string? District { get; set; }

        [Column("MOBILENO")]
        public long? MobileNo { get; set; }

        [Column("APP_ADVOCATE")]
        public long? AdvocateId { get; set; }

        [Column("APPADV_EMAIL")]
        public string? AdvocateEmail { get; set; }

        [Column("APP_ADVMOBILE")]
        public string? AdvocateMobile { get; set; }

        [Column("EMPLOYEEID")]
        public string? EmployeeId { get; set; }
    }
}