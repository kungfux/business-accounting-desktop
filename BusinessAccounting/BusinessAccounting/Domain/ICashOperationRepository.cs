using System;
using System.Collections.Generic;

namespace BusinessAccounting.Domain
{
    interface ICashOperationRepository
    {
        void Add(CashOperation pCashOperation);
        void Update(CashOperation pCashOperation);
        void Delete(CashOperation pCashOperation);
        ICollection<CashOperation> GetAll();
        ICollection<CashOperation> GetLast50();
    }
}
