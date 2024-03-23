using Microsoft.UI.Xaml.Controls;

namespace DeepWork.Views.Pages
{
	public sealed partial class SignInDialogPage : Page
	{
        public string Username { get; set; }
        public string Password { get; set; }
        public SignInDialogPage()
		{
			this.InitializeComponent();
		}
	}
}
