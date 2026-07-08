using GCMS.Data;
using GCMS.Models.Entities;
using GCMS.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;
namespace GCMS.Repository
{
    public class CaseRepository : ICaseRepository
    {
        private readonly ApplicationDbContext _context;

        public CaseRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<CaseRegistration>> GetAllAsync()
        {
            return await _context.CaseRegistrations
                                 .OrderByDescending(x => x.CaseId)
                                 .ToListAsync();
        }

        public async Task<CaseRegistration?> GetByIdAsync(long id)
        {
            return await _context.CaseRegistrations.FindAsync(id);
        }

        public async Task<long> AddAsync(CaseRegistration entity)
        {
            _context.CaseRegistrations.Add(entity);
            await _context.SaveChangesAsync();
            return entity.CaseId;
        }

        public async Task UpdateAsync(CaseRegistration entity)
        {
            _context.CaseRegistrations.Update(entity);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(long id)
        {
            var entity = await _context.CaseRegistrations.FindAsync(id);

            if (entity != null)
            {
                _context.CaseRegistrations.Remove(entity);
                await _context.SaveChangesAsync();
            }
        }
    }
}
