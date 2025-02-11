using UserApi.Application.DTOs.UserDTOs;
using UserApi.Application.DTOs.WorkScheduleDetailDTOs;

namespace UserApi.Application.DTOs.WorkScheduleDTOs
{
    public record WorkScheduleDTO
    (
        Guid WorkScheduleId,
        BriefUserDTO Staff,
        IEnumerable<WorkScheduleDetailDTO> WorkScheduleDetails
    );
}
