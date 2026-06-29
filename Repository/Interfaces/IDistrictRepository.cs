using GCMS.Models;

public interface IDistrictRepository
{
    Task<List<DistrictMaster>> GetAllAsync(
        int pageNo,
        int rowCnt);

    Task<DistrictMaster?> GetByIdAsync(long id);

    Task AddAsync(DistrictMaster model);

    Task UpdateAsync(DistrictMaster model);

    Task DeleteAsync(long id);
}