using BusinessAccounting.Model;
using BusinessAccounting.Repositories;
using NUnit.Framework;
using System;
using System.IO;

namespace BusinessAccountingTests.RepositoryTests
{
    [TestFixture]
    public class EmployeeRepositoryTests
    {
        [Test]
        public void AddEmployee()
        {
            var employeeRepo = new EmployeeRepository();

            var employee = new Employee() { FullName = "Name1", HiredDate = DateTime.Today  };
            employeeRepo.Add(employee);
            Assert.AreEqual(employee, employeeRepo.GetById(1));
        }

        [Test]
        public void AddSalary()
        {
            var employeeRepo = new EmployeeRepository();

            var employee = new Employee() { FullName = "Name2", HiredDate = DateTime.Today };
            employeeRepo.Add(employee);

            var cash = new Cash() { Date = DateTime.Now, Sum = 1m, Comment = "Salary" };

            var employee2 = employeeRepo.GetById(1);
            employeeRepo.AddSalary(employee2, cash);
        }
    }
}
