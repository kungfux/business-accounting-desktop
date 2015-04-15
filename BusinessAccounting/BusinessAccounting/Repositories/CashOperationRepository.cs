using BusinessAccounting.Model;
using NHibernate;
using System.Collections.Generic;

namespace BusinessAccounting.Repositories
{
    class CashOperationRepository : ICashOperationRepository
    {
        public void Add(CashOperation pCashOperation)
        {
            using (ISession session = NHibernateSessionFactory.OpenSession())
            using (ITransaction transaction = session.BeginTransaction())
            {
                session.Save(pCashOperation);
                transaction.Commit();
            }
        }

        public void Update(CashOperation pCashOperation)
        {
            using (ISession session = NHibernateSessionFactory.OpenSession())
            using (ITransaction transaction = session.BeginTransaction())
            {
                session.Update(pCashOperation);
                transaction.Commit();
            }
        }

        public void Delete(CashOperation pCashOperation)
        {
            using (ISession session = NHibernateSessionFactory.OpenSession())
            using (ITransaction transaction = session.BeginTransaction())
            {
                session.Delete(pCashOperation);
                transaction.Commit();
            }
        }

        public ICollection<CashOperation> GetAll()
        {
            using (ISession session = NHibernateSessionFactory.OpenSession())
            {
                var cashOperations = session
                    .CreateCriteria(typeof(CashOperation))
                    .List<CashOperation>();
                return cashOperations;
            }
        }

        public ICollection<CashOperation> GetLast50()
        {
            using (ISession session = NHibernateSessionFactory.OpenSession())
            {
                var cashOperations = session
                    .CreateCriteria(typeof(CashOperation))
                    .SetMaxResults(50)
                    .List<CashOperation>();
                return cashOperations;
            }
        }
    }
}
