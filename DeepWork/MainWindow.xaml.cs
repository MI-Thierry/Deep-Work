using Microsoft.UI.Composition.SystemBackdrops;
using Microsoft.UI;
using Microsoft.UI.Xaml;
using DeepWork.Utils;
using Microsoft.UI.Windowing;
using WinRT;
using DeepWork.MVVM.Views;
using Microsoft.UI.Xaml.Media.Animation;

namespace DeepWork
{
    public sealed partial class MainWindow : Window
    {
        WindowsSystemDispatcherQueueHelper m_wsdqHelper; // See below for implementation.
        MicaController m_backdropController;
        SystemBackdropConfiguration m_configurationSource;
        AppWindow m_AppWindow;
        AppWindowTitleBar m_TitleBar;
        public MainWindow()
        {
            this.InitializeComponent();
            m_AppWindow = Utils.Utils.GetAppWindowForCurrentWindow(this);
            m_TitleBar = m_AppWindow.TitleBar;
            m_TitleBar.ExtendsContentIntoTitleBar = true;
            m_TitleBar.ButtonBackgroundColor = Colors.Transparent;
            TrySetSystemBackdrop();

            ContentFrame.Navigate(typeof(NavigationViewPage), null, new EntranceNavigationTransitionInfo());
        }
        bool TrySetSystemBackdrop()
        {
            if (MicaController.IsSupported())
            {
                m_wsdqHelper = new WindowsSystemDispatcherQueueHelper();
                m_wsdqHelper.EnsureWindowsSystemDispatcherQueueController();

                // Create the policy object.
                m_configurationSource = new SystemBackdropConfiguration();
                this.Activated += Window_Activated;
                this.Closed += Window_Closed;
                ((FrameworkElement)this.Content).ActualThemeChanged += Window_ThemeChanged;

                // Initial configuration state.
                m_configurationSource.IsInputActive = true;
                SetConfigurationSourceTheme();

                m_backdropController = new MicaController();

                // Enable the system backdrop.
                // Note: Be sure to have "using WinRT;" to support the Window.As<...>() call.
                m_backdropController.AddSystemBackdropTarget(this.As<Microsoft.UI.Composition.ICompositionSupportsSystemBackdrop>());
                m_backdropController.SetSystemBackdropConfiguration(m_configurationSource);
                return true; // succeeded
            }

            return false; // Mica is not supported on this system
        }

        private void Window_Activated(object sender, WindowActivatedEventArgs args)
        {
            m_configurationSource.IsInputActive = args.WindowActivationState != WindowActivationState.Deactivated;
        }

        private void Window_Closed(object sender, WindowEventArgs args)
        {
            // Make sure any Mica/Acrylic controller is disposed
            // so it doesn't try to use this closed window.
            if (m_backdropController != null)
            {
                m_backdropController.Dispose();
                m_backdropController = null;
            }
            this.Activated -= Window_Activated;
            m_configurationSource = null;
        }

        private void Window_ThemeChanged(FrameworkElement sender, object args)
        {
            if (m_configurationSource != null)
            {
                SetConfigurationSourceTheme();
            }
        }

        private void SetConfigurationSourceTheme()
        {
            switch (((FrameworkElement)this.Content).ActualTheme)
            {
                case ElementTheme.Dark:
                    m_configurationSource.Theme = SystemBackdropTheme.Dark;
                    m_TitleBar.ButtonHoverBackgroundColor = new Windows.UI.Color { A = 10, B = 255, G = 255, R = 255 };
                    m_TitleBar.ButtonPressedBackgroundColor = new Windows.UI.Color { A = 20, B = 255, G = 255, R = 255 };
                    m_TitleBar.ButtonForegroundColor = Colors.White;
                    break;
                case ElementTheme.Light:
                    m_configurationSource.Theme = SystemBackdropTheme.Light;
                    m_TitleBar.ButtonHoverBackgroundColor = new Windows.UI.Color { A = 10, B = 0, G = 0, R = 0 };
                    m_TitleBar.ButtonPressedBackgroundColor = new Windows.UI.Color { A = 20, B = 0, G = 0, R = 0 };
                    m_TitleBar.ButtonForegroundColor = Colors.Black;
                    break;
                case ElementTheme.Default:
                    m_configurationSource.Theme = SystemBackdropTheme.Default;
                    break;
            }
        }
    }
}