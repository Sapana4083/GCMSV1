using GCMS.Data;
using GCMS.Models;
using GCMS.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace GCMS.Repository
{
    public class CauseListConfigurationRepository : ICauseListConfigurationRepository
    {
        private readonly ApplicationDbContext _context;

        public CauseListConfigurationRepository(
            ApplicationDbContext context)
        {
            _context = context;
        }


        public async Task<List<CasePurposeMaster>> GetCasePurposesAsync()
        {
            return await _context.CasePurposeMasters
                .Where(x => x.InActive == "Y")
                .OrderBy(x => x.CasePurposeName)
                .ToListAsync();
        }
    }
}
