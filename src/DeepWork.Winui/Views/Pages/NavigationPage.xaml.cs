using DeepWork.Winui.Helpers;
using DeepWork.Winui.ViewModels;
using DeepWork.Winui.Windows;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media.Animation;
using Microsoft.UI.Xaml.Navigation;
using System;
using System.Linq;
using Windows.Foundation;
using Windows.Graphics;

namespace DeepWork.Winui.Views.Pages;
public sealed partial class NavigationPage : Page
{
	public NavigationViewModel ViewModel { get; private set; }
	public double NavViewCompactModeThresholdWidth { get; private set; } = 500;

	public NavigationPage()
	{
		ViewModel = new NavigationViewModel();
		this.InitializeComponent();
		AppTitleBar.Loaded += (sender, e) => SetRegionsForCustomTitleBar();
		AppTitleBar.SizeChanged += (sender, e) => SetRegionsForCustomTitleBar();
	}

	private void SetRegionsForCustomTitleBar()
	{
		// Specify the interactive regions of the title bar.
		Window window = ApplicationHost.GetElementsWindow(this);
		if (window is INavigableWindow navigationWindow)
		{
			// Undone: Correct the bug here
			double scaleAdjustment = AppTitleBar.XamlRoot.RasterizationScale;
			double navigationButtonsWidth = NavView.DisplayMode == NavigationViewDisplayMode.Minimal
				? (double)Application.Current.Resources["NavigationBackButtonWidth"] * 2
				: (double)Application.Current.Resources["NavigationBackButtonWidth"];

			RightPaddingColumn.Width = new GridLength(window.AppWindow.TitleBar.RightInset / scaleAdjustment);
			LeftPaddingColumn.Width = new GridLength(window.AppWindow.TitleBar.LeftInset / scaleAdjustment);
			NavigationButtonsColumn.Width = new GridLength(navigationButtonsWidth);

			// Get rectangle around NavigationButtonsColumn.
			Rect bounds = new(0, 0, (float)NavigationButtonsColumn.ActualWidth, (float)AppTitleBar.ActualHeight);
			RectInt32 navigationButtonsRect = WindowHelpers.GetRect(bounds, scaleAdjustment);

			RectInt32[] rectArray = [navigationButtonsRect];
			navigationWindow.SetDragRegion(rectArray);
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

	private void NavView_BackRequested(NavigationView sender, NavigationViewBackRequestedEventArgs args)
	{
		ContentFrame.GoBack();
		NavView.SelectedItem = ViewModel.MenuItems
			.FirstOrDefault(item => item.Tag as Type == ContentFrame.CurrentSourcePageType)
			?? NavView.SettingsItem;
	}
}
