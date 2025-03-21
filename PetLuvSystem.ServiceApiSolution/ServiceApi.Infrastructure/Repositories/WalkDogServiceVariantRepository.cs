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
    public class WalkDogServiceVariantRepository(ServiceDbContext context, IBreedMappingService _breedMappingClient) : IWalkDogServiceVariant
    {
        public async Task<Response> CreateAsync(WalkDogServiceVariant entity)
        {
            try
            {
                var existingVariant = await context.WalkDogServiceVariants.FirstOrDefaultAsync(
                    v => v.ServiceId == entity.ServiceId
                        && v.PricePerPeriod == entity.PricePerPeriod
                        && v.Period == entity.Period
                );

                if (existingVariant != null)
                {
                    return new Response(false, 409, "This Service Variant already exists");
                }

                var serviceVariant = await context.WalkDogServiceVariants.AddAsync(entity)
                                            ?? throw new Exception("Failed to create service variant");

                await context.SaveChangesAsync();

                var (responseData, _) = WalkDogServiceVariantConversion.FromEntity(entity, null);

                return new Response(true, 201, "Service Variant created successfully")
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

        public async Task<Response> DeleteAsync(Guid serviceId, Guid breedId)
        {
            try
            {
                var existingVariant = await FindByKeyAsync(serviceId, breedId);

                if (existingVariant == null)
                {
                    return new Response(false, 404, "Service Variant Not Found");
                }

                var (responseData, _) = WalkDogServiceVariantConversion.FromEntity(existingVariant, null);

                if (existingVariant.IsVisible)
                {
                    existingVariant.IsVisible = false;
                    await context.SaveChangesAsync();

                    return new Response(true, 200, "Service Variant was made as hidden successfully")
                    {
                        Data = new { data = responseData }
                    };
                }

                context.WalkDogServiceVariants.Remove(existingVariant);
                await context.SaveChangesAsync();

                return new Response(true, 200, "Service Variant deleted successfully")
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

        public async Task<Response> GetByKeyAsync(Guid serviceId, Guid breedId)
        {
            try
            {
                var serviceVariant = await FindByKeyAsync(serviceId, breedId, true);

                if (serviceVariant == null || serviceVariant.IsVisible == false)
                {
                    return new Response(false, 404, "Biến thể này đã bị xóa hoặc không tồn tại");
                }

                var (responseData, _) = WalkDogServiceVariantConversion.FromEntity(serviceVariant, null);

                return new Response(true, 200, "Service Variant retrieved successfully")
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

        public async Task<Response> GetByServiceAsync(Guid serviceId)
        {
            try
            {
                var serviceVariants = await context.WalkDogServiceVariants
                    .Where(s => s.ServiceId == serviceId).AsNoTracking().ToListAsync();

                if (serviceVariants is null || serviceVariants.Count == 0)
                {
                    return new Response(false, 404, "Service Variants not found");
                }

                var breedMapping = await _breedMappingClient.GetBreedMappingAsync();
                var (_, responseData) = WalkDogServiceVariantConversion.FromEntity(null, serviceVariants, breedMapping);

                return new Response(true, 200, "Service Variants retrieved successfully")
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

        public async Task<Response> UpdateAsync(Guid serviceId, Guid breedId, decimal pricePerPeriod)
        {
            try
            {
                var existingVariant = await FindByKeyAsync(serviceId, breedId);

                if (existingVariant == null)
                {
                    return new Response(false, 404, "Service Variant Not Found");
                }

                existingVariant.PricePerPeriod = pricePerPeriod;

                context.WalkDogServiceVariants.Update(existingVariant);
                await context.SaveChangesAsync();

                var (responseData, _) = WalkDogServiceVariantConversion.FromEntity(existingVariant, null);

                return new Response(true, 200, "Service Variant updated successfully")
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

        public async Task<WalkDogServiceVariant> FindByKeyAsync(Guid serviceId, Guid breedId, bool noTracking = false)
        {
            var query = context.WalkDogServiceVariants.Where(x => x.ServiceId == serviceId && x.BreedId == breedId);

            if (noTracking)
            {
                query = query.AsNoTracking();
            }

            return await query.FirstOrDefaultAsync() ?? null!;
        }

        public Task<Response> DeleteAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public async Task<Response> GetAllAsync(int pageIndex, int pageSize)
        {
            try
            {
                var entities = await context.WalkDogServiceVariants.ToListAsync();

                if (entities is null || !entities.Any())
                {
                    return new Response(false, 404, "Không tìm thấy biển thể nào của walk dog trong database");
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

        public Task<Response> GetByAsync(Expression<Func<WalkDogServiceVariant, bool>> predicate)
        {
            throw new NotImplementedException();
        }

        public Task<Response> GetByIdAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<Response> UpdateAsync(Guid id, WalkDogServiceVariant entity)
        {
            throw new NotImplementedException();
        }
    }
}
