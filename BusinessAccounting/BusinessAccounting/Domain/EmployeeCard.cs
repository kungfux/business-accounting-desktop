using System;
using System.Drawing;

namespace BusinessAccounting.Domain
{
    class EmployeeCard
    {
        public Int64 Id { get; set; }
        public DateTime HiredDate { get; set; }
        public DateTime FiredDate { get; set; }
        public string FullName { get; set; }
        public Image Photo { get; set; }
        public string DocumentInfo { get; set; }
        public string PhoneNumber { get; set; }
        public string Address { get; set; }
        public string Notes { get; set; }
    }
}
