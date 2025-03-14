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
    public class ServiceComboRepository(ServiceDbContext context, IBreedMappingService _breedMappingClient) : IServiceCombo
    {
        public async Task<Response> CreateAsync(ServiceCombo entity, ICollection<Guid> serviceIds)
        {
            var strategy = context.Database.CreateExecutionStrategy();

            return await strategy.ExecuteAsync(async () =>
            {
                using var transaction = await context.Database.BeginTransactionAsync();
                try
                {
                    var existingServiceCombo = await GetByAsync(x => x.ServiceComboName == entity.ServiceComboName);
                    if (existingServiceCombo.Data is not null)
                    {
                        return new Response(false, 409, "Service Combo already exists");
                    }

                    await context.ServiceCombos.AddAsync(entity);
                    await context.SaveChangesAsync();

                    var serviceComboMappings = serviceIds.Select(serviceId => new ServiceComboMapping
                    {
                        ServiceComboId = entity.ServiceComboId,
                        ServiceId = serviceId
                    }).ToList();

                    await context.ServiceComboMappings.AddRangeAsync(serviceComboMappings);
                    await context.SaveChangesAsync();

                    await transaction.CommitAsync();

                    var (responseData, _) = ServiceComboConversion.FromEntity(entity, null!);
                    return new Response(true, 201, "Service Combo created successfully")
                    {
                        Data = new { data = responseData },
                    };
                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync();
                    LogException.LogExceptions(ex);
                    return new Response(false, 500, "Internal Server Error");
                }
            });
        }


        public async Task<Response> DeleteAsync(Guid id)
        {
            try
            {
                var existingServiceCombo = await FindByIdAsync(id);

                if (existingServiceCombo is null)
                {
                    return new Response(false, 404, $"Can not find service combo with id {id}");
                }

                var (responseData, _) = ServiceComboConversion.FromEntity(existingServiceCombo, null!);

                if (existingServiceCombo.IsVisible == true)
                {
                    existingServiceCombo.IsVisible = false;
                    await context.SaveChangesAsync();

                    return new Response(false, 200, "Service Combo is made as hiden successfully")
                    {
                        Data = new
                        {
                            Data = responseData
                        }
                    };
                }

                var response = context.ServiceCombos.Remove(existingServiceCombo) ?? throw new Exception("Fail to delete Service Combo");
                await context.SaveChangesAsync();

                return new Response(true, 200, "Service Combo deleted successfully")
                {
                    Data = new { data = responseData },
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
                var serviceCombos = await context.ServiceCombos
                    .Skip((pageIndex - 1) * pageSize)
                    .Take(pageSize)
                    .ToListAsync();

                if (serviceCombos is null || serviceCombos.Count == 0)
                {
                    return new Response(false, 404, "Service combos not found");
                }

                var totalCount = await context.ServiceCombos.CountAsync();

                var (_, responseData) = ServiceComboConversion.FromEntity(null, serviceCombos);

                return new Response(true, 200, "Service Combos retrieved successfully")
                {
                    Data = new
                    {
                        data = responseData,
                        meta = new
                        {
                            pageIndex,
                            pageSize,
                            totalPage = Math.Ceiling((double)totalCount / pageSize)
                        }
                    },
                };
            }
            catch (Exception ex)
            {
                LogException.LogExceptions(ex);
                return new Response(false, 500, "Internal Server Error");
            }
        }

        public async Task<Response> GetByAsync(Expression<Func<ServiceCombo, bool>> predicate)
        {
            try
            {
                var serviceCombo = await context.ServiceCombos
                    .Where(predicate)
                    .FirstOrDefaultAsync();

                if (serviceCombo is null)
                {
                    return new Response(false, 404, "Can not found any service combo with this predicate");
                }

                var (responseData, _) = ServiceComboConversion.FromEntity(serviceCombo, null!);

                return new Response(true, 200, "Service combo retrieved successfully")
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

        public async Task<Response> GetByIdAsync(Guid id)
        {
            try
            {
                var serviceCombo = await FindByIdAsync(id, true, true);

                if (serviceCombo is null)
                {
                    return new Response(false, 404, "Can not found service combo with this id");
                }

                var breedMapping = await _breedMappingClient.GetBreedMappingAsync();

                var (responseData, _) = ServiceComboConversion.FromEntity(serviceCombo, null!, breedMapping);

                return new Response(true, 200, "Service combo retrieved successfully")
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

        public async Task<Response> UpdateAsync(Guid id, ServiceCombo entity, ICollection<Guid> serviceIds)
        {
            var strategy = context.Database.CreateExecutionStrategy();

            return await strategy.ExecuteAsync(async () =>
            {
                using var transaction = await context.Database.BeginTransactionAsync();
                try
                {
                    var existingServiceCombo = await FindByIdAsync(id, false, true);

                    if (existingServiceCombo is null)
                    {
                        return new Response(false, 404, $"Can not find service combo with id {id}");
                    }

                    bool hasChanges =
                        existingServiceCombo.ServiceComboName != entity.ServiceComboName ||
                        existingServiceCombo.ServiceComboDesc != entity.ServiceComboDesc ||
                        existingServiceCombo.IsVisible != entity.IsVisible;

                    if (hasChanges)
                    {
                        existingServiceCombo.ServiceComboName = entity.ServiceComboName;
                        existingServiceCombo.ServiceComboDesc = entity.ServiceComboDesc;
                        existingServiceCombo.IsVisible = entity.IsVisible;

                        await context.SaveChangesAsync();
                    }

                    var existingMappings = context.ServiceComboMappings
                        .Where(m => m.ServiceComboId == existingServiceCombo.ServiceComboId);
                    context.ServiceComboMappings.RemoveRange(existingMappings);
                    await context.SaveChangesAsync();

                    var serviceComboMappings = serviceIds.Select(serviceId => new ServiceComboMapping
                    {
                        ServiceComboId = existingServiceCombo.ServiceComboId,
                        ServiceId = serviceId
                    }).ToList();

                    if (serviceComboMappings.Any())
                    {
                        await context.ServiceComboMappings.AddRangeAsync(serviceComboMappings);
                        await context.SaveChangesAsync();
                    }

                    await transaction.CommitAsync();

                    var (responseData, _) = ServiceComboConversion.FromEntity(existingServiceCombo, null!);

                    return new Response(true, 200, "Service Combo updated successfully")
                    {
                        Data = new { data = responseData }
                    };
                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync();
                    LogException.LogExceptions(ex);
                    return new Response(false, 500, "Internal Server Error");
                }
            });
        }


        public async Task<ServiceCombo> FindByIdAsync(Guid id, bool noTracking = false, bool includeNavigation = false)
        {
            var query = context.ServiceCombos.Where(x => x.ServiceComboId == id);

            if (noTracking)
            {
                query = query.AsNoTracking();
            }

            if (includeNavigation)
            {
                query = query
                    .Include(x => x.ServiceComboMappings)
                        .ThenInclude(x => x.Service)
                    .Include(x => x.ServiceComboVariants);
            }

            return await query.FirstOrDefaultAsync() ?? null!;
        }

        public Task<Response> CreateAsync(ServiceCombo entity)
        {
            throw new NotImplementedException();
        }

        public Task<Response> UpdateAsync(Guid id, ServiceCombo entity)
        {
            throw new NotImplementedException();
        }
    }
}
