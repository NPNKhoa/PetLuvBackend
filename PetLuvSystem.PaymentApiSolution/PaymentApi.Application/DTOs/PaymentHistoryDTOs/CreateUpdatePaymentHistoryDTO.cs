namespace PaymentApi.Application.DTOs.PaymentHistoryDTOs
{
    public record CreateUpdatePaymentHistoryDTO
    (
        DateTime UpdatedAt,
        Guid PaymentId,
        Guid PaymentStatusId
    );
}
