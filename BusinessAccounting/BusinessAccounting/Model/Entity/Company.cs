﻿using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BusinessAccounting.Model.Entity
{
    [Table("COMPANY")]
    public class Company : IEntity
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        public string Name { get; set; }
        public virtual Picture Logo { get; set; }
    }
}