namespace PaymentApi.Application.DTOs.PaymentMethodDTOs
{
    public record PaymentMethodDTO
    (
        Guid PaymentMethodId,
        string PaymentMethodName,
        bool IsActive
    );
}
