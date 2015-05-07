using BusinessAccounting.Model;
using NHibernate;
using NHibernate.Criterion;
using System.Collections.Generic;

namespace BusinessAccounting.Repositories
{
    public class TransactionRepository : ITransactionRepository
    {
        public void Add(Transaction pCash)
        {
            using (ISession session = NHibernateSessionFactory.OpenSession())
            using (ITransaction transaction = session.BeginTransaction())
            {
                session.Save(pCash);
                transaction.Commit();
            }
        }

        public void Update(Transaction pCash)
        {
            using (ISession session = NHibernateSessionFactory.OpenSession())
            using (ITransaction transaction = session.BeginTransaction())
            {
                session.Update(pCash);
                transaction.Commit();
            }
        }

        public void Delete(Transaction pCash)
        {
            using (ISession session = NHibernateSessionFactory.OpenSession())
            using (ITransaction transaction = session.BeginTransaction())
            {
                session.Delete(pCash);
                transaction.Commit();
            }
        }

        public Transaction GetById(long pId)
        {
            using (ISession session = NHibernateSessionFactory.OpenSession())
            {
                var result = session
                    .CreateCriteria(typeof(Transaction))
                    .Add(Restrictions.Eq("Id", pId))
                    .UniqueResult<Transaction>();
                return result;
            }
        }

        public ICollection<Transaction> GetAll()
        {
            using (ISession session = NHibernateSessionFactory.OpenSession())
            {
                var result = session
                    .CreateCriteria(typeof(Transaction))
                    .List<Transaction>();
                return result;
            }
        }
    }
}
