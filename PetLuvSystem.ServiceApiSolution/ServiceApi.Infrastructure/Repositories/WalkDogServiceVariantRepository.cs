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
    public class WalkDogServiceVariantRepository(ServiceDbContext context) : IWalkDogServiceVariant
    {
        public async Task<Response> CreateAsync(WalkDogServiceVariant entity)
        {
            try
            {
                var existingVariant = await GetByKeyAsync(entity.ServiceId, entity.BreedId);
                if (existingVariant.Data != null)
                {
                    return new Response(false, 409, "This Service Variant already exists");
                }

                var serviceVariant = await context.WalkDogServiceVariants.AddAsync(entity)
                                            ?? throw new Exception("Failed to create service variant");

                await context.SaveChangesAsync();

                var (responseData, _) = WalkDogServiceVariantConversion.FromEntity(entity, null);

                return new Response(false, 201, "Service Variant created successfully")
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

                context.WalkDogServiceVariants.Remove(existingVariant);
                await context.SaveChangesAsync();

                return new Response(true, 200, "Service Variant deleted successfully")
                {
                    Data = new { data = existingVariant }
                };
            }
            catch (Exception ex)
            {
                LogException.LogExceptions(ex);
                return new Response(false, 500, "Internal Server Error");
            }
        }

        public async Task<WalkDogServiceVariant> FindByKeyAsync(Guid serviceId, Guid breedId)
        {
            return await context.WalkDogServiceVariants.Where(x => x.ServiceId == serviceId && x.BreedId == breedId).FirstOrDefaultAsync() ?? null!;
        }

        public async Task<Response> GetByKeyAsync(Guid serviceId, Guid breedId)
        {
            try
            {
                var serviceVariant = await FindByKeyAsync(serviceId, breedId);

                if (serviceVariant == null)
                {
                    return new Response(false, 404, "Service Variant Not Found");
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

                var (_, responseData) = WalkDogServiceVariantConversion.FromEntity(null, serviceVariants);

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

        public Task<Response> DeleteAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<Response> GetAllAsync(int pageIndex, int pageSize)
        {
            throw new NotImplementedException();
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
