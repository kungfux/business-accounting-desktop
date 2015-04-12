using BusinessAccounting.Domain;
using NHibernate;
using NHibernate.Cfg;
using NHibernate.Tool.hbm2ddl;

namespace BusinessAccounting.Repositories
{
    class NHibernateSession
    {
        static NHibernateSession()
        {
            var configuration = new Configuration();
            configuration.Configure();
            //configuration.AddAssembly(typeof(EmployeeCard).Assembly);
            configuration.AddAssembly(typeof(CashOperation).Assembly);

            new SchemaExport(configuration).Execute(false, true, false);
        }

        private static ISessionFactory _sessionFactory;
 
        private static ISessionFactory SessionFactory
        {
            get
            {
                if(_sessionFactory == null)
                {
                    var configuration = new Configuration();
                    configuration.Configure();
                    configuration.AddAssembly(typeof(CashOperation).Assembly);
                    _sessionFactory = configuration.BuildSessionFactory();
                }
                return _sessionFactory;
            }
        }
 
        public static ISession OpenSession()
        {
            return SessionFactory.OpenSession();
        }
    }
}
