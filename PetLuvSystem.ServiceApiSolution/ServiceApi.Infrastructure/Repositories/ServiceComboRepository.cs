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
    public class ServiceComboRepository(ServiceDbContext context) : IServiceCombo
    {
        public async Task<Response> CreateAsync(ServiceCombo entity)
        {
            try
            {
                var existingServiceCombo = await GetByAsync(x => x.ServiceComboName == entity.ServiceComboName);

                if (existingServiceCombo.Data is not null)
                {
                    return new Response(false, 409, "Service Combo already exists");
                }

                var response = await context.ServiceCombos.AddAsync(entity) ?? throw new Exception("Fail to create Service Combo");
                await context.SaveChangesAsync();

                var (responseData, _) = ServiceComboConversion.FromEntity(entity, null!);

                return new Response(true, 201, "Service Combo created successfully")
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
                var serviceCombo = await FindByIdAsync(id);

                if (serviceCombo is null)
                {
                    return new Response(false, 404, "Can not found service combo with this id");
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

        public async Task<Response> UpdateAsync(Guid id, ServiceCombo entity)
        {
            try
            {
                var existingServiceCombo = await FindByIdAsync(id, false);

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

                var (responseData, _) = ServiceComboConversion.FromEntity(existingServiceCombo, null!);

                return hasChanges ?
                    new Response(true, 200, "Service Combo updated successfully")
                    {
                        Data = new { data = responseData }
                    } :
                    new Response(false, 204, "No changes made");
            }
            catch (Exception ex)
            {
                LogException.LogExceptions(ex);
                return new Response(false, 500, "Internal Server Error");
            }
        }

        public async Task<ServiceCombo> FindByIdAsync(Guid id, bool noTracking = false, bool includeNavigation = false)
        {
            var query = context.ServiceCombos.AsQueryable();

            if (noTracking)
            {
                query = query.AsNoTracking();
            }

            if (includeNavigation)
            {
                query.Include(x => x.ServiceComboMappings).Include(x => x.ServiceComboVariants);
            }

            return await query.FirstOrDefaultAsync(x => x.ServiceComboId == id) ?? null!;
        }
    }
}
