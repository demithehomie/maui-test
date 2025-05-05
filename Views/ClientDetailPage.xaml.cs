using BTGClientManager.ViewModels;

namespace BTGClientManager.Views
{
    public partial class ClientDetailPage : ContentPage
    {
        public ClientDetailPage(ClientDetailViewModel viewModel)
        {
            InitializeComponent();
            BindingContext = viewModel;
        }
    }
}