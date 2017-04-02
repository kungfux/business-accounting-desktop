using System.Windows;
using XDatabase;

namespace BusinessAccounting
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App
    {
        public static readonly XQuerySqlite Sqlite = new XQuerySqlite();
    }
}
