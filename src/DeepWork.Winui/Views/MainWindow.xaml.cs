using DeepWork.Winui.ViewModels;
using Microsoft.UI;
using Microsoft.UI.Composition.SystemBackdrops;
using Microsoft.UI.Windowing;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Media.Animation;
using System.Linq;
using System;
using Windows.UI;
using DeepWork.Winui.Views.Pages;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Navigation;
using Microsoft.UI.Input;
using Windows.Graphics;
using Windows.Foundation;
using DeepWork.Winui.Helpers;
using System.Runtime.InteropServices;
using WinRT.Interop;

namespace DeepWork.Winui.Views;

public sealed partial class MainWindow : Window
{
    private readonly IntPtr _hWnd = IntPtr.Zero;
    private readonly SubclassProc _subclassDelegate;
    public MainWindowViewModel ViewModel { get; set; }
    public double NavViewCompactModeThresholdWidth { get; set; }
    public int MinHeight { get; set; } = 300;
    public int MinWidth { get; set; } = 400;

    public MainWindow()
    {
        ViewModel = new MainWindowViewModel();
        this.InitializeComponent();
        TryCustomizeWindow();

        // Initializing variables which will be used to maintain minimum size of window
        _hWnd = WindowNative.GetWindowHandle(this);
        _subclassDelegate = new SubclassProc(WindowSubClass);
        bool bReturn = WindowHelpers.SetWindowSubclass(_hWnd, _subclassDelegate, 0, 0);

        BackRequestButton.PointerEntered += BackRequestButton_PointerEntered;
        BackRequestButton.PointerExited += BackRequestButton_PointerExited;
        BackRequestButton.PointerPressed += BackRequestButton_PointerPressed;
    }

    private void BackRequestButton_PointerEntered(object sender, PointerRoutedEventArgs e)
    {
        AnimatedIcon.SetState(BackRequestButtonIcon, "PointerOver");
    }

    private void BackRequestButton_PointerExited(object sender, PointerRoutedEventArgs e)
    {
        AnimatedIcon.SetState(BackRequestButtonIcon, "Normal");
    }

    private void BackRequestButton_PointerPressed(object sender, PointerRoutedEventArgs e)
    {
        AnimatedIcon.SetState(BackRequestButton, "Pressed");
    }

    private bool TryCustomizeWindow()
    {
        if (MicaController.IsSupported())
        {
            SystemBackdrop = new MicaBackdrop();
            ExtendsContentIntoTitleBar = true;
            AppWindow.TitleBar.PreferredHeightOption = TitleBarHeightOption.Standard;

            AppTitleBar.Loaded += AppTitleBar_Loaded;
            AppTitleBar.SizeChanged += AppTitleBar_SizeChanged;
            if (Content is FrameworkElement element)
                element.ActualThemeChanged += (elem, o) => SetTheme();

            return true;
        }
        return false;
    }

    private void AppTitleBar_SizeChanged(object sender, SizeChangedEventArgs e)
    {
        if (ExtendsContentIntoTitleBar)
        {
            // Set initial interactive regions
            SetRegionsForCustomTitleBar();
            SetTheme();
        }
    }

    private void SetRegionsForCustomTitleBar()
    {
        // Specify the interactive regions of the title bar.
        double scaleAdjustment = AppTitleBar.XamlRoot.RasterizationScale;

        RightPaddingColumn.Width = new GridLength(AppWindow.TitleBar.RightInset / scaleAdjustment);
        LeftPaddingColumn.Width = new GridLength(AppWindow.TitleBar.LeftInset / scaleAdjustment);

        // Get rectangle around BackRequestButton control.
        GeneralTransform transform = BackRequestButton.TransformToVisual(null);
        Rect bounds = transform.TransformBounds(new Rect(0, 0, BackRequestButton.ActualWidth, BackRequestButton.ActualHeight));
        RectInt32 backRequestButtonRect = GetRect(bounds, scaleAdjustment);

        RectInt32[] rectArray = [backRequestButtonRect];

        InputNonClientPointerSource nonClientPointerSource = InputNonClientPointerSource.GetForWindowId(AppWindow.Id);
        nonClientPointerSource.SetRegionRects(NonClientRegionKind.Passthrough, rectArray);
    }

    private RectInt32 GetRect(Rect bounds, double scale)
    {
        return new RectInt32(
            _X: (int)Math.Round(bounds.X * scale),
            _Y: (int)Math.Round(bounds.Y * scale),
            _Width: (int)Math.Round(bounds.Width * scale),
            _Height: (int)Math.Round(bounds.Height * scale)
            );
    }

    private void AppTitleBar_Loaded(object sender, RoutedEventArgs e)
    {
        if (ExtendsContentIntoTitleBar)
        {
            // Set initial interactive regions
            SetRegionsForCustomTitleBar();

            SetTheme();
        }
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

    private void NavView_ItemInvoked(NavigationView sender, NavigationViewItemInvokedEventArgs args)
    {
        if (args.IsSettingsInvoked)
            ContentFrame.Navigate(typeof(SettingsPage), null, args.RecommendedNavigationTransitionInfo);
        else if (args.InvokedItemContainer != null)
        {
            Type pageType = (args.InvokedItemContainer.Tag as Type)!;
            ContentFrame.Navigate(pageType, null, args.RecommendedNavigationTransitionInfo);
        }
    }

    private void NavView_Loaded(object sender, RoutedEventArgs e)
    {
        if (ViewModel.MenuItems.First() is NavigationViewItem item)
            ContentFrame.Navigate(item.Tag as Type, null, new EntranceNavigationTransitionInfo());
        else
            throw new InvalidOperationException("The first element of MenuItems should be a NavigationViewItem");
    }

    private void ContentFrame_NavigationFailed(object sender, NavigationFailedEventArgs e)
    {
        throw new InvalidOperationException("Failed to navigate to specified page.");
    }

    private void BackRequestButton_Click(object sender, RoutedEventArgs e)
    {
        ContentFrame.GoBack();
        NavView.SelectedItem = ViewModel.MenuItems
            .FirstOrDefault(item => item.Tag as Type == ContentFrame.CurrentSourcePageType)
            ?? NavView.SettingsItem;
    }

    private int WindowSubClass(IntPtr hWnd, uint uMsg, nint wParam, IntPtr lParam, IntPtr uIdSubClass, uint dwRefData)
    {
        switch (uMsg)
        {
            case WindowHelpers.WM_GETMINMAXINFO:
                MINMAXINFO mmi = (MINMAXINFO)Marshal.PtrToStructure(lParam, typeof(MINMAXINFO))!;
                mmi.ptMinTrackSize.X = MinWidth;
                mmi.ptMinTrackSize.Y = MinHeight;
                Marshal.StructureToPtr(mmi, lParam, false);
                return 0;
        }
        return WindowHelpers.DefSubclassProc(hWnd, uMsg, wParam, lParam);
    }
}