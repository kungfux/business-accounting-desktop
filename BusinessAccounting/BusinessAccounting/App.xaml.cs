using BusinessAccounting.Properties;
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
        public static string ConnectionString { get; private set; }
        public static string DatabasePath { get; private set; }
        public static string DatabaseFilePath { get; private set; }
        public static string BackupRemoteFolderId { get; private set; }
        public static string BackupRemoteFileId { get; private set; }
        public static int AutoBackupInterval { get; private set; }

        private void App_OnStartup(object sender, StartupEventArgs e)
        {
            DatabasePath = $@"{Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData)}\{Assembly.GetExecutingAssembly().GetName().Name}";
            DatabaseFilePath = $@"{DatabasePath}\ba.sqlite";

            ReadConfiguration();
            CheckDatabaseConnection();
        }

        private void ReadConfiguration()
        {
            try
            {
                if (ConfigurationManager.ConnectionStrings.Count > 0)
                {
                    var sqliteConn = ConfigurationManager.ConnectionStrings["SqliteConnection"];
                    if (sqliteConn != null)
                    {
                        ConnectionString = ConfigurationManager.ConnectionStrings["SqliteConnection"].ConnectionString;
                        DatabaseFilePath = ExtractFilePathFromConnectionString(ConnectionString);
                        DatabasePath = ExtractPathFromFilePath(DatabaseFilePath);
                    }
                }
                BackupRemoteFolderId = ConfigurationManager.AppSettings["BackupRemoteFolderId"];
                BackupRemoteFileId = ConfigurationManager.AppSettings["BackupRemoteFileId"];
                int autoBackupValue;
                int.TryParse(ConfigurationManager.AppSettings["AutoBackupInterval"], out autoBackupValue);
                if (autoBackupValue < 0)
                {
                    autoBackupValue = 0;
                }
                AutoBackupInterval = autoBackupValue;
            }
            catch (ConfigurationErrorsException ex)
            {
                MessageBox.Show($"Конфигурация приложения содержит ошибки, что может привести к неправильной работе. Детали: {Environment.NewLine}{ex.Message}",
                    Resource.AppName, MessageBoxButton.OK, MessageBoxImage.Exclamation);
            }
        }

        private void CheckDatabaseConnection()
        {
            var connectionString = $@"Data Source={DatabaseFilePath};Version=3;UTF8Encoding=True;foreign keys=true;FailIfMissing=true;";

            if (Sqlite.TestConnection(connectionString))
            {
                Sqlite.ConnectionString = connectionString;
            }
            else
            {
                MessageBox.Show($"Не удалось установить соединение с базой данных.{Environment.NewLine}Детали: {Sqlite.LastErrorMessage}",
                    Resource.AppName, MessageBoxButton.OK, MessageBoxImage.Stop);
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
