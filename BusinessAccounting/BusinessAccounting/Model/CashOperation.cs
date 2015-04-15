using System;

namespace BusinessAccounting.Model
{
    class CashOperation
    {
        public virtual Int64 Id { get; set; }
        public virtual DateTime Date { get; set; }
        public virtual decimal Sum { get; set; }
        public virtual string Comment { get; set; }
    }
}
