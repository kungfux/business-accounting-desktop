using System;
using System.Collections.Generic;

namespace BusinessAccounting.Domain
{
    interface IEmployeeCardRepository
    {
        void Add(EmployeeCard pEmployeeCard);
        void Update(EmployeeCard pEmployeeCard);
        void Delete(EmployeeCard pEmployeeCard);
        ICollection<EmployeeCard> GetAll();
        ICollection<CashOperation> GetSalaryHistory(EmployeeCard pEmployeeCard);
    }
}
