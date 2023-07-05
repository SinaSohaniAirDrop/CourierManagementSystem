using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace CourierManagementSystem.Data
{
    public class DataContext : IdentityDbContext<IdentityUser>
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {
        }

        public DbSet<Request> requests { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            MapTables(modelBuilder);
            SetDescriptionsAndDefaultValues(modelBuilder);
            base.OnModelCreating(modelBuilder);
            SeedRoles(modelBuilder);
        }

        private void MapTables(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Request>().ToTable("Request");
        }

        private static void SeedRoles(ModelBuilder builder)
        {
            builder.Entity<IdentityRole>().HasData(
                new IdentityRole() { Name = "Admin", ConcurrencyStamp = "1", NormalizedName = "Admin" },
                new IdentityRole() { Name = "Developer", ConcurrencyStamp = "2", NormalizedName = "Developer" },
                new IdentityRole() { Name = "User", ConcurrencyStamp = "3", NormalizedName = "User" }
                );
        }

        private void SetDescriptionsAndDefaultValues(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Request>()
                .HasOne(x => x.User)
                .WithMany()
                .HasForeignKey(x => x.UserId);
        }

    }
}
