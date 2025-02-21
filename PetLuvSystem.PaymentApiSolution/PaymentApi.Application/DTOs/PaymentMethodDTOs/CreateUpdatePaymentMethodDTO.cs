namespace PaymentApi.Application.DTOs.PaymentMethodDTOs
{
    public record CreateUpdatePaymentMethodDTO
    (
        string PaymentMethodName,
        bool IsActive
    );
}
