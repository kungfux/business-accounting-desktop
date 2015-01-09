using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessAccounting.UserControls
{
    public class Employee
    {
        public int id { get; set; }
        public DateTime hired { get; set; }
        public DateTime fired { get; set; }
        public string fullname { get; set; }
        public string document { get; set; }
        public string telephone { get; set; }
        public string address { get; set; }
        public string notes { get; set; }
    }
}
