using System.ComponentModel.DataAnnotations;

namespace ServiceApi.Domain.Entities
{
    public class ServiceImage
    {
        [Key]
        public string? ServiceImagePath { get; set; }

        public Guid ServiceId { get; set; }
        public Service? Service { get; set; }
    }
}
