using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PetApi.Domain.Entities
{
    public class PetHealthBook
    {
        [Key, ForeignKey("Pet")]
        public Guid PetHealthBookId { get; set; }
        public Pet Pet { get; set; }
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;

        public virtual ICollection<PetHealthBookDetail> PetHealthBookDetails { get; set; }
    }
}
