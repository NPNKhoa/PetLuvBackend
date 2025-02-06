using UserApi.Application.DTOs.UserDTOs;

namespace UserApi.Application.DTOs.StaffDegreeDTOs
{
    public record StaffDegreeDTO
    (
        Guid DegreeId,
        string DegreeName,
        DateTime SignedDate,
        DateTime ExpiryDate,
        string? DegreeDesc,
        string DegreeImage,
        BriefUserDTO Staff
    );
}
