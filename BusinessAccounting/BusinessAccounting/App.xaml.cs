using BusinessAccounting.Model;
using System.Windows;

namespace BusinessAccounting
{
    public partial class App : Application
    {
        private void Application_Startup(object sender, StartupEventArgs e)
        {
            new DatabaseDefaults().InitDatabaseDefaults();
        }
    }
}
