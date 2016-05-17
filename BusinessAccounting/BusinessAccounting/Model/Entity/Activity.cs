using System;
using System.ComponentModel.DataAnnotations;

namespace BusinessAccounting.Model.Entity
{
    public class Activity : IEntity
    {
        public int Id { get; set; }

        [Required]
        public virtual ActivityType Type { get; set; }
        public DateTime DueDate { get; set; }

        [Required]
        public string Description { get; set; }
        public bool IsDone { get; set; }

        public Activity()
        {
            IsDone = false;
        }
    }
}
