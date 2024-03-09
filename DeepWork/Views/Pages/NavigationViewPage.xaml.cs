using DeepWork.ViewModels.Pages;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media.Animation;
using Microsoft.UI.Xaml.Navigation;
using System;
using System.Linq;

namespace DeepWork.Views.Pages;

public sealed partial class NavigationViewPage : Page
{
    public NavigationViewViewModel ViewModel { get; set; }
    public double NavViewCompactModeThresholdWidth { get; set; }

    public NavigationViewPage()
    {
        ViewModel = App.GetService<NavigationViewViewModel>();
        this.InitializeComponent();
    }

    private void ContentFrame_NavigationFailed(object sender, NavigationFailedEventArgs e)
    {
        throw new InvalidOperationException("Failed to navigate to specified page.");
    }

    private void NavigationView_ItemInvoked(NavigationView sender, NavigationViewItemInvokedEventArgs args)
    {
        if (args.IsSettingsInvoked)
            ContentFrame.Navigate(typeof(SettingsPage), null, args.RecommendedNavigationTransitionInfo);
        else if (args.InvokedItemContainer != null)
        {
            Type pageType = args.InvokedItemContainer.Tag as Type;
            ContentFrame.Navigate(pageType, null, args.RecommendedNavigationTransitionInfo);
        }
    }

    private void NavigationView_BackRequested(NavigationView sender, NavigationViewBackRequestedEventArgs args)
    {
        ContentFrame.GoBack();
    }

	private void NavigationView_Loaded(object sender, RoutedEventArgs e)
	{
        Type pageType = ViewModel.MenuItems.First().Tag as Type;
		ContentFrame.Navigate(pageType, null, new EntranceNavigationTransitionInfo());
    }
}
