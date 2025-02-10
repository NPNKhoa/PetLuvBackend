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
    public class FoodFlavorRepository(FoodDbContext _context) : IFoodFlavor
    {
        public async Task<Response> CreateAsync(FoodFlavor entity)
        {
            try
            {
                var existingFlavor = await _context.FoodFlavors
                    .Where(fv => fv.Flavor.ToLower().Equals(entity.Flavor.Trim().ToLower()))
                    .FirstOrDefaultAsync();

                if (existingFlavor is not null)
                {
                    return new Response(false, 409, "Hương vị đã tồn tại!");
                }

                await _context.FoodFlavors.AddAsync(entity);
                await _context.SaveChangesAsync();

                var (responseData, _) = FoodFlavorConversion.FromEntity(entity, null!);

                return new Response(true, 201, "Tạo hương vị thành công!")
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
                var foodFlavor = await FindById(id);

                if (foodFlavor is null)
                {
                    return new Response(false, 404, $"Không tìm thấy hương vị với id {id}");
                }

                if (foodFlavor.IsVisible)
                {
                    foodFlavor.IsVisible = false;
                    await _context.SaveChangesAsync();
                    return new Response(true, 200, "Ẩn hương vị thành công!");
                }

                if (foodFlavor.FoodVariants is not null && foodFlavor.FoodVariants.Count > 0)
                {
                    return new Response(false, 400, "Không thể xóa flavor này vì có những biến thể liên quan");
                }

                _context.FoodFlavors.Remove(foodFlavor);
                await _context.SaveChangesAsync();

                var (responseData, _) = FoodFlavorConversion.FromEntity(foodFlavor, null!);

                return new Response(true, 200, "Xóa hương vị thành công!")
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
                var foodFlavors = await _context.FoodFlavors
                    .Skip((pageIndex - 1) * pageSize)
                    .Take(pageSize)
                    .ToListAsync();

                if (foodFlavors is null || foodFlavors.Count == 0)
                {
                    return new Response(false, 404, "Không tìm thấy hương vị!");
                }

                var totalRecords = await _context.FoodFlavors.CountAsync();

                var (_, responseData) = FoodFlavorConversion.FromEntity(null!, foodFlavors);

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

        public async Task<Response> GetByAsync(Expression<Func<FoodFlavor, bool>> predicate)
        {
            try
            {
                var foodFlavor = await _context.FoodFlavors.Where(predicate).FirstOrDefaultAsync();

                if (foodFlavor is null)
                {
                    return new Response(false, 404, "Không tìm thấy hương vị!");
                }

                var (responseData, _) = FoodFlavorConversion.FromEntity(foodFlavor, null!);

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
                var foodFlavor = await FindById(id, true, true);

                if (foodFlavor is null)
                {
                    return new Response(false, 404, $"Không tìm thấy hương vị với id {id}");
                }

                var (responseData, _) = FoodFlavorConversion.FromEntity(foodFlavor, null!);

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

        public async Task<Response> UpdateAsync(Guid id, FoodFlavor entity)
        {
            try
            {
                var existingFlavor = await FindById(id);

                if (existingFlavor is null)
                {
                    return new Response(false, 404, $"Không tìm thấy hương vị với id {id}");
                }

                bool hasChanges = !existingFlavor.Flavor.Equals(entity.Flavor);

                if (!hasChanges)
                {
                    return new Response(false, 204, "Không có thay đổi");
                }

                existingFlavor.Flavor = entity.Flavor;
                await _context.SaveChangesAsync();

                var (responseData, _) = FoodFlavorConversion.FromEntity(existingFlavor, null!);

                return new Response(true, 200, "Cập nhật hương vị thành công!")
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

        public async Task<FoodFlavor> FindById(Guid id, bool noTracking = false, bool includeOthers = false)
        {
            var query = _context.FoodFlavors.Where(fv => fv.FlavorId == id);

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
