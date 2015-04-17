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

        public virtual DateTime HiredDate { get; set; }
        public virtual DateTime FiredDate { get; set; }
        public virtual string FullName { get; set; }
        public virtual Image Photo { get; set; }
        public virtual string DocumentInfo { get; set; }
        public virtual string PhoneNumber { get; set; }
        public virtual string Address { get; set; }
        public virtual string Notes { get; set; }
        public virtual ICollection<Cash> Salary { get; set; }

        public override bool Equals(object other)
        {
            Employee employee = other as Employee;
            return this.Id == employee.Id ? true : false;
        }
    }
}
