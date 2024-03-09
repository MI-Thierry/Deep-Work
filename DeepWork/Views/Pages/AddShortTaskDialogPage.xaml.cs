using DeepWork.ViewModels.Pages;
using Microsoft.UI.Xaml.Controls;

namespace DeepWork.Views.Pages
{
	public sealed partial class AddShortTaskDialogPage : Page
	{
		public ShortTaskViewModel ViewModel { get; private set; }
		public AddShortTaskDialogPage()
		{
			ViewModel = new ShortTaskViewModel();
			this.InitializeComponent();
		}
	}
}
