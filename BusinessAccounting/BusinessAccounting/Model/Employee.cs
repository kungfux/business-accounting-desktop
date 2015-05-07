using System;
using System.Collections.Generic;
using System.Drawing;

namespace BusinessAccounting.Model
{
    public class Employee
    {
        long id; // identifier

        public virtual long Id
        {
            get { return id; }
            protected set { id = value; }
        }

        public virtual DateTime Hired { get; set; }
        public virtual DateTime Fired { get; set; }
        public virtual string Name { get; set; }
        public virtual Image Photo { get; set; }
        public virtual string Passport { get; set; }
        public virtual string Phone { get; set; }
        public virtual string Address { get; set; }
        public virtual string Notes { get; set; }
        public virtual ICollection<Transaction> Salary { get; set; }

        public Transaction Transaction
        {
            get
            {
                throw new System.NotImplementedException();
            }
            set
            {
            }
        }

        public override bool Equals(object other)
        {
            Employee employee = other as Employee;
            return this.Id == employee.Id ? true : false;
        }
    }
}
