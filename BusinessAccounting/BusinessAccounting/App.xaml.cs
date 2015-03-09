using System.Windows;
using Xclass.Database;

namespace BusinessAccounting
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public static SQLite3Query sqlite = new SQLite3Query();
    }
}
