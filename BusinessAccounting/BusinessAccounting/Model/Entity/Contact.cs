using System;
using System.ComponentModel.DataAnnotations;

namespace BusinessAccounting.Model.Entity
{
    public class Contact : IEntity
    {
        // Common
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }
        public byte[] Photo { get; set; }
        public string Company { get; set; }
        public virtual JobTitle JobTitle { get; set; }
        public string Phone { get; set; }
        public string CellPhone { get; set; }
        public string Email { get; set; }
        public DateTime Birthday { get; set; }
        public string Address { get; set; }
        public string Notes { get; set; }

        // Employee only
        public DateTime Hired { get; set; }
        public DateTime Fired { get; set; }
        public string Document { get; set; }
    }
}
