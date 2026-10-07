using GCMS.Models;

namespace GCMS.Repository.Interfaces
{
    public interface IRcsatClpParamsRepository
    {
        Task<List<RcsatClpParams>> SearchAsync(DateTime fromDate, DateTime toDate);
    }
}