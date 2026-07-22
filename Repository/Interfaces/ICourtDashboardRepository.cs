using GCMS.Models;

namespace GCMS.Repository.Interfaces
{
    public interface ICourtDashboardRepository
    {
         Task<List<CourtDashboardViewModel>> GetCourtDashboardDataAsync();
    }
}
