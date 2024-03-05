using DeepWork.MVVM.Models;
using DeepWork.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.Windows.AppLifecycle;

namespace DeepWork.MVVM.Views
{
    public sealed partial class AccountCreationPage : Page
    {
        public AccountCreationPage()
        {
            this.InitializeComponent();
        }

        private void CreateAccountButton_Click(object sender, RoutedEventArgs e)
        {
            AccountManagementServices services = App.ServiceProvider.GetRequiredService<AccountManagementServices>();
            string name = firstName.Text + " " + lastName.Text;
            string psw = password.Password;
            services.CreateAccount(name, psw);
            //_ = AppInstance.Restart("restarting");
        }
    }
}
