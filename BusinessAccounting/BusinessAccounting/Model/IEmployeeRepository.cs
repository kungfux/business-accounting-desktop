using System;
using System.Collections.Generic;

namespace BusinessAccounting.Model
{
    interface IEmployeeRepository
    {
        void Add(Employee pEmployee);
        void Update(Employee pEmployee);
        void Delete(Employee pEmployee);
        Employee GetById(long Id);
        ICollection<Employee> GetAll();
        void AddSalary(Employee pEmployee, Transaction pCash);
        ICollection<Transaction> GetSalary(Employee pEmployee);
    }
}
