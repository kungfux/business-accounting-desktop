using System;
using System.ComponentModel.DataAnnotations;

namespace BusinessAccounting.Model.Entity
{
    public class Transaction : IEntity
    {
        public int Id { get; set; }

        [Required]
        public DateTime Date { get; set; }

        [Required]
        public decimal Value { get; set; }
        public string Comment { get; set; }
        public virtual Employee Employee { get; set; }
        public virtual Expenditure Expenditure { get; set; }
        public virtual Property Property { get; set; }
        public virtual Document Document { get; set; }

        public Transaction()
        {
            Date = DateTime.Now;
        }
    }
}
