using Learning_Web.API.Models.Domain;
using Microsoft.EntityFrameworkCore;

namespace Learning_Web.API.Data
{
    public class WalkDBContext : DbContext
    {
        public WalkDBContext(DbContextOptions dbContextOptions) : base(dbContextOptions)
        {
            
        }

        public DbSet<Walk> Walks { get; set; }
        public DbSet<Difficulty> Difficultys { get; set; }
        public DbSet<Region> Regions { get; set; }

    }
}
