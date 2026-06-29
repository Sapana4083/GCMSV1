
using GCMS.Models.Entities;
using GCMS.Models;
using Microsoft.EntityFrameworkCore;


namespace GCMS.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Users> AxUsers { get; set; }

        public DbSet<AxCourts> AxCourts { get; set; }
        
        public DbSet<DepartmentMaster> DepartmentMasters { get; set; }

        public DbSet<CourtNameMaster> CourtNameMasters { get; set; }
    }
}
