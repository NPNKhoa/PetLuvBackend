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
    public class FoodRepository(FoodDbContext _context) : IFood
    {
        public async Task<Response> CreateAsync(Food entity)
        {
            try
            {
                var existingFood = await _context.Foods
                    .Where(f => f.FoodName.Trim().ToLower().Equals(entity.FoodName.Trim().ToLower()))
                    .Where(f => f.Brand.Trim().ToLower().Equals(entity.Brand.Trim().ToLower()))
                    .FirstOrDefaultAsync();

                if (existingFood is not null)
                {
                    return new Response(false, 409, "Sản phẩm đã tồn tại");
                }

                await _context.Foods.AddAsync(entity);
                await _context.SaveChangesAsync();

                var (responseData, _) = FoodConversion.FromEntity(entity, null!);

                return new Response(true, 201, "Tạo thành công")
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
                var food = await FindById(id);

                if (food is null)
                {
                    return new Response(false, 404, "Không tìm thấy sản phẩm nào");
                }

                if (food.IsVisible)
                {
                    food.IsVisible = false;
                    await _context.SaveChangesAsync();

                    return new Response(true, 200, "Ẩn sản phẩm thành công");
                }

                if (food.FoodVariants is not null && food.FoodVariants.Count > 0)
                {
                    return new Response(false, 400, "Không thể xóa sản phẩm này vì đã có biến thể");
                }

                // Check order

                _context.Remove(food);
                await _context.SaveChangesAsync();

                return new Response(true, 200, "Xóa thành công");
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
                var foods = await _context.Foods
                    .Skip((pageIndex - 1) * pageSize)
                    .Take(pageSize)
                    .ToListAsync();

                if (foods.Count == 0)
                {
                    return new Response(false, 404, "Không tìm thấy sản phẩm nào");
                }

                var totalFoods = await _context.Foods.CountAsync();

                var (_, responseData) = FoodConversion.FromEntity(null!, foods);

                return new Response(true, 200, "Tìm thấy")
                {
                    Data = new
                    {
                        data = responseData,
                        meta = new
                        {
                            currentPage = pageIndex,
                            totalPages = Math.Ceiling((double)totalFoods / pageSize),
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

        public async Task<Response> GetByAsync(Expression<Func<Food, bool>> predicate)
        {
            try
            {
                var food = await _context.Foods.Where(predicate).FirstOrDefaultAsync();

                if (food is null)
                {
                    return new Response(false, 404, "Không tìm thấy sản phẩm nào");
                }

                var (responseData, _) = FoodConversion.FromEntity(food, null!);

                return new Response(true, 200, "Tìm thấy")
                {
                    Data = responseData,
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
                var food = await FindById(id, true, true);

                if (food is null)
                {
                    return new Response(false, 404, "Không tìm thấy sản phẩm nào");
                }

                var (responseData, _) = FoodConversion.FromEntity(food, null!);

                return new Response(true, 200, "Tìm thấy")
                {
                    Data = responseData,
                };
            }
            catch (Exception ex)
            {
                LogException.LogExceptions(ex);
                return new Response(false, 500, "Internal Server Error");
            }
        }

        public async Task<Response> UpdateAsync(Guid id, Food entity)
        {
            try
            {
                var food = await FindById(id);

                if (food is null)
                {
                    return new Response(false, 404, "Không tìm thấy sản phẩm nào");
                }

                bool hasChanges = entity.FoodName != food.FoodName
                        || entity.FoodDesc != food.FoodDesc
                        || entity.Brand != food.Brand
                        || entity.Ingredient != food.Ingredient
                        || entity.Origin != food.Origin
                        || entity.AgeRange != food.AgeRange
                        || entity.CountInStock != food.CountInStock
                        || entity.IsVisible != food.IsVisible;

                if (!hasChanges)
                {
                    return new Response(false, 400, "Không có thay đổi");
                }

                food.FoodName = entity.FoodName;
                food.FoodDesc = entity.FoodDesc;
                food.Brand = entity.Brand;
                food.Ingredient = entity.Ingredient;
                food.Origin = entity.Origin;
                food.AgeRange = entity.AgeRange;
                food.CountInStock = entity.CountInStock;
                food.IsVisible = entity.IsVisible;

                await _context.SaveChangesAsync();

                var (responseData, _) = FoodConversion.FromEntity(food, null!);

                return new Response(true, 200, "Cập nhật thành công")
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

        public async Task<Food> FindById(Guid id, bool noTracking = false, bool includeOthers = false)
        {
            var query = _context.Foods.Where(f => f.FoodId == id);

            if (noTracking)
            {
                query = query.AsNoTracking();
            }

            if (includeOthers)
            {
                query = query
                    .Include(f => f.FoodVariants)
                        .ThenInclude(fv => fv.Flavor)
                    .Include(f => f.FoodVariants)
                        .ThenInclude(fv => fv.Size)
                    .Include(f => f.FoodImages);
            }

            return await query.FirstOrDefaultAsync() ?? null!;
        }
    }
}
