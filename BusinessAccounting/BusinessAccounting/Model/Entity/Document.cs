using System.ComponentModel.DataAnnotations;

namespace BusinessAccounting.Model.Entity
{
    public class Document : IEntity
    {
        public int Id { get; set; }

        [Required]
        public string Title { get; set; }
        public string Comment { get; set; }

        [Required]
        public byte[] Attachment { get; set; }
    }
}
