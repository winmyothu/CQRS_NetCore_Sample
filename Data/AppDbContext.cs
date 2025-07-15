using Microsoft.EntityFrameworkCore;
using CQRSExample.Models;

namespace CQRSExample.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Product> Products { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<GuestRegistration> GuestRegistrations { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<RefreshToken> RefreshTokens { get; set; }
        public DbSet<PaymentTransaction> PaymentTransactions { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configure one-to-many relationship between User and RefreshToken
            modelBuilder.Entity<RefreshToken>()
                .HasOne(rt => rt.User)
                .WithMany(u => u.RefreshTokens) // Specify the navigation property on the User side
                .HasForeignKey(rt => rt.UserId)
                .OnDelete(DeleteBehavior.Cascade); // Cascade delete refresh tokens when user is deleted

            // Configure precision for decimal properties
            modelBuilder.Entity<PaymentTransaction>().Property(pt => pt.Amount).HasPrecision(18, 2);
            modelBuilder.Entity<Product>().Property(p => p.Price).HasPrecision(18, 2);

            // Configure one-to-many relationship between GuestRegistration and PaymentTransaction
            modelBuilder.Entity<PaymentTransaction>()
                .HasOne(pt => pt.GuestRegistration)
                .WithMany()
                .HasForeignKey(pt => pt.GuestRegistrationId)
                .OnDelete(DeleteBehavior.Cascade); // Cascade delete payments when registration is deleted
        }
    }
}
