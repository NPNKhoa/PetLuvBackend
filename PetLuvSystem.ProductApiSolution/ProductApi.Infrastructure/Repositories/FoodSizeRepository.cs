using Microsoft.EntityFrameworkCore;
using PetLuvSystem.SharedLibrary.Logs;
using PetLuvSystem.SharedLibrary.Responses;
using ProductApi.Application.DTOs.Conversions;
using ProductApi.Application.Interfaces;
using ProductApi.Domain.Entities;
using ProductApi.Infrastructure.Data;
using System.Linq.Expressions;

namespace ProductApi.Infrastructure.Repositories
{
    public class FoodSizeRepository(FoodDbContext _context) : IFoodSize
    {
        public async Task<Response> CreateAsync(FoodSize entity)
        {
            try
            {
                var existingSize = await _context.FoodSizes
                    .Where(fv => fv.Size.ToLower().Equals(entity.Size.Trim().ToLower()))
                    .FirstOrDefaultAsync();

                if (existingSize is not null)
                {
                    return new Response(false, 409, "size đã tồn tại!");
                }

                await _context.FoodSizes.AddAsync(entity);
                await _context.SaveChangesAsync();

                var (responseData, _) = FoodSizeConversion.FromEntity(entity, null!);

                return new Response(true, 201, "Tạo size thành công!")
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

        public async Task<Response> DeleteAsync(Guid id)
        {
            try
            {
                var foodSize = await FindById(id);

                if (foodSize is null)
                {
                    return new Response(false, 404, $"Không tìm thấy size với id {id}");
                }

                if (foodSize.Isvisible)
                {
                    foodSize.Isvisible = false;
                    await _context.SaveChangesAsync();
                    return new Response(true, 200, "Ẩn size thành công!");
                }

                if (foodSize.FoodVariants is not null && foodSize.FoodVariants.Count > 0)
                {
                    return new Response(false, 400, "Không thể xóa Size này vì có những biến thể liên quan");
                }

                _context.FoodSizes.Remove(foodSize);
                await _context.SaveChangesAsync();

                var (responseData, _) = FoodSizeConversion.FromEntity(foodSize, null!);

                return new Response(true, 200, "Xóa size thành công!")
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

        public async Task<Response> GetAllAsync(int pageIndex = 1, int pageSize = 10)
        {
            try
            {
                var foodSizes = await _context.FoodSizes
                    .Skip((pageIndex - 1) * pageSize)
                    .Take(pageSize)
                    .ToListAsync();

                if (foodSizes is null || foodSizes.Count == 0)
                {
                    return new Response(false, 404, "Không tìm thấy size!");
                }

                var totalRecords = await _context.FoodSizes.CountAsync();

                var (_, responseData) = FoodSizeConversion.FromEntity(null!, foodSizes);

                return new Response(true, 200, "Tìm thấy!")
                {
                    Data = new
                    {
                        data = responseData,
                        meta = new
                        {
                            currentPage = pageIndex,
                            totalPages = Math.Ceiling((double)totalRecords / pageSize)
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

        public async Task<Response> GetByAsync(Expression<Func<FoodSize, bool>> predicate)
        {
            try
            {
                var foodSize = await _context.FoodSizes.Where(predicate).FirstOrDefaultAsync();

                if (foodSize is null)
                {
                    return new Response(false, 404, "Không tìm thấy size!");
                }

                var (responseData, _) = FoodSizeConversion.FromEntity(foodSize, null!);

                return new Response(true, 200, "Tìm thấy!")
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
                var foodSize = await FindById(id, true, true);

                if (foodSize is null)
                {
                    return new Response(false, 404, $"Không tìm thấy size với id {id}");
                }

                var (responseData, _) = FoodSizeConversion.FromEntity(foodSize, null!);

                return new Response(true, 200, "Tìm thấy!")
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

        public async Task<Response> UpdateAsync(Guid id, FoodSize entity)
        {
            try
            {
                var existingSize = await FindById(id);

                if (existingSize is null)
                {
                    return new Response(false, 404, $"Không tìm thấy size với id {id}");
                }

                bool hasChanges = existingSize.Size != entity.Size;

                if (!hasChanges)
                {
                    return new Response(false, 204, "Không có thay đổi");
                }

                existingSize.Size = entity.Size;
                await _context.SaveChangesAsync();

                var (responseData, _) = FoodSizeConversion.FromEntity(existingSize, null!);

                return new Response(true, 200, "Cập nhật size thành công!")
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

        public async Task<FoodSize> FindById(Guid id, bool noTracking = false, bool includeOthers = false)
        {
            var query = _context.FoodSizes.Where(fv => fv.SizeId == id);

            if (noTracking)
            {
                query = query.AsNoTracking();
            }

            if (includeOthers)
            {
                query = query.Include(fv => fv.FoodVariants);
            }

            return await query.FirstOrDefaultAsync() ?? null!;
        }
    }
}
