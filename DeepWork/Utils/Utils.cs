using Microsoft.UI;
using Microsoft.UI.Windowing;
using Microsoft.UI.Xaml;
using System;
using WinRT.Interop;

namespace DeepWork.Utils
{
	public static class Utils
	{
		public static DateTime FirstDateOfWeek(DateTime date)
		{
			return date.AddDays((date.DayOfWeek == DayOfWeek.Sunday) ? -6 : -((int)date.DayOfWeek - 1));
		}
		public static DateTime LastDateOfWeek(DateTime date)
		{
			var firstDate = FirstDateOfWeek(date);
			return firstDate.AddDays(6);
		}
		public static DateTime FirstDateOfMonth(DateTime date)
		{
			return date.AddDays(-(date.Day - 1));
		}
		public static DateTime LastDateOfMonth(DateTime date)
		{
			var firstDate = FirstDateOfMonth(date);
			return firstDate.AddDays(DateTime.DaysInMonth(firstDate.Year, firstDate.Month) - 1);
		}
		public static DateTime FirstDateOfYear(DateTime date)
		{
			return date.AddDays(-(date.DayOfYear - 1));
		}
		public static DateTime LastDayOfYear(DateTime date)
		{
			var firstDate = FirstDateOfYear(date);
			return firstDate.AddDays(DateTime.IsLeapYear(firstDate.Year) ? 365 : 364);
		}
		public static AppWindow GetAppWindowForCurrentWindow(Window window)
		{
			IntPtr hWnd = WindowNative.GetWindowHandle(window);
			WindowId winId = Win32Interop.GetWindowIdFromWindow(hWnd);
			return AppWindow.GetFromWindowId(winId);
		}
	}
}
