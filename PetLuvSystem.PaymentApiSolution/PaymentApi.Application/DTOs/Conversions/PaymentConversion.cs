using PaymentApi.Application.DTOs.PaymentDTOs;
using PaymentApi.Domain.Entities;

namespace PaymentApi.Application.DTOs.Conversions
{
    public static class PaymentConversion
    {
        public static Payment ToEntity(PaymentDTO dto) => new()
        {
            PaymentId = dto.PaymentId,
            Amount = dto.Amount,
            TransactionRef = dto.TransactionRef,
            CreatedAt = dto.CreatedAt,
            UpdatedTime = dto.UpdatedTime,
            OrderId = dto.OrderId,
            UserId = dto.UserId,
            PaymentMethodId = dto.PaymentMethodId,
            PaymentStatusId = dto.PaymentStatusId,
        };

        public static Payment ToEntity(CreateUpdatePaymentDTO dto) => new()
        {
            PaymentId = Guid.NewGuid(),
            Amount = dto.Amount,
            TransactionRef = dto.TransactionRef,
            OrderId = dto.OrderId,
            UserId = dto.UserId,
            PaymentMethodId = dto.PaymentMethodId,
            PaymentStatusId = dto.PaymentStatusId,
        };

        public static (PaymentDTO?, IEnumerable<PaymentDTO>?) FromEntity(Payment? entity, IEnumerable<Payment>? entities)
        {
            if (entity is not null && entities is null)
            {
                return (new PaymentDTO(
                    entity.PaymentId,
                    entity.Amount,
                    entity.TransactionRef,
                    entity.CreatedAt,
                    entity.UpdatedTime,
                    entity.OrderId,
                    entity.UserId,
                    entity.PaymentMethodId,
                    new PaymentMethodDTOs.PaymentMethodDTO(
                        entity.PaymentMethod.PaymentMethodId,
                        entity.PaymentMethod.PaymentMethodName,
                        entity.PaymentMethod.IsVisible
                    ),
                    entity.PaymentStatusId,
                    new PaymentStatusDTOs.PaymentStatusDTO(
                        entity.PaymentStatus.PaymentStatusId,
                        entity.PaymentStatus.PaymentStatusName,
                        entity.PaymentStatus.IsVisible
                    )
                ), null);
            }

            if (entities is not null && entity is null)
            {
                return (null, entities.Select(e => new PaymentDTO(
                    e.PaymentId,
                    e.Amount,
                    e.TransactionRef,
                    e.CreatedAt,
                    e.UpdatedTime,
                    e.OrderId,
                    e.UserId,
                    e.PaymentMethodId,
                    new PaymentMethodDTOs.PaymentMethodDTO(
                        e.PaymentMethod.PaymentMethodId,
                        e.PaymentMethod.PaymentMethodName,
                        e.PaymentMethod.IsVisible
                    ),
                    e.PaymentStatusId,
                    new PaymentStatusDTOs.PaymentStatusDTO(
                        e.PaymentStatus.PaymentStatusId,
                        e.PaymentStatus.PaymentStatusName,
                        e.PaymentStatus.IsVisible
                    )
                )).ToList());
            }

            return (null, null);
        }
    }
}
