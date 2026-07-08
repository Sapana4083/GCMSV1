using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace GCMS.Models.Entities
{
    [Table("TRN_CASE_PRIVATE_PARTY")]
    public class CasePrivateParty
    {
        [Key]
        public long PrivatePartyId { get; set; }

        public long CaseId { get; set; }

        public string PartyName { get; set; } = string.Empty;

        public string? Designation { get; set; }

        public string? AdvocateName { get; set; }

        [ForeignKey(nameof(CaseId))]
        public virtual CaseRegistration? CaseRegistration { get; set; }
    }
}
