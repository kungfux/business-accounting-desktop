using System;
using System.ComponentModel.DataAnnotations;

namespace BusinessAccounting.Model.Entity
{
    public class Employee : IEntity
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }
        public string PhoneNumber { get; set; }
        public byte[] Photo { get; set; }

        [Required]
        public DateTime Hired { get; set; }
        public DateTime Fired { get; set; }
        public DateTime Birthday { get; set; }
        public string Document { get; set; }
        public string Address { get; set; }
        public string Notes { get; set; }
        public virtual EmployeePosition Position { get; set; }

        public Employee()
        {
            Hired = DateTime.Now;
        }
    }
}
