using BusinessAccounting.Domain;
using NHibernate;

namespace BusinessAccounting.Repositories
{
    class CashOperationRepository : ICashOperationRepository
    {
        public void Add(CashOperation pCashOperation)
        {
            using (ISession session = NHibernateSession.OpenSession())
            using (ITransaction transaction = session.BeginTransaction())
            {
                session.Save(pCashOperation);
                transaction.Commit();
            }
        }

        public void Update(CashOperation pCashOperation)
        {
            using (ISession session = NHibernateSession.OpenSession())
            using (ITransaction transaction = session.BeginTransaction())
            {
                session.Update(pCashOperation);
                transaction.Commit();
            }
        }

        public void Remove(CashOperation pCashOperation)
        {
            using (ISession session = NHibernateSession.OpenSession())
            using (ITransaction transaction = session.BeginTransaction())
            {
                session.Delete(pCashOperation);
                transaction.Commit();
            }
        }

        public CashOperation GetById(long pCashOperationId)
        {
            using (ISession session = NHibernateSession.OpenSession())
                return session.Get<CashOperation>(pCashOperationId);
        }

        public System.Collections.Generic.ICollection<CashOperation> GetAll()
        {
            using (ISession session = NHibernateSession.OpenSession())
            {
                var cashOperations = session
                    .CreateCriteria(typeof(CashOperation))
                    .List<CashOperation>();
                return cashOperations;
            }
        }
    }
}
