using System;

namespace BusinessAccounting.Domain
{
    class CashOperation
    {
        public Int64 Id { get; set; }
        public DateTime Date { get; set; }
        public decimal Sum { get; set; }
        public string Comment { get; set; }
    }
}
