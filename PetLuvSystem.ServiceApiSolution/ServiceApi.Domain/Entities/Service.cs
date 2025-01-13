using System.ComponentModel.DataAnnotations;

namespace ServiceApi.Domain.Entities
{
    public class Service
    {
        [Key]
        public Guid ServiceId { get; set; }
        public string? ServiceName { get; set; }
        public string? ServiceDesc { get; set; }
        public bool IsVisible { get; set; }

        public Guid ServiceTypeId { get; set; }
        public ServiceType? ServiceType { get; set; }

        public ICollection<ServiceImage>? ServiceImages { get; set; }
        public virtual ICollection<ServiceComboMapping>? ServiceComboMappings { get; set; }
        public virtual ICollection<ServiceVariant>? ServiceVariants { get; set; }
        public virtual ICollection<WalkDogServiceVariant>? WalkDogServiceVariants { get; set; }
    }
}
