using BusinessAccounting.Model;
using NHibernate;
using NHibernate.Criterion;
using System.Collections.Generic;

namespace BusinessAccounting.Repositories
{
    public class CashRepository : ICashRepository
    {
        public void Add(Cash pCash)
        {
            using (ISession session = NHibernateSessionFactory.OpenSession())
            using (ITransaction transaction = session.BeginTransaction())
            {
                session.Save(pCash);
                transaction.Commit();
            }
        }

        public void Update(Cash pCash)
        {
            using (ISession session = NHibernateSessionFactory.OpenSession())
            using (ITransaction transaction = session.BeginTransaction())
            {
                session.Update(pCash);
                transaction.Commit();
            }
        }

        public void Delete(Cash pCash)
        {
            using (ISession session = NHibernateSessionFactory.OpenSession())
            using (ITransaction transaction = session.BeginTransaction())
            {
                session.Delete(pCash);
                transaction.Commit();
            }
        }

        public Cash GetById(long pId)
        {
            using (ISession session = NHibernateSessionFactory.OpenSession())
            {
                var result = session
                    .CreateCriteria(typeof(Cash))
                    .Add(Restrictions.Eq("Id", pId))
                    .UniqueResult<Cash>();
                return result;
            }
        }

        public ICollection<Cash> GetAll()
        {
            using (ISession session = NHibernateSessionFactory.OpenSession())
            {
                var result = session
                    .CreateCriteria(typeof(Cash))
                    .List<Cash>();
                return result;
            }
        }
    }
}
