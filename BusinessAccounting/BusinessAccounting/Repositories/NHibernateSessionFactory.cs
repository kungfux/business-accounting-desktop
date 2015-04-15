using BusinessAccounting.Model;
using NHibernate;
using NHibernate.Cfg;
using NHibernate.Tool.hbm2ddl;
using System.IO;

namespace BusinessAccounting.Repositories
{
    class NHibernateSessionFactory
    {
        static Configuration configuration;

        static NHibernateSessionFactory()
        {
            configuration = new Configuration();
            configuration.Configure();
            configuration.AddAssembly(typeof(CashOperation).Assembly);

            if (!File.Exists("ba.sqlite"))
            {
                new SchemaExport(configuration).Execute(false, true, false);
            }
        }

        private static ISessionFactory _sessionFactory;
 
        private static ISessionFactory SessionFactory
        {
            get
            {
                if(_sessionFactory == null)
                {
                    //var configuration = new Configuration();
                    //configuration.Configure();
                    //configuration.AddAssembly(typeof(CashOperation).Assembly);
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
