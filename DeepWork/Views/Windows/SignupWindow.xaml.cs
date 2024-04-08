using DeepWork.Helpers;
using DeepWork.ViewModels.Pages;
using DeepWork.ViewModels.Windows;
using Microsoft.UI;
using Microsoft.UI.Composition.SystemBackdrops;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media;
using System;
using System.Linq;
using System.Runtime.InteropServices;
using Windows.UI;
using WinRT.Interop;

namespace DeepWork.Views.Windows
{
	public sealed partial class SignupWindow : Window
	{
		private readonly IntPtr _hWnd = IntPtr.Zero;
		private readonly SubclassProc _subclassDelegate;
		public SignupWindowViewModel ViewModel { get; set; }
		public SignupWindow()
		{
			this.InitializeComponent();

			_hWnd = WindowNative.GetWindowHandle(this);
			_subclassDelegate = new SubclassProc(WindowSubclass);
			bool bReturn = WindowHelpers.SetWindowSubclass(_hWnd, _subclassDelegate, 0, 0);

			ViewModel = new SignupWindowViewModel();
			(Content as FrameworkElement).ActualThemeChanged += NavigationWindow_ActualThemeChanged;

			ExtendsContentIntoTitleBar = true;
			if (ExtendsContentIntoTitleBar)
				SetTheme();

			if (MicaController.IsSupported())
				SystemBackdrop = new MicaBackdrop();
		}

		private int WindowSubclass(nint hWnd, uint uMsg, nint wParam, nint lParam, nint uIdSubClass, uint dwRefData)
		{
			switch (uMsg)
			{
				case WindowHelpers.WM_GETMINMAXINFO:
					MINMAXINFO mmi = (MINMAXINFO)Marshal.PtrToStructure(lParam, typeof(MINMAXINFO));
					mmi.ptMinTrackSize.X = 1280;
					mmi.ptMinTrackSize.Y = 640;
					mmi.ptMaxTrackSize.X = 1280;
					mmi.ptMaxTrackSize.Y = 640;
					Marshal.StructureToPtr(mmi, lParam, false);
					return 0;
			}
			return WindowHelpers.DefSubclassProc(hWnd, uMsg, wParam, lParam);
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
