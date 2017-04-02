using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessAccounting.UserControls
{
    public class CashTransaction
    {
        public int Id {get; set;}
        public DateTime Date { get; set; }
        public decimal Sum { get; set; }
        public string Comment { get; set; }
    }
}
