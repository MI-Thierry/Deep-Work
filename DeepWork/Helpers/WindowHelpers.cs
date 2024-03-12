using Microsoft.UI.Xaml;
using Microsoft.UI;
using System;
using WinRT.Interop;
using Microsoft.UI.Windowing;
using Microsoft.UI.Xaml.Controls;

namespace DeepWork.Helpers
{
    public static class WindowHelpers
    {
		public static AppWindow GetAppWindow(Window window)
		{
			IntPtr hWnd = WindowNative.GetWindowHandle(window);
			WindowId winId = Win32Interop.GetWindowIdFromWindow(hWnd);
			return AppWindow.GetFromWindowId(winId);
		}

		public static async void WarningDialog(string message, XamlRoot xamlRoot)
		{
			ContentDialog TaskDialog = new()
			{
				Title = "Warning",
				Content = message,
				CloseButtonText = "Ok",
				DefaultButton = ContentDialogButton.Close,
				XamlRoot = xamlRoot
			};
			_ = await TaskDialog.ShowAsync();
		}
	}
}
