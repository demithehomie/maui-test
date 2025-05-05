using BTGClientManager.Data;
using BTGClientManager.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.ObjectModel;

namespace BTGClientManager.Services
{
    public class ClientServicePersistent : IClientService
    {
        private readonly AppDbContext _dbContext;

        public ClientServicePersistent(AppDbContext dbContext)
        {
            _dbContext = dbContext;
            
            // Add sample data if database is empty
            if (!_dbContext.Clients.Any())
            {
                _dbContext.Clients.Add(new Client { Name = "João", Lastname = "Silva", Age = 30, Address = "Rua A, 123" });
                _dbContext.Clients.Add(new Client { Name = "Maria", Lastname = "Santos", Age = 25, Address = "Av. B, 456" });
                _dbContext.Clients.Add(new Client { Name = "Carlos", Lastname = "Ferreira", Age = 40, Address = "Praça C, 789" });
                _dbContext.SaveChanges();
            }
        }

        public List<Client> GetAllClients()
        {
            return _dbContext.Clients.ToList();
        }

        public Client GetClientById(int id)
        {
            return _dbContext.Clients.FirstOrDefault(c => c.Id == id) ?? new Client();
        }

        public void AddClient(Client client)
        {
            _dbContext.Clients.Add(client);
            _dbContext.SaveChanges();
        }

        public void UpdateClient(Client client)
        {
            var existingClient = _dbContext.Clients.FirstOrDefault(c => c.Id == client.Id);
            if (existingClient != null)
            {
                existingClient.Name = client.Name;
                existingClient.Lastname = client.Lastname;
                existingClient.Age = client.Age;
                existingClient.Address = client.Address;
                _dbContext.SaveChanges();
            }
        }

        public void DeleteClient(int id)
        {
            var client = _dbContext.Clients.FirstOrDefault(c => c.Id == id);
            if (client != null)
            {
                _dbContext.Clients.Remove(client);
                _dbContext.SaveChanges();
            }
        }
    }
}