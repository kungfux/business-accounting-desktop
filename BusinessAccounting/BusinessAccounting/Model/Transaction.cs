using System;

namespace BusinessAccounting.Model
{
    public class Transaction
    {
        long id; // identifier

        public virtual long Id
        {
            get { return id; }
            protected set { id = value; }
        }

        public virtual DateTime Date { get; set; }
        public virtual decimal Sum { get; set; }
        public virtual string Comment { get; set; }

        public override bool Equals(object other)
        {
            Transaction cash = other as Transaction;
            return this.Id == cash.Id ? true : false;
        }
    }
}
