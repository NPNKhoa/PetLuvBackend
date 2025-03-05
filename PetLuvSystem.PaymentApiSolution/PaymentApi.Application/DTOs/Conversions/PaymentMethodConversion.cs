using PaymentApi.Application.DTOs.PaymentMethodDTOs;
using PaymentApi.Domain.Entities;

namespace PaymentApi.Application.DTOs.Conversions
{
    public static class PaymentMethodConversion
    {
        public static PaymentMethod ToEntity(PaymentMethodDTO dto) => new()
        {
            PaymentMethodId = dto.PaymentMethodId,
            PaymentMethodName = dto.PaymentMethodName,
            IsVisible = dto.IsActive
        };

        public static PaymentMethod ToEntity(CreateUpdatePaymentMethodDTO dto) => new()
        {
            PaymentMethodId = Guid.NewGuid(),
            PaymentMethodName = dto.PaymentMethodName,
            IsVisible = dto.IsActive
        };

        public static (PaymentMethodDTO?, IEnumerable<PaymentMethodDTO>?) FromEntity(PaymentMethod? entity, IEnumerable<PaymentMethod>? entities)
        {
            if (entity is not null && entities is null)
            {
                return (
                    new PaymentMethodDTO(

                        entity.PaymentMethodId,
                        entity.PaymentMethodName,
                        entity.IsVisible
                    )
                , null);
            }

            if (entities is not null && entity is null)
            {
                return (null, entities.Select(p =>
                    new PaymentMethodDTO(
                        p.PaymentMethodId,
                        p.PaymentMethodName,
                        p.IsVisible
                    )).ToList()
                );
            }

            return (null, null);
        }
    }
}
