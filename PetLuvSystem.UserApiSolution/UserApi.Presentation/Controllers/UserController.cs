using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PetLuvSystem.SharedLibrary.Helpers.CloudinaryHelper;
using PetLuvSystem.SharedLibrary.Logs;
using PetLuvSystem.SharedLibrary.Responses;
using System.Security.Claims;
using UserApi.Application.DTOs.AuthDTOs;
using UserApi.Application.DTOs.Conversions;
using UserApi.Application.DTOs.UserDTOs;
using UserApi.Application.Interfaces;

namespace UserApi.Presentation.Controllers
{
    [Route("api/users")]
    [ApiController]
    public class UserController(IUser _user, IStaffDegree _staffDegree, IWorkSchedule _schedule) : ControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> GetAllUsers([FromQuery] int pageIndex = 1, [FromQuery] int pageSize = 10)
        {
            var response = await _user.GetAllAsync(pageIndex, pageSize);
            return response.ToActionResult(this);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetUserById(Guid id)
        {
            var response = await _user.GetByIdAsync(id);
            return response.ToActionResult(this);
        }

        [HttpGet("/api/staff/{id}/degree")]
        public async Task<IActionResult> GetDegreeByStaff(Guid id)
        {
            var response = await _staffDegree.GetByAsync(x => x.StaffId == id);
            return response.ToActionResult(this);
        }

        [HttpGet("/api/staff/{id}/schedule")]
        public async Task<IActionResult> GetScheduleByStaff(Guid id)
        {
            var response = await _schedule.GetByAsync(x => x.StaffId == id);
            return response.ToActionResult(this);
        }

        [HttpPost("/api/register")]
        public async Task<IActionResult> Register([FromForm] RegisterDTO registerDTO)
        {
            if (!ModelState.IsValid)
            {
                var errorMessages = string.Join("; ", ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage));

                return BadRequest(new Response(false, 400, errorMessages));
            }

            string avatarPath = string.Empty;
            if (registerDTO.Avatar is not null)
            {
                try
                {
                    avatarPath = await CloudinaryHelper.UploadImageToCloudinary(registerDTO.Avatar, "UserAvts");
                }
                catch (Exception ex)
                {
                    LogException.LogExceptions(ex);
                    return (new Response(false, 400, "Lỗi trong quá trình upload ảnh lên cloud")).ToActionResult(this);
                }
            }

            var response = await _user.Register(registerDTO, avatarPath);
            return response.ToActionResult(this);
        }

        [HttpPost("/api/login")]
        public async Task<IActionResult> Login([FromBody] LoginDTO loginDTO)
        {
            if (!ModelState.IsValid)
            {
                var errorMessages = string.Join("; ", ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage));

                return BadRequest(new Response(false, 400, errorMessages));
            }

            var response = await _user.Login(loginDTO);
            return response.ToActionResult(this);
        }

        [HttpPost("/api/change-password")]
        [Authorize]
        public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordDTO changePasswordDTO)
        {
            if (!ModelState.IsValid)
            {
                var errorMessages = string.Join("; ", ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage));

                return BadRequest(new Response(false, 400, errorMessages));
            }

            var emailFromToken = User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Email)?.Value;

            if (emailFromToken is null)
            {
                return (new Response(false, 401, "Token không hợp lệ")).ToActionResult(this);
            }

            var response = await _user.ChangePassword(changePasswordDTO with { Email = emailFromToken });
            return response.ToActionResult(this);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUser(Guid id, [FromForm] UpdateUserDTO dto)
        {
            if (!ModelState.IsValid)
            {
                var errorMessages = string.Join("; ", ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage));

                return BadRequest(new Response(false, 400, errorMessages));
            }

            string avatarPath = string.Empty;
            if (dto.Avatar is not null)
            {
                try
                {
                    avatarPath = await CloudinaryHelper.UploadImageToCloudinary(dto.Avatar, "UserAvts");
                }
                catch (Exception ex)
                {
                    LogException.LogExceptions(ex);
                    return (new Response(false, 500, "Error while uploading image")).ToActionResult(this);
                }
            }

            var user = UserConversion.ToEntity(dto, avatarPath);
            user.Avatar = avatarPath;

            var response = await _user.UpdateAsync(id, user);
            return response.ToActionResult(this);
        }

        [HttpPut("{userId}/toggle-account-status")]
        public async Task<IActionResult> ToggleAccoutStatus(Guid userId)
        {
            return (await _user.ToggleAccountStatus(userId)).ToActionResult(this);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(Guid id)
        {
            var response = await _user.DeleteAsync(id);
            return response.ToActionResult(this);
        }

        private static async Task<string> HandleUploadImage(IFormFile imageFile)
        {
            var randomSuffix = new Random().Next(1000, 9999);
            var fileName = DateTime.Now.ToString("yyyyMMddHHmmssfff") + "_" +
                randomSuffix + Path.GetExtension(imageFile.FileName);

            var directoryPath = Path.Combine("wwwroot", "images");
            if (!Directory.Exists(directoryPath))
            {
                Directory.CreateDirectory(directoryPath);
            }

            var filePath = Path.Combine(directoryPath, fileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await imageFile.CopyToAsync(stream);
            }

            return $"/images/{fileName}";
        }

    }
}
