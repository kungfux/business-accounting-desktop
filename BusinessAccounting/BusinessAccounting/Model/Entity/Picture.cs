using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BusinessAccounting.Model.Entity
{
    [Table("PICTURES")]
    public class Picture : IEntity
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        public byte[] Source { get; set; }
    }
}
