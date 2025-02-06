using Microsoft.AspNetCore.Http;

namespace UserApi.Application.DTOs.UserDTOs
{
    public record UpdateUserDTO
    (
        string FullName,
        bool Gender,
        DateTime? DateOfBirth,
        string PhoneNumber,
        string? Address,
        IFormFile? Avatar,
        bool IsActive,
        string? StaffType
    );
}
