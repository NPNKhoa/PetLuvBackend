namespace PaymentApi.Application.DTOs.PaymentDTOs
{
    public record UpdateStatusDTO
    (
        decimal Amount,
        bool IsComplete
    );
}
