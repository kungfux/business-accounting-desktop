using System.ComponentModel.DataAnnotations;

namespace BusinessAccounting.Model.Entity
{
    public class Property : IEntity
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }
        public string Code { get; set; }
        public decimal Cost { get; set; }
        public string Comment { get; set; }
        public bool IsActive { get; set; }

        public Property()
        {
            IsActive = true;
        }
    }
}
