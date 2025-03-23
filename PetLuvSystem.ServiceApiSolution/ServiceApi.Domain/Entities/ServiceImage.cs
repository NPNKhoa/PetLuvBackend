using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace ServiceApi.Domain.Entities
{
    public class ServiceImage
    {
        [Key]
        public string? ServiceImagePath { get; set; }

        public Guid ServiceId { get; set; }
        [JsonIgnore]
        public Service? Service { get; set; }
    }
}
