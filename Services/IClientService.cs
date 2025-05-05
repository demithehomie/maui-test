// using System.ComponentModel.DataAnnotations;

// namespace BTGClientManager.Models
// {
//     public class Client
//     {
//         public int Id { get; set; }
        
//         [Required(ErrorMessage = "Nome é obrigatório")]
//         public string Name { get; set; }
        
//         [Required(ErrorMessage = "Sobrenome é obrigatório")]
//         public string Lastname { get; set; }
        
//         [Range(0, 120, ErrorMessage = "Idade deve estar entre 0 e 120")]
//         public int Age { get; set; }
        
//         [Required(ErrorMessage = "Endereço é obrigatório")]
//         public string Address { get; set; }

//         public Client()
//         {
//             Name = string.Empty;
//             Lastname = string.Empty;
//             Address = string.Empty;
//         }
//     }
// }

using BTGClientManager.Models;
using System.Collections.Generic;

namespace BTGClientManager.Services
{
    public interface IClientService
    {
        /// <summary>
        /// Retorna todos os clientes.
        /// </summary>
        List<Client> GetAllClients();

        /// <summary>
        /// Retorna o cliente com o Id informado, ou null se não existir.
        /// </summary>
        Client GetClientById(int id);

        /// <summary>
        /// Adiciona um novo cliente. O serviço deve atribuir o Id.
        /// </summary>
        void AddClient(Client client);

        /// <summary>
        /// Atualiza os dados do cliente (identificado pelo client.Id).
        /// </summary>
        void UpdateClient(Client client);

        /// <summary>
        /// Remove o cliente com o Id informado.
        /// </summary>
        void DeleteClient(int id);
    }
}
