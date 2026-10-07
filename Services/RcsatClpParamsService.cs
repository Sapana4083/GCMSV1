using GCMS.Models;
using GCMS.Repository.Interfaces;
using GCMS.Services.Interfaces;

namespace GCMS.Services
{
    public class RcsatClpParamsService : IRcsatClpParamsService
    {
        private readonly IRcsatClpParamsRepository _repository;

        public RcsatClpParamsService(IRcsatClpParamsRepository repository)
        {
            _repository = repository;
        }

        public Task<List<RcsatClpParams>> SearchAsync(DateTime fromDate, DateTime toDate) =>
            _repository.SearchAsync(fromDate, toDate);
    }
}