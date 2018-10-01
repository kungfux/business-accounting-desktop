using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace BusinessAccounting.Model.Entity
{
    [Table("COMPANY")]
    public class Company : IEntity
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
    }
}
