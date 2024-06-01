using Microsoft.UI;
using Microsoft.UI.Composition.SystemBackdrops;
using Microsoft.UI.Windowing;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Media.Animation;
using System;
using Windows.UI;
using Microsoft.UI.Xaml.Navigation;
using Microsoft.UI.Input;
using Windows.Graphics;
using DeepWork.Winui.Helpers;
using System.Runtime.InteropServices;
using WinRT.Interop;

namespace DeepWork.Winui.Windows;

public sealed partial class MainWindow : Window, INavigableWindow
{
	private readonly nint _hWnd = nint.Zero;
	private readonly SubclassProc _subclassDelegate;
	public double NavViewCompactModeThresholdWidth { get; set; }
	public int MinHeight { get; set; }
	public int MinWidth { get; set; }
    public int MaxHeight { get; set; } = int.MaxValue;
    public int MaxWidth { get; set; } = int.MaxValue;

	public MainWindow()
	{
		this.InitializeComponent();
		TryCustomizeWindow();

		// Initializing variables which will be used to maintain minimum size of window
		_hWnd = WindowNative.GetWindowHandle(this);
		_subclassDelegate = new SubclassProc(WindowSubClass);
		bool bReturn = WindowHelpers.SetWindowSubclass(_hWnd, _subclassDelegate, 0, 0);
	}

	private bool TryCustomizeWindow()
	{
		if (MicaController.IsSupported())
		{
			SystemBackdrop = new MicaBackdrop();
			ExtendsContentIntoTitleBar = true;
			AppWindow.TitleBar.PreferredHeightOption = TitleBarHeightOption.Standard;
			SetTheme();

			if (Content is FrameworkElement element)
				element.ActualThemeChanged += (elem, o) => SetTheme();

			return true;
		}
		return false;
	}

	private void SetTheme()
	{
		switch ((Content as FrameworkElement)?.ActualTheme)
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

	private void ContentFrame_NavigationFailed(object sender, NavigationFailedEventArgs e)
	{
		throw new InvalidOperationException("Failed to navigate to specified page.");
	}

	private int WindowSubClass(nint hWnd, uint uMsg, nint wParam, nint lParam, nint uIdSubClass, uint dwRefData)
	{
		switch (uMsg)
		{
			case WindowHelpers.WM_GETMINMAXINFO:
				MINMAXINFO mmi = (MINMAXINFO)Marshal.PtrToStructure(lParam, typeof(MINMAXINFO))!;
				mmi.ptMinTrackSize.X = MinWidth;
				mmi.ptMinTrackSize.Y = MinHeight;
				mmi.ptMaxTrackSize.X = MaxWidth;
				mmi.ptMaxTrackSize.Y = MaxHeight;
				Marshal.StructureToPtr(mmi, lParam, false);
				return 0;
		}
		return WindowHelpers.DefSubclassProc(hWnd, uMsg, wParam, lParam);
	}

	public void NavigateTo(Type pageType, NavigationTransitionInfo navigationTransitionInfo)
	{
		ContentFrame.Navigate(pageType, null, navigationTransitionInfo);
	}

	public void SetDragRegion(RectInt32[] rectArray)
	{
		InputNonClientPointerSource nonClientPointerSource = InputNonClientPointerSource.GetForWindowId(AppWindow.Id);
		nonClientPointerSource.SetRegionRects(NonClientRegionKind.Passthrough, rectArray);
	}
}