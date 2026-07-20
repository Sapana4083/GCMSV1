using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GCMS.Models.Entities
{
    [Table("TRN_RCSAT_PRIVATE_PARTY")]
    public class CasePrivateParty
    {
        [Key]
        [Column("TRN_RCSAT_PRIVATE_PARTYID")]
        public long PrivatePartyId { get; set; }

        [Column("TRN_RCSAT_CASEREGID")]
        public long CaseId { get; set; }

        [Column("NAME")]
        public string? PartyName { get; set; }

        [Column("PRIVATE_DESIGNATION")]
        public string? Designation { get; set; }

        [Column("PRIVADVOCATEE")]
        public long? Advocate { get; set; }

        public string? CreatedBy { get; set; }
            public long? AdvocateId { get; set; }
    }
}