using BusinessAccounting.Model;
using BusinessAccounting.Repositories;
using NUnit.Framework;
using System;
using System.Collections.Generic;

namespace BusinessAccountingTests.RepositoryTests
{
    [TestFixture]
    public class EmployeeRepositoryTests
    {
        [Test]
        public void AddEmployee()
        {
            var employeeRepo = new EmployeeRepository();

            var employee = new Employee() { Name = "Name1", Hired = DateTime.Today  };
            employeeRepo.Add(employee);
            Assert.AreEqual(employee, employeeRepo.GetById(1));
        }

        [Test]
        public void AddSalary()
        {
            var employeeRepo = new EmployeeRepository();

            var employee = new Employee() { Name = "Name2", Hired = DateTime.Today };
            var cash = new Transaction() { Date = DateTime.Now, Sum = 1m, Comment = "Salary" };

            employee.Salary = new List<Transaction>() { cash };
            employeeRepo.Add(employee);
        }
    }
}
