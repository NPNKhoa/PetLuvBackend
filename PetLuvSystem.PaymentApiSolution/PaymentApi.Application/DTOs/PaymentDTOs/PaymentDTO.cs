using PaymentApi.Application.DTOs.PaymentMethodDTOs;
using PaymentApi.Application.DTOs.PaymentStatusDTOs;

namespace PaymentApi.Application.DTOs.PaymentDTOs
{
    public record PaymentDTO
    (
        Guid PaymentId,
        decimal Amount,
        string TransactionRef,
        DateTime CreatedAt,
        DateTime UpdatedTime,
        Guid OrderId,
        Guid UserId,
        Guid PaymentMethodId,
        PaymentMethodDTO PaymentMethod,
        Guid PaymentStatusId,
        PaymentStatusDTO PaymentStatus
    );
}
