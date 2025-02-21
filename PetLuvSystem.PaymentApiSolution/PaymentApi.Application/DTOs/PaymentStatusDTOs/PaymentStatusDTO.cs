namespace PaymentApi.Application.DTOs.PaymentStatusDTOs
{
    public record PaymentStatusDTO
    (
        Guid PaymentStatusId,
        string PaymentStatusName,
        bool IsVisible
    );
}
