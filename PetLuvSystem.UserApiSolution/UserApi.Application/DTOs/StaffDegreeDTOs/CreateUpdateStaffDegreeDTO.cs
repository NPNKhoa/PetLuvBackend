using Microsoft.AspNetCore.Http;

namespace UserApi.Application.DTOs.StaffDegreeDTOs
{
    public record CreateUpdateStaffDegreeDTO
    (
        Guid StaffId,
        string DegreeName,
        DateTime SignedDate,
        DateTime ExpiryDate,
        string? DegreeDesc,
        IFormFile DegreeImage
    );
}
