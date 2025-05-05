using Microsoft.Maui.Controls;

namespace BTGClientManager
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();
            Routing.RegisterRoute(nameof(Views.ClientDetailPage), typeof(Views.ClientDetailPage));
        }
    }
}
