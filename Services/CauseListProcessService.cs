using GCMS.Models.ViewModels;
using GCMS.Repository.Interfaces;
using GCMS.Services.Interfaces;

namespace GCMS.Services
{
    public class CauseListProcessService : ICauseListProcessService
    {
        private readonly ICauseListProcessRepository _repository;

        public CauseListProcessService(ICauseListProcessRepository repository)
        {
            _repository = repository;
        }

        public Task GenerateCauseListAsync(CauseListGenerateViewModel model) =>
            _repository.GenerateCauseListAsync(model);

        public Task<List<CauseListGeneratedRow>> GetGeneratedListAsync(DateTime hearingDate, string courtCode) =>
            _repository.GetGeneratedListAsync(hearingDate, courtCode);
    }
}