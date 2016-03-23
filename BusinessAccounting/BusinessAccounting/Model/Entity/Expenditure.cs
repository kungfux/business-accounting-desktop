using System.ComponentModel.DataAnnotations;

namespace BusinessAccounting.Model.Entity
{
    public class Expenditure : IEntity
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }
        public bool IsActive { get; set; }

        public Expenditure()
        {
            IsActive = true;
        }
    }
}
