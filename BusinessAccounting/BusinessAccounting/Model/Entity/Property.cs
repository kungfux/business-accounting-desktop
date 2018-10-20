using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BusinessAccounting.Model.Entity
{
    [Table("PROPERTY")]
    public class Property : IEntity
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        public string Title { get; set; }
        public string InventoryNumber { get; set; }
        public string Comment { get; set; }

        [Required]
        public virtual Company Company { get; set; }
    }
}
