using Microsoft.UI.Xaml.Media.Animation;
using System;

namespace DeepWork.Services
{
    public interface INavigationWindow
    {
        public bool Navigate(Type pageType, object parameter, NavigationTransitionInfo transitionInfo);
        public void ActivateWindow();
    }
}
