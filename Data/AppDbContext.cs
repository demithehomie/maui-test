using BTGClientManager.Models;
using Microsoft.EntityFrameworkCore;
using System.IO;

namespace BTGClientManager.Data
{
    public class AppDbContext : DbContext
    {
        public DbSet<Client> Clients { get; set; }

        public AppDbContext()
        {
            SQLitePCL.Batteries_V2.Init();
            this.Database.EnsureCreated();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            string dbPath = Path.Combine(FileSystem.AppDataDirectory, "clients.db3");
            optionsBuilder.UseSqlite($"Data Source={dbPath}");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Client>()
                .HasKey(c => c.Id);
            
            modelBuilder.Entity<Client>()
                .Property(c => c.Name)
                .IsRequired();
            
            modelBuilder.Entity<Client>()
                .Property(c => c.Lastname)
                .IsRequired();
            
            modelBuilder.Entity<Client>()
                .Property(c => c.Address)
                .IsRequired();
        }
    }
}