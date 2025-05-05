using BTGClientManager.Models;
using System.Collections.ObjectModel;

namespace BTGClientManager.Services
{
    public class ClientService : IClientService
    {
        private List<Client> _clients;
        private int _nextId = 1;

        public ClientService()
        {
            _clients = new List<Client>();
            // Add some sample clients for testing
            AddClient(new Client { Name = "João", Lastname = "Silva", Age = 30, Address = "Rua A, 123" });
            AddClient(new Client { Name = "Maria", Lastname = "Santos", Age = 25, Address = "Av. B, 456" });
            AddClient(new Client { Name = "Carlos", Lastname = "Ferreira", Age = 40, Address = "Praça C, 789" });
        }

        public List<Client> GetAllClients()
        {
            return _clients.ToList();
        }

        public Client GetClientById(int id)
        {
            return _clients.FirstOrDefault(c => c.Id == id) ?? new Client();
        }

        public void AddClient(Client client)
        {
            client.Id = _nextId++;
            _clients.Add(client);
        }

        public void UpdateClient(Client client)
        {
            var existingClient = _clients.FirstOrDefault(c => c.Id == client.Id);
            if (existingClient != null)
            {
                var index = _clients.IndexOf(existingClient);
                _clients[index] = client;
            }
        }

        public void DeleteClient(int id)
        {
            var client = _clients.FirstOrDefault(c => c.Id == id);
            if (client != null)
            {
                _clients.Remove(client);
            }
        }
    }
}