using MassTransit;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace BookingOrchestrator.Domain.Entities
{
    public class BookingState : SagaStateMachineInstance, ISagaVersion
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; } // MongoDB Id

        [BsonElement("CorrelationId")]
        public Guid CorrelationId { get; set; }

        [BsonElement("BookingId")]
        public Guid BookingId { get; set; }

        [BsonElement("CustomerId")]
        public Guid CustomerId { get; set; }
        [BsonElement("CustomerEmail ")]
        public string CustomerEmail { get; set; }

        [BsonElement("PetId")]
        public Guid PetId { get; set; }

        [BsonElement("TotalPrice")]
        public decimal TotalPrice { get; set; }

        [BsonElement("PaymentUrl")]
        public string PaymentUrl { get; set; }

        [BsonElement("Status")]
        public string Status { get; set; }

        [BsonElement("CreatedAt")]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        [BsonElement("EmailSentAt")]
        public DateTime? EmailSentAt { get; set; }

        [BsonElement("PaymentCompletedAt")]
        public DateTime? PaymentCompletedAt { get; set; }

        [BsonElement("CanceledAt")]
        public DateTime? CanceledAt { get; set; }
        public int Version { get; set; }
    }
}
