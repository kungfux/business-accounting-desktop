using BusinessAccounting.Model;
using BusinessAccounting.Model.Entity;
using System;
using System.Windows;

namespace BusinessAccounting
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private void Application_Startup(object sender, StartupEventArgs e)
        {
            InitDatabase();
        }

        private void InitDatabase()
        {
            var context = new DatabaseContext();
            context.Set<Company>().Add(new Company() { Id = Guid.NewGuid(), Name = "Company Name" });
            context.SaveChanges();
        }
    }
}
