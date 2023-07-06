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
        public DbSet<Packaging> packagings { get; set; }
        public DbSet<ComCost> comCosts { get; set; }
        public DbSet<Insurance> insurances { get; set; }
        public DbSet<WeightDist> weightDists { get; set; }
        public DbSet<Package> packages { get; set; }
        public DbSet<Order> orders { get; set; }

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
            modelBuilder.Entity<Packaging>().ToTable("Packaging");
            modelBuilder.Entity<ComCost>().ToTable("ComCost");
            modelBuilder.Entity<Insurance>().ToTable("Insurance");
            modelBuilder.Entity<WeightDist>().ToTable("WeightDist");
            modelBuilder.Entity<Package>().ToTable("Package");
            modelBuilder.Entity<Order>().ToTable("Order");
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
            modelBuilder.Entity<Package>()
                .HasOne(x => x.Sender)
                .WithMany()
                .HasForeignKey(x => x.SenderId);
            modelBuilder.Entity<Package>()
                .HasOne(x => x.Receiver)
                .WithMany()
                .HasForeignKey(x => x.ReceiverId);
            modelBuilder.Entity<Packaging>()
                .HasIndex(x => x.Size)
                .IsUnique();
            modelBuilder.Entity<WeightDist>()
                 .HasIndex(x => x.MaxWeight)
                 .IsUnique();
            modelBuilder.Entity<WeightDist>()
                 .HasIndex(x => x.MinWeight)
                 .IsUnique();
            modelBuilder.Entity<Insurance>()
                 .HasIndex(x => x.MaxVal)
                 .IsUnique();
            modelBuilder.Entity<Insurance>()
                 .HasIndex(x => x.MinVal)
                 .IsUnique();
            modelBuilder.Entity<Order>()
                .HasOne(x => x.Package)
                .WithMany()
                .HasForeignKey(x => x.PackageId);
        }
    }
}
