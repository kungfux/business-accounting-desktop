using BusinessAccounting.Model;
using NHibernate;
using NHibernate.Criterion;
using System.Collections.Generic;

namespace BusinessAccounting.Repositories
{
    class EmployeeCardRepository : IEmployeeCardRepository
    {
        public void Add(EmployeeCard pEmployeeCard)
        {
            using (ISession session = NHibernateSessionFactory.OpenSession())
            using (ITransaction transaction = session.BeginTransaction())
            {
                session.Save(pEmployeeCard);
                transaction.Commit();
            }
        }

        public void Update(EmployeeCard pEmployeeCard)
        {
            using (ISession session = NHibernateSessionFactory.OpenSession())
            using (ITransaction transaction = session.BeginTransaction())
            {
                session.Update(pEmployeeCard);
                transaction.Commit();
            }
        }

        public void Delete(EmployeeCard pEmployeeCard)
        {
            using (ISession session = NHibernateSessionFactory.OpenSession())
            using (ITransaction transaction = session.BeginTransaction())
            {
                session.Delete(pEmployeeCard);
                transaction.Commit();
            }
        }

        public ICollection<EmployeeCard> GetAll()
        {
            using (ISession session = NHibernateSessionFactory.OpenSession())
            {
                var employeeCard = session
                    .CreateCriteria(typeof(EmployeeCard))
                    .List<EmployeeCard>();
                return employeeCard;
            }
        }

        public ICollection<CashOperation> GetSalaryHistory(EmployeeCard pEmployeeCard)
        {
            using (ISession session = NHibernateSessionFactory.OpenSession())
            {
                var cashOperation = session
                    .CreateCriteria(typeof(CashOperation))
                    .Add(Restrictions.Eq("Id", pEmployeeCard.Id))
                    .List<CashOperation>();
                return cashOperation;
            }
        }
    }
}
