using System.ComponentModel.DataAnnotations;

namespace ServiceApi.Domain.Entities
{
    public class ServiceCombo
    {
        [Key]
        public Guid ServiceComboId { get; set; }
        public string? ServiceComboName { get; set; }
        public string? ServiceComboDesc { get; set; }
        public bool IsVisible { get; set; }

        public virtual ICollection<ServiceComboMapping>? ServiceComboMappings { get; set; }
        public virtual ICollection<ServiceComboPrice>? ServiceComboPrices { get; set; }
    }
}
