using Autofac;
using System.Windows;

namespace ConfigurableMessagaeLogger
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public IContainer Container { get; private set; }

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            var builder = new ContainerBuilder();
            builder.RegisterType<LoggerService>().SingleInstance().As<ILoggerService>();
            builder.RegisterType<MainWindow>().AsSelf();

            Container = builder.Build();
            var mainWindow = Container.Resolve<MainWindow>();
            mainWindow.Show();
            ;
        }
    }
}
