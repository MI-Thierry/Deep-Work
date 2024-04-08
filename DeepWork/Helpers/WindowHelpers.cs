﻿using Microsoft.UI;
using Microsoft.UI.Windowing;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using System;
using System.Runtime.InteropServices;
using Windows.Foundation;
using WinRT.Interop;

namespace DeepWork.Helpers
{
	public struct MINMAXINFO
	{
		public System.Drawing.Point ptReserved;
		public System.Drawing.Point ptMaxSize;
		public System.Drawing.Point ptMaxPosition;
		public System.Drawing.Point ptMinTrackSize;
		public System.Drawing.Point ptMaxTrackSize;
	};

	public delegate int SubclassProc(IntPtr hWnd, uint uMsg, IntPtr wParam, IntPtr lParam, IntPtr uIdSubClass, uint dwRefData);
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

		[DllImport("user32.dll")]
		private static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);

		[DllImport("user32.dll")]
		[return: MarshalAs(UnmanagedType.Bool)]
		private static extern bool SetForegroundWindow(IntPtr hWnd);

		[DllImport("Comctl32.dll", SetLastError = true)]
		public static extern bool SetWindowSubclass([In] IntPtr hWnd, [In] SubclassProc pfnSubClass, [In] uint uIdSubClass, [In] uint dwRefData);

		[DllImport("Comctl32.dll", SetLastError = true)]
		public static extern int DefSubclassProc([In] IntPtr hWnd, [In] uint uMsg, [In] IntPtr wParam, [In] IntPtr lParam);

		public const int WM_GETMINMAXINFO = 0x0024;
		public static void ShowWindow(Window window)
		{
			IntPtr hwnd = WindowNative.GetWindowHandle(window);
			ShowWindow(hwnd, 0x00000009);
			SetForegroundWindow(hwnd);
		}
	}
}