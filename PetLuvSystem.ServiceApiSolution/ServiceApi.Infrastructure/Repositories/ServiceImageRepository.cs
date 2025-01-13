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
    public class ServiceImageRepository(ServiceDbContext context) : IServiceImage
    {
        public async Task<Response> CreateAsync(ServiceImage entity)
        {
            try
            {
                var serviceImage = await context.ServiceImages.AddAsync(entity) ?? throw new Exception("Failed to create service type");
                await context.SaveChangesAsync();

                var (singleServiceImage, _) = ServiceImageConversion.FromEntity(entity, null);

                return new Response(true, 201, "Service image created successfully")
                {
                    Data = singleServiceImage
                };
            }
            catch (Exception ex)
            {
                LogException.LogExceptions(ex);

                return new Response(false, 500, "Internal Server Error");
            }
        }

        public async Task<Response> DeleteAsync(string serviceImagePath)
        {
            try
            {
                var existingServiceImage = await FindByPathAsync(serviceImagePath);

                if (existingServiceImage is null)
                {
                    return new Response(false, 404, $"Can not find service image with path {serviceImagePath}");
                }

                var serviceImage = context.ServiceImages.Remove(existingServiceImage) ?? throw new Exception("Failed to create service type");
                await context.SaveChangesAsync();

                var (singleServiceImage, _) = ServiceImageConversion.FromEntity(existingServiceImage, null);

                return new Response(true, 200, "Service type was deleted permanently")
                {
                    Data = singleServiceImage
                };
            }
            catch (Exception ex)
            {
                LogException.LogExceptions(ex);

                return new Response(false, 500, "Internal Server Error");
            }
        }

        public async Task<Response> GetByAsync(Expression<Func<ServiceImage, bool>> predicate)
        {
            try
            {
                var serviceImage = await context.ServiceImages.FirstOrDefaultAsync(predicate);

                if (serviceImage is null)
                {
                    return new Response(false, 404, "Can not find any service image with the provided predicate");
                }

                var (singleServiceImage, _) = ServiceImageConversion.FromEntity(serviceImage, null);

                return new Response(true, 200, "Service image retrived successfully")
                {
                    Data = singleServiceImage
                };
            }
            catch (Exception ex)
            {
                LogException.LogExceptions(ex);
                return new Response(false, 500, "Internal Server Error");
            }
        }

        public async Task<Response> GetServiceImageByServiceId(Guid serviceId)
        {
            try
            {
                var serviceImages = await context.ServiceImages.Where(x => x.ServiceId == serviceId).ToListAsync();

                if (serviceImages is null || !serviceImages.Any())
                {
                    return new Response(false, 404, $"Can not find any image with service id {serviceId}");
                }

                var (_, responseData) = ServiceImageConversion.FromEntity(null, serviceImages);

                return new Response(true, 200, "Service image retrived successfully")
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

        public Task<Response> DeleteAsync(Guid id)
        {
            throw new NotImplementedException();
        }
        public Task<Response> GetAllAsync(int pageIndex, int pageSize)
        {
            throw new NotImplementedException();
        }

        public Task<Response> GetByIdAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<Response> UpdateAsync(Guid id, ServiceImage entity)
        {
            throw new NotImplementedException();
        }

        public async Task<ServiceImage> FindByPathAsync(string serviceImagerPath)
        {
            return await context.ServiceImages.FindAsync(serviceImagerPath) ?? null!;
        }
    }
}
