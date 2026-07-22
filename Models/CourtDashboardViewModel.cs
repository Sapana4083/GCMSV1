namespace GCMS.Models
{
    public class CourtDashboardViewModel
    {
        public string CourtName { get; set; }
        public int RegisteredCases { get; set; }
        public int Decided { get; set; }
        public int PendingCases { get; set; }
        public int Users { get; set; }
    }
}
