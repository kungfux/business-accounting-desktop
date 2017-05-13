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
        public static string DatabaseFilePath { get; private set; }
        public static string BackupRemoteFolderId { get; private set; }

        private void App_OnStartup(object sender, StartupEventArgs e)
        {
            var defaultPath = DatabasePath = $@"{Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData)}\{Assembly.GetExecutingAssembly().GetName().Name}";
            DatabaseFilePath = $@"{defaultPath}\ba.sqlite";
            var connectionString = $@"Data Source={DatabaseFilePath};Version=3;UTF8Encoding=True;foreign keys=true;FailIfMissing=true;";

            try
            {
                connectionString = ConfigurationManager.ConnectionStrings["SqliteConnection"].ConnectionString;
                DatabaseFilePath = ExtractFilePathFromConnectionString(connectionString);
                DatabasePath = ExtractPathFromFilePath(DatabaseFilePath);
            }
            catch (NullReferenceException)
            {
                // ignored
            }

            try
            {
                BackupRemoteFolderId = ConfigurationManager.AppSettings["BackupRemoteFolderId"];
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

        private static string ExtractFilePathFromConnectionString(string connectionString)
        {
            var conn = new System.Data.SQLite.SQLiteConnectionStringBuilder(connectionString);
            var dataSource = conn.DataSource;
            if (!dataSource.Contains(@"\"))
            {
                dataSource = $@"{AppDomain.CurrentDomain.BaseDirectory}\{dataSource}";
            }
            return dataSource;
        }

        private static string ExtractPathFromFilePath(string filePath)
        {
            if (filePath.Contains(@"\"))
            {
                return filePath.Substring(0, filePath.LastIndexOf(@"\", StringComparison.InvariantCulture));
            }
            return null;
        }
    }
}
