// AppDbContextFactory.cs
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System.IO;

namespace BTGClientManager.Data;

public class AppDbContextFactory : IDesignTimeDbContextFactory<AppDbContext>
{
    public AppDbContext CreateDbContext(string[] args)
    {
        // carrega appsettings.json de forma independente do MAUI
        var config = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: false)
            .Build();

        var conn = config.GetConnectionString("Postgres") ??
                   throw new InvalidOperationException("ConnString 'Postgres' não encontrada.");

        var opts = new DbContextOptionsBuilder<AppDbContext>()
            .UseNpgsql(conn, npg => npg.EnableRetryOnFailure())
            .Options;

        return new AppDbContext(opts);
    }
}
