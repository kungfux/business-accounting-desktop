using System;
using System.Collections.Generic;

namespace BusinessAccounting.Model
{
    interface ICashRepository
    {
        void Add(Cash pCash);
        void Update(Cash pCash);
        void Delete(Cash pCash);
        Cash GetById(long pId);
        ICollection<Cash> GetAll();
    }
}
