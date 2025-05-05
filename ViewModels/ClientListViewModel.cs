using BTGClientManager.Models;
using BTGClientManager.Services;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace BTGClientManager.ViewModels
{
  public class ClientListViewModel : BaseViewModel
{
    private readonly IClientService _svc;
    public ObservableCollection<Client> Clients { get; } = new();

    public ICommand RefreshCommand { get; }
    public ICommand AddClientCommand    { get; }
    public ICommand EditClientCommand   { get; }
    public ICommand DeleteClientCommand { get; }

    public ClientListViewModel(IClientService svc)
    {
        _svc = svc;

        RefreshCommand       = new Command(async () => await LoadClients());
        AddClientCommand     = new Command(async () => await Shell.Current.GoToAsync("ClientDetailPage"));
        EditClientCommand    = new Command<Client>(async c => await EditClient(c));
        DeleteClientCommand  = new Command<Client>(async c => await DeleteClient(c));

        Task.Run(LoadClients);
    }

    private async Task LoadClients()
    {
        if (IsBusy) return;
        IsBusy = true;
        try
        {
            Clients.Clear();
            foreach (var c in await _svc.GetAllClientsAsync())
                Clients.Add(c);
        }
        finally { IsBusy = false; }
    }

    private async Task EditClient(Client? client)
    {
        if (client is null) return;
        await Shell.Current.GoToAsync("ClientDetailPage",
            new Dictionary<string, object> { ["ClientId"] = client.Id });
    }

    private async Task DeleteClient(Client? client)
    {
        if (client is null) return;

        bool ok = await Shell.Current.DisplayAlert(
            "Confirmação", $"Excluir {client.Name} {client.Lastname}?", "Sim", "Não");

        if (!ok) return;

        await _svc.DeleteClientAsync(client.Id);
        await LoadClients();
    }
}

}