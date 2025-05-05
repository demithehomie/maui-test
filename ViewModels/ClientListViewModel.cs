using BTGClientManager.Models;
using BTGClientManager.Services;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace BTGClientManager.ViewModels
{
    public class ClientListViewModel : BaseViewModel
    {
        private readonly IClientService _clientService;
        private readonly IServiceProvider _serviceProvider;
        private ObservableCollection<Client> _clients;
        private Client? _selectedClient = new Client();

        public ObservableCollection<Client> Clients
        {
            get => _clients;
            set => SetProperty(ref _clients, value);
        }

        public Client SelectedClient
        {
            get => _selectedClient!;
            set => SetProperty(ref _selectedClient, value);
        }

        public ICommand AddClientCommand { get; }
        public ICommand EditClientCommand { get; }
        public ICommand DeleteClientCommand { get; }
        public ICommand RefreshCommand { get; }

        public ClientListViewModel(IClientService clientService, IServiceProvider serviceProvider)
        {
            _clientService = clientService;
            _serviceProvider = serviceProvider;
            _clients = new ObservableCollection<Client>();
            _selectedClient = null!;

            AddClientCommand = new Command(async () => await AddClient());
            EditClientCommand = new Command(async () => await EditClient(), () => SelectedClient != null);
            DeleteClientCommand = new Command(async () => await DeleteClient(), () => SelectedClient != null);
            RefreshCommand = new Command(LoadClients);

            LoadClients();
        }

        private void LoadClients()
        {
            IsBusy = true;
            try
            {
                Clients.Clear();
                var clients = _clientService.GetAllClients();
                foreach (var client in clients)
                {
                    Clients.Add(client);
                }
            }
            finally
            {
                IsBusy = false;
            }
        }

        private async Task AddClient()
        {
            await Shell.Current.GoToAsync("ClientDetailPage");
        }

        private async Task EditClient()
        {
            if (SelectedClient == null)
                return;

            var parameters = new Dictionary<string, object>
            {
                { "ClientId", SelectedClient.Id }
            };

            await Shell.Current.GoToAsync("ClientDetailPage", parameters);
        }

        private async Task DeleteClient()
        {
            if (SelectedClient == null)
                return;

            bool answer = await Shell.Current.CurrentPage.DisplayAlert(
                "Confirmação",
                $"Deseja realmente excluir o cliente {SelectedClient.Name} {SelectedClient.Lastname}?",
                "Sim", "Não");

            if (answer)
            {
                _clientService.DeleteClient(SelectedClient.Id);
                LoadClients();
            }
        }
    }
}