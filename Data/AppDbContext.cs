using Microsoft.EntityFrameworkCore;
using BTGClientManager.Models;

namespace BTGClientManager.Data
{
    public class AppDbContext : DbContext
    {
        public DbSet<Client> Clients => Set<Client>();

        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Client>(entity =>
            {
                entity.ToTable("clients"); // nome da tabela
                entity.HasKey(e => e.Id);

                entity.Property(e => e.Id).HasColumnName("Id");
                entity.Property(e => e.Name).HasColumnName("Name");
                entity.Property(e => e.Lastname).HasColumnName("Lastname");
                entity.Property(e => e.Age).HasColumnName("Age");
                entity.Property(e => e.Address).HasColumnName("Address");
            });
        }
    }
}
