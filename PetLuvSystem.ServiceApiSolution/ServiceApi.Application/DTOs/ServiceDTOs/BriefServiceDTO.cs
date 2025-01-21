namespace ServiceApi.Application.DTOs.ServiceDTOs
{
    public record BriefServiceDTO(
        Guid ServiceId,
        string? ServiceName,
        string? ServiceDesc,
        bool IsVisible
    );
}
