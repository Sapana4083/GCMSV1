using GCMS.Models;

public class SdoService : ISdoService
{
    private readonly ISdoRepository _repository;

    public SdoService(ISdoRepository repository)
    {
        _repository = repository;
    }

    public Task AddAsync(SdoMaster model)
        => _repository.AddAsync(model);

    public Task UpdateAsync(SdoMaster model)
        => _repository.UpdateAsync(model);

    public Task<SdoMaster?> GetByIdAsync(long sdoMastId)
        => _repository.GetByIdAsync(sdoMastId);

    public Task<List<SdoMaster>> GetAllAsync(int pageNo, int rowCount)
        => _repository.GetAllAsync(pageNo, rowCount);
}