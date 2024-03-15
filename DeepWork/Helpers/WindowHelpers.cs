using Microsoft.UI.Xaml;
using Microsoft.UI;
using System;
using WinRT.Interop;
using Microsoft.UI.Windowing;
using Microsoft.UI.Xaml.Controls;
using System.Threading.Tasks;
using Windows.Foundation;

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

		public static IAsyncOperation<ContentDialogResult> WarningDialog(string message, XamlRoot xamlRoot)
		{
			ContentDialog TaskDialog = new()
			{
				Title = "Warning",
				Content = message,
				CloseButtonText = "Ok",
				DefaultButton = ContentDialogButton.Close,
				XamlRoot = xamlRoot
			};
			return TaskDialog.ShowAsync();
		}
	}
}
