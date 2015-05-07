using System;
using System.Collections.Generic;

namespace BusinessAccounting.Model
{
    interface ITransactionRepository
    {
        void Add(Transaction pCash);
        void Update(Transaction pCash);
        void Delete(Transaction pCash);
        Transaction GetById(long pId);
        ICollection<Transaction> GetAll();
    }
}
