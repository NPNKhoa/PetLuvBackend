using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using PetLuvSystem.SharedLibrary.Logs;
using PetLuvSystem.SharedLibrary.Responses;
using System.IdentityModel.Tokens.Jwt;
using System.Linq.Expressions;
using System.Security.Claims;
using System.Text;
using UserApi.Application.DTOs.AuthDTOs;
using UserApi.Application.DTOs.Conversions;
using UserApi.Application.Interfaces;
using UserApi.Domain.Etities;
using UserApi.Infrastructure.Data;

namespace UserApi.Infrastructure.Repository
{
    public class UserRepository(UserDbContext _context, IConfiguration _config) : IUser
    {
        public async Task<Response> GetAllAsync(int pageIndex = 1, int pageSize = 10)
        {
            try
            {
                var users = await _context.Users
                    .Skip((pageIndex - 1) * pageSize)
                    .Take(pageSize)
                    .ToListAsync();

                if (users is null || users.Count == 0)
                {
                    return new Response(false, 404, "Không tìm thấy người dùng nào");
                }

                var totalRecords = await _context.Users.CountAsync();

                var (_, responseData) = UserConversion.FromEntity(null!, users);

                return new Response(true, 200, "Lấy danh sách người dùng thành công")
                {
                    Data = new
                    {
                        data = responseData,
                        meta = new
                        {
                            pageIndex,
                            pageSize,
                            totalPage = Math.Ceiling((double)totalRecords / pageSize),
                        }
                    }
                };
            }
            catch (Exception ex)
            {
                LogException.LogExceptions(ex);
                return new Response(false, 500, "Internal Server Error");
            }
        }

        public async Task<Response> GetByAsync(Expression<Func<User, bool>> predicate)
        {
            try
            {
                var user = await _context.Users
                    .Where(predicate)
                    .Include(u => u.WorkSchedules)
                    .Include(u => u.StaffDegrees)
                    .FirstOrDefaultAsync();

                if (user is null)
                {
                    return new Response(false, 404, "Không tìm thấy người dùng nào");
                }

                var (responseData, _) = UserConversion.FromEntity(user, null!);

                return new Response(true, 200, "Lấy thông tin người dùng thành công")
                {
                    Data = responseData
                };
            }
            catch (Exception ex)
            {
                LogException.LogExceptions(ex);
                return new Response(false, 500, "Internal Server Error");
            }
        }

        public async Task<Response> GetByIdAsync(Guid id)
        {
            try
            {
                var user = await FindById(id, false, true);

                if (user is null)
                {
                    return new Response(false, 404, "Không tìm thấy người dùng người id này");
                }

                var (responseData, _) = UserConversion.FromEntity(user, null!);

                return new Response(true, 200, "Lấy thông tin người dùng thành công")
                {
                    Data = responseData
                };
            }
            catch (Exception ex)
            {
                LogException.LogExceptions(ex);
                return new Response(false, 500, "Internal Server Error");
            }
        }

        public async Task<Response> Login(LoginDTO loginDTO)
        {
            try
            {
                var existingUser = await _context.Users.FirstOrDefaultAsync(u => u.Email == loginDTO.Email);

                if (existingUser is null)
                {
                    return new Response(false, 404, "Email không tồn tại");
                }

                var isValidPassword = BCrypt.Net.BCrypt.Verify(loginDTO.Password, existingUser.Password);

                if (!isValidPassword)
                {
                    return new Response(false, 401, "Tài khoản hoặc Mật khẩu không đúng");
                }

                var token = GenerateToken(existingUser);
                var (responseUser, _) = UserConversion.FromEntity(existingUser, null!);

                return new Response(true, 200, "Đăng nhập thành công")
                {
                    Data = new
                    {
                        token,
                        user = responseUser
                    }
                };
            }
            catch (Exception ex)
            {
                LogException.LogExceptions(ex);
                return new Response(false, 500, "Internal Server Error");
            }
        }

        public async Task<Response> Register(RegisterDTO registerDTO, string avatarPath)
        {
            try
            {
                var existingUser = await _context.Users.FirstOrDefaultAsync(u => u.Email == registerDTO.Email);

                if (existingUser is not null)
                {
                    return new Response(false, 409, "Email đã tồn tại");
                }

                var entity = UserConversion.ToEntity(registerDTO, avatarPath);

                entity.Password = BCrypt.Net.BCrypt.HashPassword(registerDTO.Password);

                await _context.Users.AddAsync(entity);
                await _context.SaveChangesAsync();

                return new Response(true, 201, "Đăng ký tài khoản thành công")
                {
                    Data = entity
                };
            }
            catch (Exception ex)
            {
                LogException.LogExceptions(ex);
                return new Response(false, 500, "Internal Server Error");
            }
        }

