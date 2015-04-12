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
            var cfg = new Configuration();
            cfg.Configure();
            cfg.AddAssembly(typeof(CashOperation).Assembly);

            new SchemaExport(cfg).Execute(false, true, false);
        }
    }
}
