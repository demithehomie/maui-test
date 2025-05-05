using BTGClientManager.Models;
using Microsoft.EntityFrameworkCore;
using System.IO;

namespace BTGClientManager.Data
{
    public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
    {
        public DbSet<Client> Clients { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
                modelBuilder.Entity<Client>()
                 .ToTable("clients"); // Use lowercase for PostgreSQL

            modelBuilder.Entity<Client>().HasKey(c => c.Id);
            modelBuilder.Entity<Client>().Property(c => c.Name).IsRequired();
            modelBuilder.Entity<Client>().Property(c => c.Lastname).IsRequired();
            modelBuilder.Entity<Client>().Property(c => c.Address).IsRequired();
        }
    }
}