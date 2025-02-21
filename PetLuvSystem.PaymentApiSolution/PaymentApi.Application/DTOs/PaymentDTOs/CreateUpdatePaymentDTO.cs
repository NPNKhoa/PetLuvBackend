namespace PaymentApi.Application.DTOs.PaymentDTOs
{
    public record CreateUpdatePaymentDTO
    (
        decimal Amount,
        string TransactionRef,
        string? ResponseData,
        Guid OrderId,
        Guid UserId,
        Guid PaymentMethodId,
        Guid PaymentStatusId
    );
}
