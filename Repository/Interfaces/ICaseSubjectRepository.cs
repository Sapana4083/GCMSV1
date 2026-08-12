using GCMS.Models;

namespace GCMS.Repository.Interfaces
{
    public interface ICaseSubjectRepository
    {
        Task<List<CaseSubjectMaster>> GetAllAsync(int pageNo, int rowCnt);

        Task<CaseSubjectMaster?> GetByIdAsync(long id);

        Task AddAsync(CaseSubjectMaster model);

        Task UpdateAsync(CaseSubjectMaster model);
    }
}