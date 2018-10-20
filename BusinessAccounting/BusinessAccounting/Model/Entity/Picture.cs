using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BusinessAccounting.Model.Entity
{
    [Table("PICTURE")]
    public class Picture : IEntity
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        public byte[] BinaryData { get; set; }
    }
}
