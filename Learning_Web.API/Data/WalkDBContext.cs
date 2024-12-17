using Learning_Web.API.Models.Domain;
using Microsoft.EntityFrameworkCore;

namespace Learning_Web.API.Data
{
    public class WalkDBContext : DbContext
    {
        public WalkDBContext(DbContextOptions<WalkDBContext> dbContextOptions) : base(dbContextOptions)
        {

        }

        public DbSet<Walk> Walks { get; set; }
        public DbSet<Difficulty> Difficultys { get; set; }
        public DbSet<Region> Regions { get; set; }
        public DbSet<Image> Images { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            // seed data for the table difficulty (easy, medium, hard)
            modelBuilder.Entity<Difficulty>().HasData(
                new Difficulty { Id = Guid.Parse("f2a3769d-2b91-4879-a1e9-ef6601dac153"), Name = "Easy" },
                new Difficulty { Id = Guid.Parse("25ffb097-718a-4287-a194-85fb24a4dbb6"), Name = "Medium" },
                new Difficulty { Id = Guid.Parse("e348bf44-9717-48ba-8c3e-27c74d0987a3"), Name = "Hard" }
            );
        }
    }
}
