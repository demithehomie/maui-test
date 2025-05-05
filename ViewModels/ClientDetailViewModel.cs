using BTGClientManager.Models;
using BTGClientManager.Services;
using System.Windows.Input;

namespace BTGClientManager.ViewModels
{
    [QueryProperty(nameof(ClientId), "ClientId")]
    public class ClientDetailViewModel : BaseViewModel
    {
        private readonly IClientService _svc;

        public int ClientId { get; set; } = 0;
        public string Title { get; set; } = "Novo Cliente";

        public string Name { get; set; } = string.Empty;
        public string Lastname { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public int Age { get; set; } = 0;

        public ICommand SaveCommand { get; }
        public ICommand CancelCommand { get; }

        public ClientDetailViewModel(IClientService svc)
        {
            _svc = svc;

            SaveCommand = new Command(async () => await SaveAsync());
            CancelCommand = new Command(async () => await Shell.Current.GoToAsync(".."));
        }

        public async void OnNavigatedTo()
        {
            if (ClientId > 0)
            {
                var client = await _svc.GetClientByIdAsync(ClientId);
                if (client != null)
                {
                    Title = "Editar Cliente";
                    Name = client.Name;
                    Lastname = client.Lastname;
                    Address = client.Address;
                    Age = client.Age;
                    OnPropertyChanged(nameof(Name));
                    OnPropertyChanged(nameof(Lastname));
                    OnPropertyChanged(nameof(Address));
                    OnPropertyChanged(nameof(Age));
                    OnPropertyChanged(nameof(Title));
                }
            }
        }

        private async Task SaveAsync()
        {
            var client = new Client
            {
                Id = ClientId,
                Name = Name,
                Lastname = Lastname,
                Address = Address,
                Age = Age
            };

            if (ClientId > 0)
                await _svc.UpdateClientAsync(client);
            else
                await _svc.AddClientAsync(client);

            await Shell.Current.GoToAsync("..");
        }
    }
}
