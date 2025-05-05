using BTGClientManager.ViewModels;

namespace BTGClientManager.Views
{
   public partial class ClientDetailPage : ContentPage
{
    private readonly ClientDetailViewModel _viewModel;

    public ClientDetailPage(ClientDetailViewModel viewModel)
    {
        InitializeComponent();
        _viewModel = viewModel;
        BindingContext = _viewModel;
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        _viewModel.OnNavigatedTo();
    }
}

}