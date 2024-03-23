using DeepWork.ViewModels.Pages;
using DeepWork.ViewModels.Windows;
using Microsoft.UI;
using Microsoft.UI.Composition.SystemBackdrops;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media;
using System.Linq;
using Windows.UI;

namespace DeepWork.Views.Windows
{
	public sealed partial class SignupWindow : Window
	{
		public SignupWindowViewModel ViewModel { get; set; }
		public SignupWindow()
		{
			this.InitializeComponent();

			ViewModel = new SignupWindowViewModel();
			(Content as FrameworkElement).ActualThemeChanged += NavigationWindow_ActualThemeChanged;

			ExtendsContentIntoTitleBar = true;
			if (ExtendsContentIntoTitleBar)
				SetTheme();

			if (MicaController.IsSupported())
				SystemBackdrop = new MicaBackdrop();
		}

		private void NavigationWindow_ActualThemeChanged(FrameworkElement sender, object args)
		{
			if (ExtendsContentIntoTitleBar)
				SetTheme();
		}

		private void SetTheme()
		{
			switch ((Content as FrameworkElement).ActualTheme)
			{
				case ElementTheme.Light:
					AppWindow.TitleBar.ButtonHoverBackgroundColor = new Color { A = 10, B = 0, G = 0, R = 0 };
					AppWindow.TitleBar.ButtonPressedBackgroundColor = new Color { A = 20, B = 0, G = 0, R = 0 };
					AppWindow.TitleBar.ButtonForegroundColor = Colors.Black;
					AppWindow.TitleBar.ButtonHoverForegroundColor = Colors.Black;
					break;

				case ElementTheme.Dark:
					AppWindow.TitleBar.ButtonHoverBackgroundColor = new Color { A = 10, B = 255, G = 255, R = 255 };
					AppWindow.TitleBar.ButtonPressedBackgroundColor = new Color { A = 20, B = 255, G = 255, R = 255 };
					AppWindow.TitleBar.ButtonForegroundColor = Colors.White;
					AppWindow.TitleBar.ButtonHoverForegroundColor = Colors.White;
					break;
			}
		}

		private void AccountListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			if (e.AddedItems.Count != 0)
			{
				ViewModel.SelectedAccountId = (e.AddedItems.First() as AccountViewModel).Id;
				LoginButton.IsEnabled = true;
			}
		}
	}
}
