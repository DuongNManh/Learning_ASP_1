using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Learning_Web.API.Data
{
    public class WalkAuthenDBContext : IdentityDbContext
    {
        public WalkAuthenDBContext(DbContextOptions<WalkAuthenDBContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            var readerId = "33fa4de2-75bd-4817-a0ea-af457af474b7";
            var writerId = "78738410-e882-4ff0-82ef-96edae539ea5";

            var roles = new List<IdentityRole>
                {
                new IdentityRole
                {
                    Id = readerId,
                    ConcurrencyStamp = readerId,
                    Name = "reader", 
                    NormalizedName = "READER"
                },
                new IdentityRole
                {
                    Id = writerId,
                    ConcurrencyStamp = writerId,
                    Name = "writer",
                    NormalizedName = "WRITER"
                }
            };

            modelBuilder.Entity<IdentityRole>().HasData(roles);
        }
    }
}
