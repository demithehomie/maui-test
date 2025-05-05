using BTGClientManager.Data;
using BTGClientManager.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.ObjectModel;

namespace BTGClientManager.Services
{

      public class ClientServicePersistent : IClientService
{
    private readonly IDbContextFactory<AppDbContext> _factory;
    public ClientServicePersistent(IDbContextFactory<AppDbContext> factory) => _factory = factory;

    public async Task<List<Client>> GetAllClientsAsync()
    {
        await using var db = await _factory.CreateDbContextAsync();
        return await db.Clients.AsNoTracking().OrderBy(c => c.Name).ToListAsync();
    }

    public async Task<Client?> GetClientByIdAsync(int id)
    {
        await using var db = await _factory.CreateDbContextAsync();
        return await db.Clients.FindAsync(id);
    }

    public async Task AddClientAsync(Client client)
    {
        await using var db = await _factory.CreateDbContextAsync();
        db.Clients.Add(client);
        await db.SaveChangesAsync();
    }

    public async Task UpdateClientAsync(Client client)
    {
        await using var db = await _factory.CreateDbContextAsync();
        db.Clients.Update(client);
        await db.SaveChangesAsync();
    }

    public async Task DeleteClientAsync(int id)
    {
        await using var db = await _factory.CreateDbContextAsync();
        var entity = await db.Clients.FindAsync(id);
        if (entity is null) return;
        db.Clients.Remove(entity);
        await db.SaveChangesAsync();
    }
}


}