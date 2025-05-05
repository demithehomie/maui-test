using BTGClientManager.ViewModels;

namespace BTGClientManager.Views
{
    public partial class ClientListPage : ContentPage
    {
        private readonly ClientListViewModel _viewModel;

        public ClientListPage(ClientListViewModel viewModel)
        {
            InitializeComponent();
            _viewModel = viewModel;
            BindingContext = _viewModel;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            _viewModel.RefreshCommand.Execute(null);
        }
    }
}