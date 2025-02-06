namespace UserApi.Application.DTOs.UserDTOs
{
    public record BriefUserDTO
    (
        Guid UserId,
        string FullName,
        string Email,
        string PhoneNumber,
        string? Avatar,
        string StaffType,
        bool IsActive
    );
}
