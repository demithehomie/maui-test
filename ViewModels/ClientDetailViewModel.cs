using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using BTGClientManager.Models;
using BTGClientManager.Services;
using Microsoft.Maui.Controls;   //  para Command

namespace BTGClientManager.ViewModels
{
   public class ClientDetailViewModel : BaseViewModel, IQueryAttributable
{
    private readonly IClientService _svc;
    public Client Client { get; private set; } = new();

    public ICommand SaveCommand   { get; }
    public ICommand CancelCommand { get; }

    public ClientDetailViewModel(IClientService svc)
    {
        _svc = svc;
        SaveCommand   = new Command(async () => await Save());
        CancelCommand = new Command(async () => await Shell.Current.GoToAsync(".."));
    }

    public async void ApplyQueryAttributes(IDictionary<string, object> query)
    {
        if (query.TryGetValue("ClientId", out var idObj) && idObj is int id && id > 0)
            Client = await _svc.GetClientByIdAsync(id) ?? new Client();
        OnPropertyChanged(nameof(Client));            // avisa o binding
    }

    private async Task Save()
    {
        if (Client.Id == 0) await _svc.AddClientAsync(Client);
        else                await _svc.UpdateClientAsync(Client);

        await Shell.Current.GoToAsync("..");
    }
}

}
