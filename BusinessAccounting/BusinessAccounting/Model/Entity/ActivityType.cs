using System.Drawing;
using System.ComponentModel.DataAnnotations;

namespace BusinessAccounting.Model.Entity
{
    public class ActivityType : IEntity
    {
        public int Id { get; set; }

        [Required]
        public string Title { get; set; }

        [Required]
        public string Color { get; set; }
        public bool IsActive { get; set; }

        public ActivityType()
        {
            Color = System.Drawing.Color.Black.Name;
            IsActive = true;
        }
    }
}