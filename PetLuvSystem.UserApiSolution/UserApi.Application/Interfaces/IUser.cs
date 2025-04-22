using PetLuvSystem.SharedLibrary.Interfaces;
using PetLuvSystem.SharedLibrary.Responses;

using UserApi.Application.DTOs.AuthDTOs;
using UserApi.Domain.Etities;

namespace UserApi.Application.Interfaces
{
    public interface IUser : IGenericInterface<User>
    {
        public Task<Response> Register(RegisterDTO registerDTO, string avatarPath);
        public Task<Response> Login(LoginDTO loginDTO);
        public Task<Response> ChangePassword(ChangePasswordDTO changePasswordDTO);
        public Task<User> FindById(Guid id, bool noTracking = false, bool includeDetail = false);
        public Task<Response> ToggleAccountStatus(Guid userId);
    }
}
