using BusinessAccounting.Domain;
using NHibernate.Cfg;
using NHibernate.Tool.hbm2ddl;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace BusinessAccounting
{
    public partial class App
    {
        public App()
        {
            var configuration = new Configuration();
            configuration.Configure();
            //configuration.AddAssembly(typeof(EmployeeCard).Assembly);
            configuration.AddAssembly(typeof(CashOperation).Assembly);

            new SchemaExport(configuration).Execute(false, true, false);
        }
    }
}
