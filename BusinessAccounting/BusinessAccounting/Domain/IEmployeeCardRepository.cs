using System;
using System.Collections.Generic;

namespace BusinessAccounting.Domain
{
    interface IEmployeeCardRepository
    {
        void Add(EmployeeCard pEmployeeCard);
        void Update(EmployeeCard pEmployeeCard);
        void Remove(EmployeeCard pEmployeeCard);
        EmployeeCard GetById(Int64 pEmployeeCardId);
        ICollection<EmployeeCard> GetAll();
        ICollection<CashOperation> GetSalaryHistory(EmployeeCard pEmployeeCard);
    }
}
