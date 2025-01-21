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
    public class ServiceComboMappingRepository(ServiceDbContext _context) : IServiceComboMapping
    {
        public async Task<Response> CreateAsync(ServiceComboMapping entity)
        {
            try
            {
                var existingEntity = await FindByKeyAsync(entity.ServiceId, entity.ServiceComboId, true);

                if (existingEntity is not null)
                {
                    return new Response(false, 409, "Service Combo Mapping already exists");
                }

                await _context.ServiceComboMappings.AddAsync(entity);
                await _context.SaveChangesAsync();

                var (responseData, _) = ServiceComboMappingConversion.FromEntity(entity, null);

                return new Response(true, 201, "Service Combo Mapping created successfully")
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

        public async Task<Response> DeleteAsync(Guid serviceId, Guid serviceComboId)
        {
            try
            {
                var entity = await FindByKeyAsync(serviceId, serviceComboId);

                if (entity is null)
                {
                    return new Response(false, 404, "Service Combo Mapping not found");
                }

                _context.ServiceComboMappings.Remove(entity);
                await _context.SaveChangesAsync();

                var (responseData, _) = ServiceComboMappingConversion.FromEntity(entity, null);

                return new Response(true, 200, "Service Combo Mapping deleted successfully")
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
                var entities = await _context.ServiceComboMappings.AsNoTracking().Skip(pageIndex * pageSize).Take(pageSize).ToListAsync();

                if (entities is null || entities.Count == 0)
                {
                    return new Response(false, 404, "Service Combo Mappings not found");
                }

                var (_, responseData) = ServiceComboMappingConversion.FromEntity(null, entities);

                return new Response(true, 200, "Service Combo Mappings found")
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

        public async Task<Response> GetByAsync(Expression<Func<ServiceComboMapping, bool>> predicate)
        {
            try
            {
                var entity = await _context.ServiceComboMappings.AsNoTracking().FirstOrDefaultAsync(predicate);

                if (entity is null)
                {
                    return new Response(false, 404, "Service Combo Mapping not found");
                }

                var (responseData, _) = ServiceComboMappingConversion.FromEntity(entity, null);

                return new Response(true, 200, "Service Combo Mapping found")
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

        public async Task<Response> GetServicesByCombo(Guid serviceComboId)
        {
            try
            {
                var serviceCombos = await _context.ServiceComboMappings
                    .Where(e => e.ServiceComboId == serviceComboId)
                    .AsNoTracking()
                    .Include(s => s.Service)
                    .Select(mapping => new ServiceComboMapping
                    {
                        ServiceId = mapping.ServiceId,
                        ServiceComboId = mapping.ServiceComboId,
                        Service = new Service
                        {
                            ServiceId = mapping.Service!.ServiceId,
                            ServiceName = mapping.Service.ServiceName,
                        }
                    })
                    .ToListAsync();


                if (serviceCombos is null || serviceCombos.Count == 0)
                {
                    return new Response(false, 404, "Can not found any service in this combo");
                }

                var (_, responseData) = ServiceComboMappingConversion.FromEntity(null, serviceCombos);

                return new Response(true, 200, "Service Combo Mappings found")
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

        public async Task<ServiceComboMapping> FindByKeyAsync(Guid serviceId, Guid serviceComboId, bool noTracking = false)
        {
            var query = _context.ServiceComboMappings.Where(e => e.ServiceId == serviceId && e.ServiceComboId == serviceComboId);

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

        public Task<Response> UpdateAsync(Guid id, ServiceComboMapping entity)
        {
            throw new NotImplementedException();
        }

        public Task<Response> DeleteAsync(Guid id)
        {
            throw new NotImplementedException();
        }
    }
}
