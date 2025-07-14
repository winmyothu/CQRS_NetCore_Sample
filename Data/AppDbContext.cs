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
    }
}
