using UserApi.Application.DTOs.UserDTOs;

namespace UserApi.Application.DTOs.WorkScheduleDTOs
{
    public record WorkScheduleDTO
    (
        Guid WorkScheduleId,
        Guid UserId,
        BriefUserDTO User
    );
}
