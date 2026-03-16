using Microsoft.Extensions.Configuration;
using System.IO;
using System.Windows;
using TestTaskManager.Settings;

namespace TestTaskManager
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public IConfiguration Configuration { get; private set; }

        public ProjectSettings ProjectSettings { get; private set; }

        protected override void OnStartup(StartupEventArgs e)
        {
            var builder = new ConfigurationBuilder()
        .SetBasePath(Directory.GetCurrentDirectory())
        .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);

            Configuration = builder.Build();

            // Привязка к классу
            Configuration.Bind("Project", ProjectSettings = new ProjectSettings());

            base.OnStartup(e);
        }
    }

}
