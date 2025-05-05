// using BTGClientManager.Data;
// using BTGClientManager.Models;
// using BTGClientManager.Services;
// using Microsoft.EntityFrameworkCore;
// using Microsoft.VisualStudio.TestTools.UnitTesting;
// using System;
// using System.Linq;
// using System.Threading.Tasks;

// namespace BTGClientManager.Tests;

// [TestClass]
// public class ClientServicePersistentTests
// {
//     private IClientService _svc = null!;

//     [TestInitialize]
//     public void Setup()
//     {
//         // cria um banco isolado por teste
//         var opts = new DbContextOptionsBuilder<AppDbContext>()
//             .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
//             .Options;

//         // Factory evita problemas de lifetime
//         var factory = new PooledDbContextFactory<AppDbContext>(opts);

//         // garante que o schema existe
//         using var ctx = factory.CreateDbContext();
//         ctx.Database.EnsureCreated();

//         _svc = new ClientServicePersistent(factory);
//     }

//     [TestMethod]
//     public async Task AddClientAsync_ShouldPersist()
//     {
//         var client = new Client { Name = "Ana", Lastname = "Silva", Age = 22, Address = "Rua 1" };

//         await _svc.AddClientAsync(client);

//         var all = await _svc.GetAllClientsAsync();
//         Assert.AreEqual(1, all.Count);
//         Assert.AreEqual("Ana", all[0].Name);
//     }

//     [TestMethod]
//     public async Task UpdateClientAsync_ShouldModify()
//     {
//         var client = new Client { Name = "Bob", Lastname = "Souza", Age = 30, Address = "Rua 2" };
//         await _svc.AddClientAsync(client);

//         client.Age = 31;
//         client.Address = "Rua 99";
//         await _svc.UpdateClientAsync(client);

//         var db = await _svc.GetClientByIdAsync(client.Id);
//         Assert.IsNotNull(db);
//         Assert.AreEqual(31,  db!.Age);
//         Assert.AreEqual("Rua 99", db.Address);
//     }

//     [TestMethod]
//     public async Task DeleteClientAsync_ShouldRemove()
//     {
//         var c1 = new Client { Name = "Carlos", Lastname = "Santos", Age = 40, Address = "Av. X" };
//         await _svc.AddClientAsync(c1);

//         await _svc.DeleteClientAsync(c1.Id);

//         var remaining = await _svc.GetAllClientsAsync();
//         Assert.AreEqual(0, remaining.Count);
//     }

//     [TestMethod]
//     public async Task GetAllClientsAsync_ShouldReturnSeeded()
//     {
//         await _svc.AddClientAsync(new Client { Name = "João", Lastname = "A", Age = 20, Address = "-" });
//         await _svc.AddClientAsync(new Client { Name = "Maria", Lastname = "B", Age = 25, Address = "-" });

//         var list = await _svc.GetAllClientsAsync();

//         Assert.AreEqual(2, list.Count);
//         CollectionAssert.AreEquivalent(
//             new[] { "João", "Maria" },
//             list.Select(c => c.Name).ToArray());
//     }
// }
