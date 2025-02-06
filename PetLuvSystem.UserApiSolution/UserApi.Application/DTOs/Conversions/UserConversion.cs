using UserApi.Application.DTOs.AuthDTOs;
using UserApi.Application.DTOs.StaffDegreeDTOs;
using UserApi.Application.DTOs.UserDTOs;
using UserApi.Application.DTOs.WorkScheduleDTOs;
using UserApi.Domain.Etities;

namespace UserApi.Application.DTOs.Conversions
{
    public static class UserConversion
    {
        public static User ToEntity(UserDTO dto) => new()
        {
            UserId = dto.UserId,
            FullName = dto.FullName,
            Gender = dto.Gender,
            DateOfBirth = dto.DateOfBirth,
            Email = dto.Email,
            PhoneNumber = dto.PhoneNumber,
            Address = dto.Address,
            Avatar = dto.Avatar,
            IsActive = dto.IsActive,
            StaffType = dto.StaffType,
            CreatedDate = dto.CreatedDate,
        };

        public static User ToEntity(RegisterDTO dto, string avatar) => new()
        {
            UserId = Guid.Empty,
            FullName = dto.FullName,
            Gender = dto.Gender,
            DateOfBirth = dto.DateOfBirth,
            Email = dto.Email,
            PhoneNumber = dto.PhoneNumber,
            Address = dto.Address,
            Avatar = avatar,
            IsActive = true,
            StaffType = string.Empty,
        };

        public static User ToEntity(UpdateUserDTO dto, string avatar) => new()
        {
            UserId = Guid.Empty,
            FullName = dto.FullName,
            Gender = dto.Gender,
            DateOfBirth = dto.DateOfBirth,
            PhoneNumber = dto.PhoneNumber,
            Address = dto.Address,
            Avatar = avatar,
            IsActive = true,
            StaffType = dto.StaffType,
        };

        public static (UserDTO?, IEnumerable<UserDTO>?) FromEntity(User entity, IEnumerable<User> entities)
        {
            if (entity is not null && entities is null)
            {
                var singleDTO = new UserDTO
                (
                    entity.UserId,
                    entity.FullName,
                    entity.Gender,
                    entity.DateOfBirth,
                    entity.Email,
                    entity.PhoneNumber,
                    entity.Address,
                    entity.Avatar,
                    entity.IsActive,
                    entity.StaffType,
                    entity.CreatedDate,
                    new List<WorkScheduleDTO>(),
                    new List<StaffDegreeDTO>()
                );

                return (singleDTO, null);
            }

            if (entities is not null && entity is null)
            {
                var listDTO = entities.Select(entity => new UserDTO
                (
                    entity.UserId,
                    entity.FullName,
                    entity.Gender,
                    entity.DateOfBirth,
                    entity.Email,
                    entity.PhoneNumber,
                    entity.Address,
                    entity.Avatar,
                    entity.IsActive,
                    entity.StaffType,
                    entity.CreatedDate,
                    new List<WorkScheduleDTO>(),
                    new List<StaffDegreeDTO>()
                )).ToList();

                return (null, listDTO);
            }

            return (null, null);
        }
    }
}
