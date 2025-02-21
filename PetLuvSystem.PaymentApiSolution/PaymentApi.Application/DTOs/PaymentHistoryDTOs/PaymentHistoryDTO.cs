using PaymentApi.Application.DTOs.PaymentDTOs;
using PaymentApi.Application.DTOs.PaymentStatusDTOs;

namespace PaymentApi.Application.DTOs.PaymentHistoryDTOs
{
    public record PaymentHistoryDTO
    (
        Guid PaymentHistoryId,
        DateTime UpdatedAt,
        Guid PaymentId,
        PaymentDTO Payment,
        Guid PaymentStatusId,
        PaymentStatusDTO PaymentStatus
    );
}