        public async Task<Response> UpdateAsync(Guid id, User entity)
        {
            try
            {
                var existingUser = await FindById(id);

                if (existingUser is null)
                {
                    return new Response(false, 404, "Không tìm thấy người dùng nào");
                }

                bool hasChanges = existingUser.FullName != entity.FullName
                    || existingUser.Gender != entity.Gender
                    || existingUser.DateOfBirth != entity.DateOfBirth
                    || existingUser.PhoneNumber != entity.PhoneNumber
                    || existingUser.Address != entity.Address
                    || existingUser.Avatar != entity.Avatar
                    || existingUser.IsActive != entity.IsActive
                    || existingUser.StaffType != entity.StaffType;

                if (!hasChanges)
                {
                    return new Response(false, 204, "Không có thay đổi nào");
                }

                existingUser.FullName = entity.FullName;
                existingUser.Gender = entity.Gender;
                existingUser.DateOfBirth = entity.DateOfBirth;
                existingUser.PhoneNumber = entity.PhoneNumber;
                existingUser.Address = entity.Address;
                existingUser.Avatar = entity.Avatar;
                existingUser.IsActive = entity.IsActive;
                existingUser.StaffType = entity.StaffType;
                existingUser.IsActive = entity.IsActive;

                await _context.SaveChangesAsync();

                var (responseData, _) = UserConversion.FromEntity(existingUser, null!);

                return new Response(true, 200, "Cập nhật thông tin người dùng thành công")
                {
                    Data = responseData
                };
            }
            catch (Exception ex)
            {
                LogException.LogExceptions(ex);
                return new Response(false, 500, "Internal Server Error");
            }
        }

        public async Task<Response> ChangePassword(ChangePasswordDTO changePasswordDTO)
        {
            try
            {
                if (changePasswordDTO.NewPassword != changePasswordDTO.ConfirmPassword)
                {
                    return new Response(false, 400, "Mật khẩu mới không khớp");
                }

                var existingUser = await _context.Users.FirstOrDefaultAsync(u => u.Email == changePasswordDTO.Email);

                if (existingUser is null)
                {
                    return new Response(false, 404, "Email không tồn tại");
                }

                var isValidPassword = BCrypt.Net.BCrypt.Verify(changePasswordDTO.OldPassword, existingUser.Password);

                if (!isValidPassword)
                {
                    return new Response(false, 401, "Mật khẩu cũ không đúng");
                }

                existingUser.Password = BCrypt.Net.BCrypt.HashPassword(changePasswordDTO.NewPassword);

                await _context.SaveChangesAsync();

                return new Response(true, 200, "Đổi mật khẩu thành công");
            }
            catch (Exception ex)
            {
                LogException.LogExceptions(ex);
                return new Response(false, 500, "Internal Server Error");
            }
        }

        public async Task<Response> DeleteAsync(Guid id)
        {
            try
            {
                var existingUser = await FindById(id);

                if (existingUser is null)
                {
                    return new Response(false, 404, "Không tìm thấy người dùng nào");
                }

                existingUser.IsActive = false;
                await _context.SaveChangesAsync();

                var (responseData, _) = UserConversion.FromEntity(existingUser, null!);

                return new Response(true, 200, "Khóa người dùng thành công")
                {
                    Data = responseData
                };
            }
            catch (Exception ex)
            {
                LogException.LogExceptions(ex);
                return new Response(false, 500, "Internal Server Error");
            }
        }

        public Task<Response> CreateAsync(User entity)
        {
            throw new NotImplementedException();
        }

        public async Task<User> FindById(Guid id, bool noTracking = false, bool includeDetail = false)
        {
            var query = _context.Users.Where(u => u.UserId == id);

            if (noTracking)
            {
                query = query.AsNoTracking();
            }

            if (includeDetail)
            {
                query = query.Include(u => u.WorkSchedules)
                             .Include(u => u.StaffDegrees);
            }

            return await query.FirstOrDefaultAsync() ?? null!;
        }

        private string GenerateToken(User user)
        {
            var key = Encoding.UTF8.GetBytes(_config["Jwt:Key"]!);
            var securityKey = new SymmetricSecurityKey(key);
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.UserId.ToString()),
                new Claim(ClaimTypes.Name, user.FullName),
                new Claim(ClaimTypes.Email, user.Email),
            };

            if (!string.IsNullOrEmpty(user.StaffType))
            {
                claims.Add(new Claim(ClaimTypes.Role, user.StaffType!));
            }

            var token = new JwtSecurityToken(
                issuer: _config["Jwt:Issuer"],
                audience: _config["Jwt:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddDays(7),
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
