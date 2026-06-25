using System.Windows;
using CompanyCRM.Infrastructure;
using MainWindow = CompanyCRM.MVVM.Views.MainWindow;

namespace CompanyCRM
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            Bootstrapper.Initialize();
            var mainWindow = Bootstrapper.Resolve<MainWindow>();;
            mainWindow.Show();
        }

        protected override void OnExit(ExitEventArgs e)
        {
            Bootstrapper.Dispose();
            base.OnExit(e);
        }
    }
}