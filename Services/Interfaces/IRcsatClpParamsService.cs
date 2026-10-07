using GCMS.Models;

namespace GCMS.Services.Interfaces
{
    public interface IRcsatClpParamsService
    {
        Task<List<RcsatClpParams>> SearchAsync(DateTime fromDate, DateTime toDate);
    }
}