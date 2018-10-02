using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BusinessAccounting.Model.Entity
{
    [Table("TITLE")]
    public class Title : IEntity
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        public string Name { get; set; }
        public decimal Rate { get; set; }

        [Required]
        public virtual Company Company { get; set; }
    }
}
