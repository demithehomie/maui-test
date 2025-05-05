using BTGClientManager.Models;
using BTGClientManager.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;

namespace BTGClientManager.Tests
{
    [TestClass]
    public class ClientServiceTests
    {
        private IClientService _clientService = null!;

        [TestInitialize]
        public void Setup() => _clientService = new ClientService();

        [TestMethod]
        public void GetAllClients_ShouldReturnAllClients()
        {
            var clients = _clientService.GetAllClients();

            Assert.IsNotNull(clients);
            Assert.IsTrue(clients.Count >= 3);
        }

        [TestMethod]
        public void AddClient_ShouldAddNewClient()
        {
            var initialCount = _clientService.GetAllClients().Count;

            var newClient = new Client
            {
                Name = "Test",
                Lastname = "User",
                Age = 25,
                Address = "Test Address"
            };

            _clientService.AddClient(newClient);
            var clients = _clientService.GetAllClients();

            Assert.AreEqual(initialCount + 1, clients.Count);

            var addedClient = clients.FirstOrDefault(c => c.Name == "Test" && c.Lastname == "User");
            Assert.IsNotNull(addedClient);

            // uso do null-forgiving
            Assert.AreEqual(25, addedClient!.Age);
            Assert.AreEqual("Test Address", addedClient.Address);
        }

        [TestMethod]
        public void UpdateClient_ShouldModifyExistingClient()
        {
            var newClient = new Client
            {
                Name = "UpdateTest",
                Lastname = "User",
                Age = 30,
                Address = "Original Address"
            };
            _clientService.AddClient(newClient);

            var clientToUpdate = _clientService.GetAllClients()
                                               .FirstOrDefault(c => c.Name == "UpdateTest");
            Assert.IsNotNull(clientToUpdate);

            clientToUpdate!.Address = "Updated Address";
            clientToUpdate.Age = 35;
            _clientService.UpdateClient(clientToUpdate);

            var updatedClient = _clientService.GetClientById(clientToUpdate.Id);
            Assert.IsNotNull(updatedClient);

            Assert.AreEqual("UpdateTest", updatedClient!.Name);
            Assert.AreEqual("User",        updatedClient.Lastname);
            Assert.AreEqual(35,            updatedClient.Age);
            Assert.AreEqual("Updated Address", updatedClient.Address);
        }

        [TestMethod]
        public void DeleteClient_ShouldRemoveClient()
        {
            var newClient = new Client
            {
                Name = "DeleteTest",
                Lastname = "User",
                Age = 40,
                Address = "Delete Address"
            };
            _clientService.AddClient(newClient);

            var clientToDelete = _clientService.GetAllClients()
                                               .FirstOrDefault(c => c.Name == "DeleteTest");
            Assert.IsNotNull(clientToDelete);

            var initialCount = _clientService.GetAllClients().Count;

            _clientService.DeleteClient(clientToDelete!.Id);
            var clients = _clientService.GetAllClients();

            Assert.AreEqual(initialCount - 1, clients.Count);
            Assert.IsNull(clients.FirstOrDefault(c => c.Name == "DeleteTest"));
        }
    }
}
