using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BusinessAccounting.Model.Entity
{
    [Table("DOCUMENT")]
    public class Document : IEntity
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        public string Title { get; set; }
        public string Comment { get; set; }
        [Required]
        public byte[] BinaryData { get; set; }
    }
}
