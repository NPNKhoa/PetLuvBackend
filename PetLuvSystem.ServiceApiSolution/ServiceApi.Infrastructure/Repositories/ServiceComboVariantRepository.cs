using Microsoft.EntityFrameworkCore;
using PetLuvSystem.SharedLibrary.Logs;
using PetLuvSystem.SharedLibrary.Responses;
using ServiceApi.Application.DTOs.Conversions;
using ServiceApi.Application.Interfaces;
using ServiceApi.Domain.Entities;
using ServiceApi.Infrastructure.Data;
using System.Linq.Expressions;

namespace ServiceApi.Infrastructure.Repositories
{
    public class ServiceComboVariantRepository(ServiceDbContext context, IServiceComboCachingService _cacheService) : IServiceComboVariant
    {
        public async Task<Response> CreateAsync(ServiceComboVariant entity)
        {
            try
            {
                var existingCombo = await FindByKey(entity.ServiceComboId, entity.BreedId, entity.WeightRange!);

                if (existingCombo is not null)
                {
                    return new Response(false, 409, "Service combo already exist");
                }

                await context.AddAsync(entity);
                await context.SaveChangesAsync();

                var entities = await context.ServiceComboVariants.Where(x => x.IsVisible == true).ToListAsync();
                await _cacheService.Updatecache(entities);

                var (responseData, _) = ServiceComboVariantConversion.FromEntity(entity, null);

                return new Response(true, 201, "Service combo variant created")
                {
                    Data = new { data = responseData }
                };
            }
            catch (Exception ex)
            {
                LogException.LogExceptions(ex);
                return new Response(false, 500, "Internal Server Error");
            }

        }

        public async Task<Response> DeleteAsync(Guid serviceComboId, Guid breedId, string WeightRange)
        {
            try
            {
                var serviceComboVariant = await FindByKey(serviceComboId, breedId, WeightRange);

                if (serviceComboVariant is null)
                {
                    return new Response(false, 404, "Service combo variant not found");
                }

                var (responseData, _) = ServiceComboVariantConversion.FromEntity(serviceComboVariant, null!);

                if (serviceComboVariant.IsVisible)
                {
                    serviceComboVariant.IsVisible = false;

                    var combos = await context.ServiceComboVariants.Where(x => x.IsVisible == true).ToListAsync();
                    await context.SaveChangesAsync();

                    await _cacheService.Updatecache(combos);

                    return new Response(true, 200, "Service combo variant was made hidden successfully")
                    {
                        Data = new { data = responseData }
                    };
                }

                context.Remove(serviceComboVariant);
                await context.SaveChangesAsync();

                var comboss = await context.ServiceComboVariants.Where(x => x.IsVisible == true).ToListAsync();
                await _cacheService.Updatecache(comboss);

                return new Response(true, 200, "Service combo variant deleted")
                {
                    Data = new { data = responseData }
                };
            }
            catch (Exception ex)
            {
                LogException.LogExceptions(ex);
                return new Response(false, 500, "Internal Server Error");
            }
        }

        public async Task<ServiceComboVariant> FindByKey(Guid serviceComboId, Guid breedId, string WeightRange, bool noTracking = false)
        {
            var query = context.ServiceComboVariants
                .Where(x => x.ServiceComboId == serviceComboId && x.BreedId == breedId && x.WeightRange == WeightRange);

            if (noTracking)
            {
                query = query.AsNoTracking();
            }

            return await query.FirstOrDefaultAsync() ?? null!;
        }

        public async Task<Response> GetByKeyAsync(Guid serviceComboId, Guid breedId, string WeightRange)
        {
            try
            {
                var serviceComboVariant = await FindByKey(serviceComboId, breedId, WeightRange, true);

                if (serviceComboVariant is null || serviceComboVariant.IsVisible == false)
                {
                    return new Response(false, 404, "Biến thể này đã bị xóa hoặc không tồn tại");
                }

                var (responseData, _) = ServiceComboVariantConversion.FromEntity(serviceComboVariant, null);

                return new Response(true, 200, "Service combo variant found")
                {
                    Data = new { data = responseData }
                };
            }
            catch (Exception ex)
            {
                LogException.LogExceptions(ex);
                return new Response(false, 500, "Internal Server Error");
            }
        }

        public async Task<Response> GetByServiceComboAsync(Guid serviceComboId)
        {
            try
            {
                var serviceComboVariants = await context.ServiceComboVariants
                    .Where(x => x.ServiceComboId == serviceComboId)
                    .AsNoTracking()
                    .ToListAsync();

                if (serviceComboVariants is null || serviceComboVariants.Count == 0)
                {
                    return new Response(false, 404, "Service combo variants not found");
                }

                var (_, responseData) = ServiceComboVariantConversion.FromEntity(null, serviceComboVariants);

                return new Response(true, 200, "Service combo variants found")
                {
                    Data = new { data = responseData }
                };
            }
            catch (Exception ex)
            {
                LogException.LogExceptions(ex);
                return new Response(false, 500, "Internal Server Error");
            }
        }

        public async Task<Response> UpdateAsync(Guid serviceComboId, Guid breedId, string WeightRange, decimal comboPrice)
        {
            try
            {
                var serviceComboVariant = await FindByKey(serviceComboId, breedId, WeightRange);

                if (serviceComboVariant is null)
                {
                    return new Response(false, 404, "Service combo variant not found");
                }

                serviceComboVariant.ComboPrice = comboPrice;

                context.Update(serviceComboVariant);
                await context.SaveChangesAsync();

                var entities = await context.ServiceComboVariants.Where(x => x.IsVisible == true).ToListAsync();
                await _cacheService.Updatecache(entities);

                var (responseData, _) = ServiceComboVariantConversion.FromEntity(serviceComboVariant, null);

                return new Response(true, 200, "Service combo variant updated")
                {
                    Data = new { data = responseData }
                };
            }
            catch (Exception ex)
            {
                LogException.LogExceptions(ex);
                return new Response(false, 500, "Internal Server Error");
            }
        }

        public Task<Response> UpdateAsync(Guid id, ServiceComboVariant entity)
        {
            throw new NotImplementedException();
        }

        public Task<Response> DeleteAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public async Task<Response> GetAllAsync(int pageIndex, int pageSize)
        {
            try
            {
                var entities = await context.ServiceComboVariants.ToListAsync();

                if (!entities.Any())
                {
                    return new Response(false, 404, "Không tìm thấy biến thể của combo");
                }

                return new Response(true, 200, "Found")
                {
                    Data = entities
                };
            }
            catch (Exception ex)
            {
                LogException.LogExceptions(ex);
                return new Response(false, 500, "Internal Server Error");
            }
        }

        public Task<Response> GetByAsync(Expression<Func<ServiceComboVariant, bool>> predicate)
        {
            throw new NotImplementedException();
        }

        public Task<Response> GetByIdAsync(Guid id)
        {
            throw new NotImplementedException();
        }
    }
}
