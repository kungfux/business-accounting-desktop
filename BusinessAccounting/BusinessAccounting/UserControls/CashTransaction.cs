using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessAccounting.UserControls
{
    public class CashTransaction
    {
        public int id {get; set;}
        public DateTime date { get; set; }
        public decimal sum { get; set; }
        public string comment { get; set; }
    }
}
