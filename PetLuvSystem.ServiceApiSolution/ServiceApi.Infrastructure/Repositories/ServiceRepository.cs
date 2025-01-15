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
                    Data = new { data = responseData }
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
                        Data = new { data = responseData }
                    };
                }

                var deletedService = context.Services.Remove(existingService) ?? throw new Exception("Fail to delete service");
                await context.SaveChangesAsync();

                return new Response(false, 200, $"Service with id {id} was permanently deleted")
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

        public async Task<Response> GetAllAsync(int pageIndex, int pageSize)
        {
            try
            {
                var query = context.Services
                    .AsNoTracking()
                    .Include(s => s.ServiceType)
                    .Include(s => s.ServiceImages);

                var totalCount = await query.CountAsync();

                var services = await query
                    .Skip((pageIndex - 1) * pageSize)
                    .Take(pageSize)
                    .ToListAsync();

                if (services is null || services.Count == 0)
                {
                    return new Response(false, 404, "Can not find any service");
                }

                var (_, responseData) = ServiceConversion.FromEntity(null, services);

                return new Response(true, 200, "Service retrived successfully")
                {
                    Data = new
                    {
                        data = responseData,
                        meta = new
                        {
                            totalPage = Math.Ceiling((double)totalCount / pageSize),
                            pageIndex,
                            pageSize
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

        public async Task<Response> GetByAsync(Expression<Func<Service, bool>> predicate)
        {
            try
            {
                var service = await context.Services
                    .Where(predicate)
                    .AsNoTracking()
                    .Include(s => s.ServiceType)
                    .Include(s => s.ServiceImages)
                    .Include(s => s.ServiceVariants)
                    .Include(s => s.WalkDogServiceVariants)
                    .FirstOrDefaultAsync();

                if (service is null)
                {
                    return new Response(false, 404, "Can not find any service with the provided predicate");
                }

                var (singleService, _) = ServiceConversion.FromEntity(service, null);

                return new Response(true, 200, "Service retrived successfully")
                {
                    Data = new { data = singleService }
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

                context.Entry(service).State = EntityState.Detached;

                var (singleService, _) = ServiceConversion.FromEntity(service, null);

                return new Response(true, 200, "Service retrived successfully")
                {
                    Data = new { data = singleService }
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

                bool hasChanges = false;

                if (entity.ServiceName is not null && existingService.ServiceName != entity.ServiceName)
                {
                    existingService.ServiceName = entity.ServiceName;
                    hasChanges = true;
                }

                if (entity.ServiceDesc is not null && existingService.ServiceDesc != entity.ServiceDesc)
                {
                    existingService.ServiceDesc = entity.ServiceDesc;
                    hasChanges = true;
                }

                if (existingService.IsVisible != entity.IsVisible)
                {
                    existingService.IsVisible = entity.IsVisible;
                    hasChanges = true;
                }

                if (entity.ServiceTypeId != Guid.Empty && existingService.ServiceTypeId != entity.ServiceTypeId)
                {
                    existingService.ServiceTypeId = entity.ServiceTypeId;
                    hasChanges = true;
                }

                if (hasChanges)
                {
                    await context.SaveChangesAsync();
                }

                var (resposneData, _) = ServiceConversion.FromEntity(existingService, null);

                return hasChanges ?
                    new Response(true, 200, "Service updated successfully")
                    {
                        Data = new { data = resposneData }
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
            return await context.Services
                .Include(s => s.ServiceType)
                .Include(s => s.ServiceImages)
                .Include(s => s.ServiceVariants)
                .Include(s => s.WalkDogServiceVariants)
                .FirstOrDefaultAsync(s => s.ServiceId == id)
                ?? null!;
        }
    }
}
