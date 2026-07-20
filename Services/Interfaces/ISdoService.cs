using GCMS.Models;

public interface ISdoService
{
    Task AddAsync(SdoMaster model);

    Task UpdateAsync(SdoMaster model);

    Task<SdoMaster?> GetByIdAsync(long sdoMastId);

    Task<List<SdoMaster>> GetAllAsync(int pageNo, int rowCount);
}