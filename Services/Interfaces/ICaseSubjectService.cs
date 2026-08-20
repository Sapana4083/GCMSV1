using GCMS.Models;

namespace GCMS.Services.Interfaces
{
    public interface ICaseSubjectService
    {
        Task<List<CaseSubjectMaster>> GetAllAsync(int pageNo, int rowCnt);

        Task<CaseSubjectMaster?> GetByIdAsync(long id);

        Task AddAsync(CaseSubjectMaster model);

        Task UpdateAsync(CaseSubjectMaster model);
        Task<List<CaseSubjectMaster>> GetCaseSubjectAsync(int pageNo, int rowCnt);
        
    }
}