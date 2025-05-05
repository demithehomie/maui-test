using System.ComponentModel.DataAnnotations;

namespace BTGClientManager.Models
{
    public class Client
    {
      

        public int Id { get; set; }
        
        [Required(ErrorMessage = "Nome é obrigatório")]
        public string Name { get; set; }
        
        [Required(ErrorMessage = "Sobrenome é obrigatório")]
        public string Lastname { get; set; }
        
        [Range(0, 120, ErrorMessage = "Idade deve estar entre 0 e 120")]
        public int Age { get; set; }
        
        [Required(ErrorMessage = "Endereço é obrigatório")]
        public string Address { get; set; }

        public Client()
        {
            Name = string.Empty;
            Lastname = string.Empty;
            Address = string.Empty;
        }
    }
}