using DeepWork.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media.Animation;
using Microsoft.UI.Xaml.Navigation;
using System;
using System.Collections.Generic;


namespace DeepWork.MVVM.Views
{
    public sealed partial class NavigationViewPage : Page
    {
        private Dictionary<string, Type> m_PageTypes = new()
        {
            { "settings", typeof(SettingsPage) },
            { "longTasks", typeof(LongTasksPage) },
            { "monitoring", typeof(MonitoringPage) },
            { "history", typeof(HistoryPage) },
            { "focusSession", typeof(FocusSessionPage) },
        };
        private double NavViewCompactModeThresholdWidth => NavView.CompactModeThresholdWidth;
        public NavigationViewPage()
        {
            this.InitializeComponent();
        }

        private void ContentFrame_NavigationFailed(object sender, NavigationFailedEventArgs e)
        {
            throw new Exception("Failed to load page " + e.SourcePageType.FullName);
        }
        private void NavView_Loaded(object sender, RoutedEventArgs e)
        {
            // Add handle fore ContentFrame navigation
            ContentFrame.Navigated += ContentFrame_Navigated;

            // Load the navigation view default page
            AccountServices services = App.ServiceProvider.GetRequiredService<AccountServices>();
            if (services.IsAccountAvailable)
            {
                NavView.SelectedItem = NavView.MenuItems[0];
                NavViewNavigate("longTasks", new EntranceNavigationTransitionInfo());
            }
            else
            {
                ContentFrame.Navigate(typeof(AccountCreationPage), null, new EntranceNavigationTransitionInfo());
            }
        }
        void NavViewNavigate(string itemTag, NavigationTransitionInfo transitionInfo)
        {
            Type page = m_PageTypes[itemTag];
            Type prevPage = ContentFrame.CurrentSourcePageType;

            if (page != prevPage)
                ContentFrame.Navigate(page, null, transitionInfo);
        }
        private void ContentFrame_Navigated(object sender, NavigationEventArgs e)
        {
            NavView.IsBackEnabled = ContentFrame.CanGoBack;
            if (ContentFrame.SourcePageType == typeof(SettingsPage))
            {
                // SettingsItem is not part of NavView.MenuItems, and doesn't have a Tag.
                NavView.SelectedItem = (NavigationViewItem)NavView.SettingsItem;
                NavView.Header = "Settings";
            }
            else if (ContentFrame.SourcePageType == typeof(AccountCreationPage))
            {
                // Create account page is not in navView items
                NavView.Header = "Create account";
            }
            else
                NavView.Header = null;
        }

        private void NavView_ItemInvoked(NavigationView sender, NavigationViewItemInvokedEventArgs args)
        {
            if (args.IsSettingsInvoked)
                NavViewNavigate("settings", args.RecommendedNavigationTransitionInfo);
            else if (args.InvokedItemContainer != null)
            {
                string navItemTag = args.InvokedItemContainer.Tag.ToString();
                NavViewNavigate(navItemTag, args.RecommendedNavigationTransitionInfo);
            }
        }
        private void NavView_BackRequested(NavigationView sender, NavigationViewBackRequestedEventArgs args)
        {
            TryGoBack();
        }
        private bool TryGoBack()
        {
            if (!ContentFrame.CanGoBack)
                return false;
            if (NavView.IsPaneOpen &&
                (NavView.DisplayMode == NavigationViewDisplayMode.Compact ||
                 NavView.DisplayMode == NavigationViewDisplayMode.Minimal))
                return false;
            ContentFrame.GoBack();
            return true;
        }
    }
}
