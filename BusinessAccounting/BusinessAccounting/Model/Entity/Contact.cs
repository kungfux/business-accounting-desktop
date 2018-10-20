using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BusinessAccounting.Model.Entity
{
    [Table("CONTACT")]
    public class Contact : IEntity
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        public string Name { get; set; }
        public string Phone { get; set; }
        public string CellPhone { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
        public string Note { get; set; }
        public DateTime Birthday { get; set; }
        public DateTime Hired { get; set; }
        public DateTime Fired { get; set; }

        [Required]
        public virtual Company Company { get; set; }
        public virtual Title Title { get; set; }
        public virtual Picture Photo { get; set; }
        public virtual IList<Document> Documents { get; set; }
    }
}
