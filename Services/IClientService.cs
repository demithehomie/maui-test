
using BTGClientManager.Models;
using System.Collections.Generic;

namespace BTGClientManager.Services
{
    public interface IClientService
{
    Task<List<Client>>  GetAllClientsAsync();
    Task<Client?>       GetClientByIdAsync(int id);
    Task                AddClientAsync(Client client);
    Task                UpdateClientAsync(Client client);
    Task                DeleteClientAsync(int id);
}

}
