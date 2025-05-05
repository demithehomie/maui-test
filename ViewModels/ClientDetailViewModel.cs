using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using BTGClientManager.Models;
using BTGClientManager.Services;
using Microsoft.Maui.Controls;   //  para Command

namespace BTGClientManager.ViewModels
{
    public class ClientDetailViewModel : INotifyPropertyChanged
    {
        private readonly IClientService _clientService;
        private Client _client = new();

        public Client Client
        {
            get => _client;
            set { _client = value; OnPropertyChanged(); }
        }

        // ────────────  WRAPPERS  ────────────
        public string Title
        {
            get => Client.Title ?? string.Empty;
            set { if (Client.Title != value) { Client.Title = value; OnPropertyChanged(); } }
        }

        public string Name
        {
            get => Client.Name;
            set { if (Client.Name != value) { Client.Name = value; OnPropertyChanged(); } }
        }

        public string Lastname
        {
            get => Client.Lastname;
            set { if (Client.Lastname != value) { Client.Lastname = value; OnPropertyChanged(); } }
        }

        public int Age
        {
            get => Client.Age;
            set { if (Client.Age != value) { Client.Age = value; OnPropertyChanged(); } }
        }

        public string Address
        {
            get => Client.Address;
            set { if (Client.Address != value) { Client.Address = value; OnPropertyChanged(); } }
        }

        // ────────────  COMMANDS  ────────────
        public ICommand SaveCommand  { get; }
        public ICommand CancelCommand { get; }

        public ClientDetailViewModel(IClientService clientService)
        {
            _clientService = clientService;

            // comandos
            SaveCommand   = new Command(Save);
            CancelCommand = new Command(Cancel);

            // novo cliente vazio para edição
            Client = new Client();
        }

        public void Load(int id) => Client = _clientService.GetClientById(id);

        void Save()
        {
            if (Client.Id == 0) _clientService.AddClient(Client);
            else                _clientService.UpdateClient(Client);
        }

        void Cancel() => Shell.Current.GoToAsync("..");   // navega para trás

        public event PropertyChangedEventHandler? PropertyChanged;
        void OnPropertyChanged([CallerMemberName] string? propName = null) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propName));
    }
}
