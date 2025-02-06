using UserApi.Application.DTOs.StaffDegreeDTOs;
using UserApi.Application.DTOs.WorkScheduleDTOs;

namespace UserApi.Application.DTOs.UserDTOs
{
    public record UserDTO
    (
        Guid UserId,
        string FullName,
        bool Gender,
        DateTime? DateOfBirth,
        string Email,
        string PhoneNumber,
        string? Address,
        string? Avatar,
        bool IsActive,
        string? StaffType,
        DateTime CreatedDate,
        ICollection<WorkScheduleDTO>? WorkSchedules,
        ICollection<StaffDegreeDTO>? StaffDegrees
    );
}
