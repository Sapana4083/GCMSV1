using GCMS.Models;
using GCMS.Repository.Interfaces;
using GCMS.Services.Interfaces;

namespace GCMS.Services
{
    public class RevertCasePendancyService : IRevertCasePendancyService
    {
        private readonly IRevertCasePendancyRepository _repository;

        public RevertCasePendancyService(IRevertCasePendancyRepository repository)
        {
            _repository = repository;
        }

        public Task<RevertCasePendancy?> GetByHearingDateAsync(DateTime hearingDate) =>
            _repository.GetByHearingDateAsync(hearingDate);

        // Har row ke liye alag insert call (SP ki limitation — ek call = ek parent + ek child)
        public async Task<RevertCasePendancy> SaveAsync(RevertCasePendancy model, string createdBy)
        {
            RevertCasePendancy? lastResult = null;

            foreach (var row in model.Cases)
            {
                var single = new RevertCasePendancy
                {
                    MemberName = model.MemberName,
                    HearingDate = model.HearingDate,
                    Cases = new List<RevertCaseRow> { row }
                };

                lastResult = await _repository.AddAsync(single, createdBy);
            }

            return lastResult ?? model;
        }
    }
}