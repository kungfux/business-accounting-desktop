using BusinessAccounting.Model;
using BusinessAccounting.Model.Entity;
using System;
using System.Windows;

namespace BusinessAccounting
{
    public partial class App : Application
    {
        private void Application_Startup(object sender, StartupEventArgs e)
        {
            InitDatabase();
        }

        private void InitDatabase()
        {
            using (var context = new DatabaseContext())
            {
                var company = new Company() { Id = Guid.NewGuid(), Name = "Company Name" };
                var title = new Title() { Id = Guid.NewGuid(), Name = "Title1", Rate = 12, Company = company };
                context.Set<Company>().Add(company);
                context.Set<Title>().Add(title);
                context.SaveChanges();
            }
        }
    }
}
