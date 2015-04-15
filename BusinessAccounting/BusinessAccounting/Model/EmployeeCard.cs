using System;
using System.Drawing;

namespace BusinessAccounting.Model
{
    class EmployeeCard
    {
        public virtual Int64 Id { get; set; }
        public virtual DateTime HiredDate { get; set; }
        public virtual DateTime FiredDate { get; set; }
        public virtual string FullName { get; set; }
        public virtual Image Photo { get; set; }
        public virtual string DocumentInfo { get; set; }
        public virtual string PhoneNumber { get; set; }
        public virtual string Address { get; set; }
        public virtual string Notes { get; set; }
    }
}
