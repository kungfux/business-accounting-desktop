using BusinessAccounting.Model;
using NHibernate;
using NHibernate.Criterion;
using System.Collections.Generic;

namespace BusinessAccounting.Repositories
{
    public class EmployeeRepository : IEmployeeRepository
    {
        public void Add(Employee pEmployee)
        {
            using (ISession session = NHibernateSessionFactory.OpenSession())
            using (ITransaction transaction = session.BeginTransaction())
            {
                session.Save(pEmployee);
                transaction.Commit();
            }
        }

        public void Update(Employee pEmployee)
        {
            using (ISession session = NHibernateSessionFactory.OpenSession())
            using (ITransaction transaction = session.BeginTransaction())
            {
                session.Update(pEmployee);
                transaction.Commit();
            }
        }

        public void Delete(Employee pEmployee)
        {
            using (ISession session = NHibernateSessionFactory.OpenSession())
            using (ITransaction transaction = session.BeginTransaction())
            {
                session.Delete(pEmployee);
                transaction.Commit();
            }
        }

        public Employee GetById(long pId)
        {
            using (ISession session = NHibernateSessionFactory.OpenSession())
            {
                var employeeCard = session
                    .CreateCriteria(typeof(Employee))
                    .Add(Restrictions.Eq("Id", pId))
                    .UniqueResult<Employee>();
                return employeeCard;
            }
        }

        public ICollection<Employee> GetAll()
        {
            using (ISession session = NHibernateSessionFactory.OpenSession())
            {
                var employeeCard = session
                    .CreateCriteria(typeof(Employee))
                    .List<Employee>();
                return employeeCard;
            }
        }

        public void AddSalary(Employee pEmployee, Cash pCash)
        {
            using (ISession session = NHibernateSessionFactory.OpenSession())
            using (ITransaction transaction = session.BeginTransaction())
            {
                session.Save(pCash, pEmployee.Id);
                transaction.Commit();
            }
        }

        public ICollection<Cash> GetSalary(Employee pEmployee)
        {
            using (ISession session = NHibernateSessionFactory.OpenSession())
            {
                var cashOperation = session
                    .CreateCriteria(typeof(Cash))
                    .Add(Restrictions.Eq("Id", pEmployee.Id))
                    .List<Cash>();
                return cashOperation;
            }
        }
    }
}
