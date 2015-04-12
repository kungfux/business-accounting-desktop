using System;
using System.Collections.Generic;

namespace BusinessAccounting.Domain
{
    interface ICashOperationRepository
    {
        void Add(CashOperation pCashOperation);
        void Update(CashOperation pCashOperation);
        void Remove(CashOperation pCashOperation);
        CashOperation GetById(Int64 pCashOperationId);
        ICollection<CashOperation> GetAll();
    }
}
