namespace ServiceApi.Application.DTOs.ServiceImageDTOs
{
    public record ServiceImageDTO
    (
        string? ServiceImagePath,
        Guid ServiceId
    );
}
