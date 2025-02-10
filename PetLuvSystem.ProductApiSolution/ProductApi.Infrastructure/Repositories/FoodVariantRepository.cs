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
    public class FoodVariantRepository(FoodDbContext _context) : IFoodVariant
    {
        public async Task<Response> CreateAsync(FoodVariant entity)
        {
            try
            {
                var existingVariant = await _context.FoodVariants
                    .Where(fv => fv.FoodId == entity.FoodId
                        && fv.FlavorId == entity.FlavorId
                        && fv.SizeId == entity.SizeId
                        && fv.Price == entity.Price)
                    .FirstOrDefaultAsync();

                if (existingVariant is not null)
                {
                    return new Response(false, 409, "Food Variant already exists");
                }

                var existingFlavor = await _context.FoodFlavors
                    .Where(ff => ff.FlavorId == entity.FlavorId)
                    .FirstOrDefaultAsync();

                if (existingFlavor is null)
                {
                    return new Response(false, 404, "Không tìm thấy vị này");
                }

                var existingSize = await _context.FoodSizes
                    .Where(fs => fs.SizeId == entity.SizeId)
                    .FirstOrDefaultAsync();

                if (existingSize is null)
                {
                    return new Response(false, 404, "Không tìm thấy kích thước này");
                }

                await _context.FoodVariants.AddAsync(entity);
                await _context.SaveChangesAsync();

                entity.Flavor = existingFlavor;
                entity.Size = existingSize;

                var (responseData, _) = FoodVariantConversion.FromEntity(entity, null!);

                return new Response(true, 201, "Food Variant created")
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
                var foodVariants = await _context.FoodVariants
                    .Skip((pageIndex - 1) * pageSize)
                    .Take(pageSize)
                    .ToListAsync();

                if (foodVariants.Count == 0)
                {
                    return new Response(false, 404, "Food Variants not found");
                }

                var totalRecords = await _context.FoodVariants.CountAsync();

                var (_, responseData) = FoodVariantConversion.FromEntity(null!, foodVariants);

                return new Response(true, 200, "Food Variants found")
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

        public async Task<Response> GetByAsync(Expression<Func<FoodVariant, bool>> predicate)
        {
            try
            {
                var foodVariant = await _context.FoodVariants
                    .Include(fv => fv.Food)
                    .Include(fv => fv.Flavor)
                    .Include(fv => fv.Size)
                    .Where(predicate)
                    .FirstOrDefaultAsync();

                if (foodVariant is null)
                {
                    return new Response(false, 404, "Food Variant not found");
                }

                var (responseData, _) = FoodVariantConversion.FromEntity(foodVariant, null);

                return new Response(true, 200, "Food Variant found")
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

        public async Task<Response> GetByKey(Guid FoodId, Guid FlavorId, Guid SizeId)
        {
            try
            {
                var foodVariant = await _context.FoodVariants
                    .Include(fv => fv.Food)
                    .Include(fv => fv.Flavor)
                    .Include(fv => fv.Size)
                    .Where(fv => fv.FoodId == FoodId
                        && fv.FlavorId == FlavorId
                        && fv.SizeId == SizeId)
                    .AsNoTracking()
                    .FirstOrDefaultAsync();

                if (foodVariant is null)
                {
                    return new Response(false, 404, "Food Variant not found");
                }

                var (responseData, _) = FoodVariantConversion.FromEntity(foodVariant, null);

                return new Response(true, 200, "Food Variant found")
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

        public async Task<Response> Delete(Guid FoodId, Guid FlavorId, Guid SizeId)
        {
            try
            {
                var foodVariant = await _context.FoodVariants
                    .Where(fv => fv.FoodId == FoodId
                        && fv.FlavorId == FlavorId
                        && fv.SizeId == SizeId)
                    .FirstOrDefaultAsync();

                if (foodVariant is null)
                {
                    return new Response(false, 404, "Không tìm thấy biển thể nào");
                }

                if (foodVariant.Isvisible)
                {
                    foodVariant.Isvisible = false;
                    await _context.SaveChangesAsync();
                    return new Response(true, 200, "Ấn biển thể thành công");
                }

                _context.Remove(foodVariant);
                await _context.SaveChangesAsync();

                return new Response(true, 200, "Xóa biển thể thành công");
            }
            catch (Exception ex)
            {
                LogException.LogExceptions(ex);
                return new Response(false, 500, "Internal Server Error");
            }
        }

        public async Task<Response> Update(Guid FoodId, Guid FlavorId, Guid SizeId, FoodVariant foodVariant)
        {
            try
            {
                var existingFoodVariant = await _context.FoodVariants
                            .Where(fv => fv.FoodId == FoodId
                                && fv.FlavorId == FlavorId
                                && fv.SizeId == SizeId)
                            .FirstOrDefaultAsync();

                if (existingFoodVariant is null)
                {
                    return new Response(false, 404, "Không tìm thấy biển thể nào");
                }

                bool hasChanges = existingFoodVariant.Price != foodVariant.Price
                    || existingFoodVariant.Isvisible != foodVariant.Isvisible;

                if (!hasChanges)
                {
                    return new Response(false, 204, "Không có thay đổi nào");
                }

                existingFoodVariant.Price = foodVariant.Price;
                existingFoodVariant.Isvisible = foodVariant.Isvisible;

                await _context.SaveChangesAsync();

                var (responseData, _) = FoodVariantConversion.FromEntity(existingFoodVariant, null!);

                return new Response(true, 200, "Cập nhật biển thể thành công")
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

        public async Task<FoodVariant> FindById(Guid FoodId, Guid FlavorId, Guid SizeId, bool noTracking = false, bool includeOthers = false)
        {
            var query = _context.FoodVariants
                .Where(fv => fv.FoodId == FoodId
                    && fv.FlavorId == FlavorId && fv.SizeId == SizeId);

            if (noTracking)
            {
                query = query.AsNoTracking();
            }

            if (includeOthers)
            {
                query = query.Include(fv => fv.Food)
                    .Include(fv => fv.Flavor)
                    .Include(fv => fv.Size);
            }

            return await query.FirstOrDefaultAsync() ?? null!;
        }

        // Unused methods

        public Task<Response> GetByIdAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<Response> UpdateAsync(Guid id, FoodVariant entity)
        {
            throw new NotImplementedException();
        }

        public Task<Response> DeleteAsync(Guid id)
        {
            throw new NotImplementedException();
        }
    }
}
