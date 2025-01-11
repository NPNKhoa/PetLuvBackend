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
    public class ServiceTypeRepository(ServiceDbContext context) : IServiceType
    {
        public async Task<Response> CreateAsync(ServiceType entity)
        {
            try
            {
                var existingServiceType = await GetByAsync(x => x.ServiceTypeName!.Equals(entity.ServiceTypeName));

                Console.WriteLine(existingServiceType);

                if (existingServiceType.Data is not null)
                {
                    return new Response(false, 409, $"A service type with this name {entity.ServiceTypeName} already exist");
                }

                var serviceType = await context.ServiceTypes.AddAsync(entity) ?? throw new Exception("Failed to create service type");
                await context.SaveChangesAsync();

                var (singleServiceType, _) = ServiceTypeConversion.FromEntity(entity, null);

                return new Response(true, 201, "Service type created successfully")
                {
                    Data = singleServiceType
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
                var existingServiceType = await FindByIdAsync(id);

                if (existingServiceType is null)
                {
                    return new Response(false, 404, $"Can not find service type with id {id}");
                }

                if (existingServiceType.IsVisible)
                {
                    existingServiceType.IsVisible = false;
                    await context.SaveChangesAsync();
                    return new Response(true, 200, "Service type deleted successfully")
                    {
                        Data = existingServiceType
                    };
                }

                var serviceType = context.ServiceTypes.Remove(existingServiceType) ?? throw new Exception("Failed to create service type");
                await context.SaveChangesAsync();

                var (singleServiceType, _) = ServiceTypeConversion.FromEntity(existingServiceType, null);

                return new Response(true, 200, "Service type was deleted permanently")
                {
                    Data = singleServiceType
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
                var serviceTypes = await context.ServiceTypes.AsNoTracking().ToListAsync();

                return serviceTypes is not null && serviceTypes.Any() ?
                    new Response(true, 200, "Service types retrived succesfully") { Data = serviceTypes } :
                    new Response(false, 404, "No service type found");
            }
            catch (Exception ex)
            {
                LogException.LogExceptions(ex);

                throw new Exception("Internal Server Error");
            }
        }

        public async Task<Response> GetByAsync(Expression<Func<ServiceType, bool>> predicate)
        {
            try
            {
                var serviceType = await context.ServiceTypes.Where(predicate).AsNoTracking().FirstOrDefaultAsync();

                return serviceType is not null ?
                    new Response(true, 200, "Service type retrived succesfully") { Data = serviceType } :
                    new Response(false, 404, "No service type found");
            }
            catch (Exception ex)
            {
                LogException.LogExceptions(ex);
                throw new Exception("Internal Server Error");
            }
        }

        public async Task<Response> GetByIdAsync(Guid id)
        {
            try
            {
                var existingServiceType = await context.ServiceTypes.FindAsync(id);

                return existingServiceType is not null ?
                    new Response(true, 200, "Service type retrived succesfully") { Data = existingServiceType } :
                    new Response(false, 404, "No service type found");
            }
            catch (Exception ex)
            {
                LogException.LogExceptions(ex);

                throw new Exception("Internal Server Error");
            }
        }

        public async Task<Response> UpdateAsync(Guid id, ServiceType entity)
        {
            try
            {
                var existingServiceType = await FindByIdAsync(id);

                if (existingServiceType is null)
                {
                    return new Response(false, 404, $"Can not find service type with id {id}");
                }

                var isChanged =
                    existingServiceType.ServiceTypeName != entity.ServiceTypeName
                    || existingServiceType.ServiceTypeDesc != entity.ServiceTypeDesc
                    || existingServiceType.IsVisible != entity.IsVisible;

                if (isChanged)
                {
                    existingServiceType.ServiceTypeName = entity.ServiceTypeName;
                    existingServiceType.ServiceTypeDesc = entity.ServiceTypeDesc;
                    existingServiceType.IsVisible = entity.IsVisible;

                    context.ServiceTypes.Update(existingServiceType);
                    await context.SaveChangesAsync();
                }

                return isChanged ?
                    new Response(true, 200, "Service type updated successfully")
                    {
                        Data = existingServiceType
                    } : new Response(true, 204, "No changes detected. No update needed");
            }
            catch (Exception ex)
            {
                LogException.LogExceptions(ex);

                return new Response(false, 500, "Internal Server Error");
            }
        }

        public async Task<ServiceType> FindByIdAsync(Guid id)
        {
            return await context.ServiceTypes.FindAsync(id) ?? null!;
        }
    }
}
