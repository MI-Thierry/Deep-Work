using Microsoft.UI;
using Microsoft.UI.Windowing;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using System;
using System.Runtime.InteropServices;
using Windows.Foundation;
using WinRT.Interop;

namespace DeepWork.Helpers
{
	public static class WindowHelper
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

		[DllImport("user32.dll")]
		private static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);

		[DllImport("user32.dll")]
		[return: MarshalAs(UnmanagedType.Bool)]
		private static extern bool SetForegroundWindow(IntPtr hWnd);

		public static void ShowWindow(Window window)
		{
			IntPtr hwnd = WindowNative.GetWindowHandle(window);
			ShowWindow(hwnd, 0x00000009);
			SetForegroundWindow(hwnd);
		}
	}
}
