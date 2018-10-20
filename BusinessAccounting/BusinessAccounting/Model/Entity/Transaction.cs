using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BusinessAccounting.Model.Entity
{
    [Table("TRANSACTION")]
    public class Transaction : IEntity
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        public DateTime PostDate { get; set; }
        [Required]
        public decimal Value { get; set; }
        public string Comment { get; set; }

        [Required]
        public virtual Company Company { get; set; }
        public virtual Document Document { get; set; }
        public virtual Contact Contact { get; set; }
        public virtual Property Property { get; set; }
        public virtual Expenditure Expenditure { get; set; }
    }
}
