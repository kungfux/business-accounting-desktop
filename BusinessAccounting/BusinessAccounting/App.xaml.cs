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
        public static string DatabasePath { get; private set; }

        private void App_OnStartup(object sender, StartupEventArgs e)
        {
            var defaultPath = DatabasePath = $@"{Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData)}\{Assembly.GetExecutingAssembly().GetName().Name}";
            var connectionString = $@"Data Source={defaultPath}\ba.sqlite;Version=3;UTF8Encoding=True;foreign keys=true;FailIfMissing=true;";

            try
            {
                connectionString = ConfigurationManager.ConnectionStrings["SqliteConnection"].ConnectionString;
                DatabasePath = ExtractPathFromConnectionString(connectionString) ?? AppDomain.CurrentDomain.BaseDirectory;
            }
            catch (NullReferenceException)
            {
                // ignored
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

        private static string ExtractPathFromConnectionString(string connectionString)
        {
            var conn = new System.Data.SQLite.SQLiteConnectionStringBuilder(connectionString);
            var dataSource = conn.DataSource;
            if (dataSource.Contains(@"\"))
            {
                return dataSource.Substring(0, dataSource.LastIndexOf(@"\", StringComparison.InvariantCulture));
            }
            return null;
        }
    }
}
