using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace ServiceApi.Domain.Entities
{
    public class ServiceType
    {
        [Key]
        public Guid ServiceTypeId { get; set; }
        public string? ServiceTypeName { get; set; }
        public string? ServiceTypeDesc { get; set; }
        public bool IsVisible { get; set; }

        [JsonIgnore]
        public virtual ICollection<Service>? Services { get; set; }
    }
}
