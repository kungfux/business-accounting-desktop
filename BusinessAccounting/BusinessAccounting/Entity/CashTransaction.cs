using System;

namespace BusinessAccounting.Entity
{
    public class CashTransaction
    {
        public int Id {get; set;}
        public DateTime Date { get; set; }
        public decimal Sum { get; set; }
        public string Comment { get; set; }
        public string EmployeeFullName { get; set; }
    }
}
