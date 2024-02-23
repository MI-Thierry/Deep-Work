using DeepWork.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.UI.Xaml;
using System;

namespace DeepWork
{
    public partial class App : Application
    {
        public static IServiceProvider ServiceProvider { get; private set; }
        public App()
        {
            this.InitializeComponent();
        }

        protected override void OnLaunched(LaunchActivatedEventArgs args)
        {
            // Initializing application's service provider.
            IServiceCollection services = new ServiceCollection();
            services.AddSingleton<AccountServices>();
            ServiceProvider = services.BuildServiceProvider();

            // Create the MainWindow.
            m_window = new MainWindow();
            m_window.Activate();
        }

        private Window m_window;
    }
}
