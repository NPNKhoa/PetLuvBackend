using UserApi.Application.DTOs.StaffDegreeDTOs;
using UserApi.Application.DTOs.UserDTOs;
using UserApi.Domain.Etities;

namespace UserApi.Application.DTOs.Conversions
{
    public static class StaffDegreeConversion
    {
        public static StaffDegree ToEntity(StaffDegreeDTO dto) => new()
        {
            DegreeId = dto.DegreeId,
            DegreeName = dto.DegreeName,
            SignedDate = dto.SignedDate,
            ExpiryDate = dto.ExpiryDate,
            DegreeDesc = dto.DegreeDesc,
            DegreeImage = dto.DegreeImage,
            StaffId = dto.Staff.UserId
        };

        public static StaffDegree ToEntity(CreateUpdateStaffDegreeDTO dto, string imagePath) => new()
        {
            DegreeId = Guid.Empty,
            DegreeName = dto.DegreeName,
            SignedDate = dto.SignedDate,
            ExpiryDate = dto.ExpiryDate,
            DegreeDesc = dto.DegreeDesc,
            DegreeImage = imagePath,
            StaffId = dto.StaffId
        };

        public static (StaffDegreeDTO?, IEnumerable<StaffDegreeDTO>?) FromEntity(StaffDegree? entity, IEnumerable<StaffDegree>? entities)
        {
            if (entity is not null && entities is null)
            {
                return (new StaffDegreeDTO(
                    entity.DegreeId,
                    entity.DegreeName,
                    entity.SignedDate,
                    entity.ExpiryDate,
                    entity.DegreeDesc,
                    entity.DegreeImage,
                    new BriefUserDTO(
                        entity.Staff.UserId,
                        entity.Staff.FullName,
                        entity.Staff.Email,
                        entity.Staff.PhoneNumber,
                        entity.Staff.Avatar,
                        entity.Staff!.StaffType!,
                        entity.Staff.IsActive
                    )
                ), null);
            }

            if (entities is not null && entity is null)
            {
                var dtos = entities.Select(entity => new StaffDegreeDTO(
                    entity.DegreeId,
                    entity.DegreeName,
                    entity.SignedDate,
                    entity.ExpiryDate,
                    entity.DegreeDesc,
                    entity.DegreeImage,
                    new BriefUserDTO(
                        entity.Staff.UserId,
                        entity.Staff.FullName,
                        entity.Staff.Email,
                        entity.Staff.PhoneNumber,
                        entity.Staff.Avatar,
                        entity.Staff!.StaffType!,
                        entity.Staff.IsActive
                    )
                )).ToList();

                return (null, dtos);
            }

            return (null, null);
        }
    }
}
