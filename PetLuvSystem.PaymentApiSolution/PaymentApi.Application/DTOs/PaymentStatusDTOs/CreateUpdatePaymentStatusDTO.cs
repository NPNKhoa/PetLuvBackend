namespace PaymentApi.Application.DTOs.PaymentStatusDTOs
{
    public record CreateUpdatePaymentStatusDTO
    (
        string PaymentStatusName,
        bool IsVisible
    );
}
