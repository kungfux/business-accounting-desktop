using System.Windows;
using XDatabase;

namespace BusinessAccounting
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public static readonly XQuerySqlite sqlite = new XQuerySqlite();
    }
}
