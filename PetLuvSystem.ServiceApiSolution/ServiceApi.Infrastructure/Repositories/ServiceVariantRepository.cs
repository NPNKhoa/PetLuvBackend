﻿using Microsoft.EntityFrameworkCore;
using PetLuvSystem.SharedLibrary.Logs;
using PetLuvSystem.SharedLibrary.Responses;
using ServiceApi.Application.DTOs.Conversions;
using ServiceApi.Application.Interfaces;
using ServiceApi.Domain.Entities;
using ServiceApi.Infrastructure.Data;
using System.Linq.Expressions;

namespace ServiceApi.Infrastructure.Repositories
{
    public class ServiceVariantRepository(ServiceDbContext context, IServiceVariantCachingService _cacheService, IBreedMappingService _breedMappingClient) : IServiceVariant
    {
        public async Task<Response> CreateAsync(ServiceVariant entity)
        {
            try
            {
                var existingVariant = await GetByKeyAsync(entity.ServiceId, entity.BreedId, entity.PetWeightRange!.Trim());

                if (existingVariant.Data != null)
                {
                    return new Response(false, 409, "This Service Variant already exists");
                }

                LogException.LogInformation("[Service service] Variant.Isvisible: " + entity.IsVisible.ToString());

                var serviceVariant = await context.ServiceVariants.AddAsync(entity)
                                            ?? throw new Exception("Failed to create service variant");

                await context.SaveChangesAsync();

                var entities = await context.ServiceVariants.Where(s => s.IsVisible == true).ToListAsync();
                await _cacheService.UpdateCacheAsync(entities);

                var (responseData, _) = ServiceVariantConversion.FromEntity(entity, null);

                return new Response(true, 201, "Service Variant created successfully")
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

        public async Task<Response> GetByAsync(Expression<Func<ServiceVariant, bool>> predicate)
        {
            try
            {
                var serviceVariant = await context.ServiceVariants.Where(predicate).AsNoTracking().FirstOrDefaultAsync();

                if (serviceVariant is null)
                {
                    return new Response(false, 404, "Service Variant Not Found");
                }

                var (responseData, _) = ServiceVariantConversion.FromEntity(serviceVariant, null);

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

        public async Task<Response> GetByKeyAsync(Guid serviceId, Guid breedId, string petWeightRange)
        {
            try
            {
                var serviceVariant = await FindByKeyAsync(serviceId, breedId, petWeightRange, true);

                if (serviceVariant is null || serviceVariant.IsVisible == false)
                {
                    return new Response(false, 404, "Biến thể này đã bị xóa hoặc không tồn tại");
                }

                var (responseData, _) = ServiceVariantConversion.FromEntity(serviceVariant, null);

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
                var serviceVariants = await context.ServiceVariants.Where(s => s.ServiceId == serviceId).ToListAsync();

                if (serviceVariants is null || serviceVariants.Count == 0)
                {
                    return new Response(false, 404, "Service Variants not found");
                }

                var breedMapping = await _breedMappingClient.GetBreedMappingAsync();

                var (_, responseDatas) = ServiceVariantConversion.FromEntity(null, serviceVariants, breedMapping);

                return new Response(true, 200, "Service Variants retrieved successfully")
                {
                    Data = new { data = responseDatas }
                };

            }
            catch (Exception ex)
            {
                LogException.LogExceptions(ex);
                return new Response(false, 500, "Internal Server Error");
            }
        }

        public async Task<Response> UpdateAsync(Guid serviceId, Guid breedId, string petWeightRange, ServiceVariant entity)
        {
            try
            {
                var existingServiceVariant = await FindByKeyAsync(serviceId, breedId, petWeightRange, false);

                if (existingServiceVariant is null)
                {
                    return new Response(false, 404, "Không tìm thấy biến thể này!");
                }

                bool hasChanges = entity.Price != existingServiceVariant.Price
                                    || entity.BreedId != existingServiceVariant.BreedId
                                    || entity.PetWeightRange != existingServiceVariant.PetWeightRange
                                    || entity.IsVisible != existingServiceVariant.IsVisible
                                    || entity.EstimateTime != existingServiceVariant.EstimateTime;

                if (hasChanges)
                {
                    LogException.LogInformation($"[Service] The entity has change. Updating...");

                    existingServiceVariant.Price = entity.Price;
                    existingServiceVariant.BreedId = entity.BreedId;
                    existingServiceVariant.PetWeightRange = entity.PetWeightRange;
                    existingServiceVariant.EstimateTime = entity.EstimateTime;
                    existingServiceVariant.IsVisible = entity.IsVisible;

                    await context.SaveChangesAsync();
                }

                var (responseData, _) = ServiceVariantConversion.FromEntity(existingServiceVariant, null);

                var entities = await context.ServiceVariants.Where(s => s.IsVisible == true).ToListAsync();
                await _cacheService.UpdateCacheAsync(entities);

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

        public async Task<Response> DeleteAsync(Guid serviceId, Guid breedId, string petWeightRange)
        {
            try
            {
                var existingServiceVariant = await FindByKeyAsync(serviceId, breedId, petWeightRange, false);

                if (existingServiceVariant is null)
                {
                    return new Response(false, 404, "Service Variant not found");
                }

                var (responseData, _) = ServiceVariantConversion.FromEntity(existingServiceVariant, null);

                if (existingServiceVariant.IsVisible == true)
                {
                    existingServiceVariant.IsVisible = false;
                    await context.SaveChangesAsync();
                    var entities = await context.ServiceVariants.Where(s => s.IsVisible == true).ToListAsync();
                    await _cacheService.UpdateCacheAsync(entities);
                    var (responseData2, _) = ServiceVariantConversion.FromEntity(existingServiceVariant, null);
                    return new Response(true, 200, "Service Variant was made as hidden successfully")
                    {
                        Data = new { data = responseData2 }
                    };
                }

                context.Remove(existingServiceVariant);
                await context.SaveChangesAsync();
                var entitiess = await context.ServiceVariants.Where(s => s.IsVisible == true).ToListAsync();
                await _cacheService.UpdateCacheAsync(entitiess);

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

        public async Task<ServiceVariant> FindByKeyAsync(Guid serviceId, Guid breedId, string petWeightRange, bool noTracking = false)
        {
            var query = context.ServiceVariants.Where(s => s.ServiceId == serviceId
                                && s.BreedId == breedId
                                && s.PetWeightRange!.Trim() == petWeightRange.Trim());

            if (noTracking)
            {
                query = query.AsNoTracking();
            }

            return await query.FirstOrDefaultAsync() ?? null!;
        }

        public Task<Response> GetByIdAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<Response> UpdateAsync(Guid id, ServiceVariant entity)
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
                var entities = await context.ServiceVariants.ToListAsync();

                if (entities is null || !entities.Any())
                {
                    return new Response(false, 404, "Không tìm thấy biển thể nào trong database");
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
    }
}
