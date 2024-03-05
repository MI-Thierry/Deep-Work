using DeepWork.Services;
using DeepWork.Views.Windows;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.UI.Xaml;
using System;

namespace DeepWork
{
    public partial class App : Application
    {
        private INavigationWindow _window;
        private static IServiceProvider _serviceProvider;
        public App()
        {
            this.InitializeComponent();
        }

        /// <summary>
        /// It gets the services from service provider
        /// </summary>
        /// <typeparam name="T">Type of service you want to get</typeparam>
        /// <returns>Requested service</returns>
        public static T GetService<T>()
        {
            return _serviceProvider.GetService<T>();
        }

        protected override void OnLaunched(LaunchActivatedEventArgs args)
        {
            // Initializing application's service provider.
            IServiceCollection services = new ServiceCollection();
            services.AddSingleton<AccountManagementServices>();
            services.AddTransient<INavigationWindow, MainWindow>();
            _serviceProvider = services.BuildServiceProvider();

            // Create the MainWindow.
            _window = GetService<INavigationWindow>();
            _window.ActivateWindow();
        }

    }
}
