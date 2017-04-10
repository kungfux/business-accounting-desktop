using System;
using System.Configuration;
using System.Reflection;
using System.Windows;
using XDatabase;

namespace BusinessAccounting
{
    public partial class App
    {
        public static readonly XQuerySqlite Sqlite = new XQuerySqlite();

        private void App_OnStartup(object sender, StartupEventArgs e)
        {
            var connectionString = $@"Data Source={Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData)}\{Assembly.GetExecutingAssembly().GetName().Name}\ba.sqlite;" +
                "Version=3;UTF8Encoding=True;foreign keys=true;FailIfMissing=true;";

            try
            {
                connectionString = ConfigurationManager.ConnectionStrings["SqliteConnection"].ConnectionString;
            }
            catch (NullReferenceException)
            {
            }

            if (Sqlite.TestConnection(connectionString))
            {
                Sqlite.ConnectionString = connectionString;
            }
            else
            {
                MessageBox.Show($"Не удалось установить соединение с базой данных.{Environment.NewLine}Детали: {Sqlite.LastErrorMessage}", 
                    "Business Accounting", MessageBoxButton.OK, MessageBoxImage.Stop);
                Current.Shutdown();
            }
        }
    }
}
