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
    public class ServiceRepository(ServiceDbContext context) : IService
    {
        public async Task<Response> CreateAsync(Service entity)
        {
            try
            {
                var existingService = await GetByAsync(x => x.ServiceName == entity.ServiceName);

                if (existingService.Data is not null)
                {
                    return new Response(false, 409, "Service already exists");
                }

                var createdService = await context.Services.AddAsync(entity) ?? throw new Exception("Failed to create service");
                await context.SaveChangesAsync();

                var (responseData, _) = ServiceConversion.FromEntity(entity, null);

                return new Response(true, 201, "Service created successfully")
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
                var existingService = await FindServiceById(id);

                if (existingService is null)
                {
                    return new Response(false, 404, $"Can not find service with id {id}");
                }

                var (responseData, _) = ServiceConversion.FromEntity(existingService, null!);

                // TODO: Before delete, check if service is in use by any other entity

                if (existingService.IsVisible == true)
                {
                    existingService.IsVisible = false;
                    await context.SaveChangesAsync();

                    return new Response(true, 200, "Service was marked as hidden successfully")
                    {
                        Data = responseData
                    };
                }

                var deletedService = context.Services.Remove(existingService) ?? throw new Exception("Fail to delete service");
                await context.SaveChangesAsync();

                return new Response(false, 200, $"Service with id {id} was permanently deleted")
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

        public async Task<Response> GetAllAsync()
        {
            try
            {
                var services = await context.Services.AsNoTracking().ToListAsync();

                if (services is null || !services.Any())
                {
                    return new Response(false, 404, "Can not find any service");
                }

                var (_, responseData) = ServiceConversion.FromEntity(null, services);

                return new Response(true, 200, "Service retrived successfully")
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

        public async Task<Response> GetByAsync(Expression<Func<Service, bool>> predicate)
        {
            try
            {
                var service = await context.Services.Where(predicate).AsNoTracking().FirstOrDefaultAsync();

                if (service is null)
                {
                    return new Response(false, 404, "Can not find any service with the provided predicate");
                }

                var (singleService, _) = ServiceConversion.FromEntity(service, null);

                return new Response(true, 200, "Service retrived successfully")
                {
                    Data = singleService
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
                var service = await FindServiceById(id);

                if (service is null)
                {
                    return new Response(false, 404, $"Can not find any service with id {id}");
                }

                var (singleService, _) = ServiceConversion.FromEntity(service, null);

                return new Response(true, 200, "Service retrived successfully")
                {
                    Data = singleService
                };
            }
            catch (Exception ex)
            {
                LogException.LogExceptions(ex);
                return new Response(false, 500, "Internal Server Error");
            }
        }

        public async Task<Response> UpdateAsync(Guid id, Service entity)
        {
            try
            {
                var existingService = await FindServiceById(id);

                if (existingService is null)
                {
                    return new Response(false, 404, $"Can not find service with id {id}");
                }

                var isChanged =
                    existingService.ServiceName != entity.ServiceName
                    || existingService.ServiceDesc != entity.ServiceDesc
                    || existingService.IsVisible != entity.IsVisible
                    || existingService.ServiceTypeId != entity.ServiceTypeId;

                if (isChanged)
                {
                    existingService.ServiceName = entity.ServiceName;
                    existingService.ServiceDesc = entity.ServiceDesc;
                    existingService.IsVisible = entity.IsVisible;
                    existingService.ServiceTypeId = entity.ServiceTypeId;

                    context.Services.Update(existingService);
                    await context.SaveChangesAsync();
                }

                return isChanged ?
                    new Response(true, 200, "Service updated successfully")
                    {
                        Data = existingService
                    } :
                    new Response(false, 204, "No changes made to the service");
            }
            catch (Exception ex)
            {
                LogException.LogExceptions(ex);
                return new Response(false, 500, "Internal Server Error");
            }
        }

        public async Task<Service> FindServiceById(Guid id)
        {
            return await context.Services.FindAsync(id) ?? null!;
        }
    }
}
