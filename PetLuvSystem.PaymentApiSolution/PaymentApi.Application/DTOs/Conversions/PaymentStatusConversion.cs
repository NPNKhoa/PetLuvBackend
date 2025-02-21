using PaymentApi.Application.DTOs.PaymentStatusDTOs;
using PaymentApi.Domain.Entities;

namespace PaymentApi.Application.DTOs.Conversions
{
    public static class PaymentStatusConversion
    {
        public static PaymentStatus ToEntity(PaymentStatusDTO dto) => new()
        {
            PaymentStatusId = dto.PaymentStatusId,
            PaymentStatusName = dto.PaymentStatusName,
            IsVisible = dto.IsVisible
        };

        public static PaymentStatus ToEntity(CreateUpdatePaymentStatusDTO dto) => new()
        {
            PaymentStatusId = Guid.NewGuid(),
            PaymentStatusName = dto.PaymentStatusName,
            IsVisible = dto.IsVisible
        };

        public static (PaymentStatusDTO?, IEnumerable<PaymentStatusDTO>?) FromEntity(PaymentStatus? entity, IEnumerable<PaymentStatus>? entities)
        {
            if (entity is not null && entities is null)
            {
                return (new PaymentStatusDTO(entity.PaymentStatusId, entity.PaymentStatusName, entity.IsVisible), null);
            }

            if (entities is not null && entity is null)
            {
                return (null, entities.Select(e => new PaymentStatusDTO(e.PaymentStatusId, e.PaymentStatusName, e.IsVisible)).ToList());
            }

            return (null, null);
        }
    }
}
