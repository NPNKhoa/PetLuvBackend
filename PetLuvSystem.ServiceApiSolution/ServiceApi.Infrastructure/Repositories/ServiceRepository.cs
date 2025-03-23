using Microsoft.EntityFrameworkCore;
using PetLuvSystem.SharedLibrary.Logs;
using PetLuvSystem.SharedLibrary.Responses;
using ServiceApi.Application.DTOs.Conversions;
using ServiceApi.Application.Interfaces;
using ServiceApi.Domain.Entities;
using ServiceApi.Infrastructure.Data;
using System.Globalization;
using System.Linq.Expressions;

namespace ServiceApi.Infrastructure.Repositories
{
    public class ServiceRepository(ServiceDbContext context, IBreedMappingService _breedMappingClient, IServiceMappingCaching _serviceMappingCache) : IService
    {
        public async Task<Response> CreateAsync(Service entity)
        {
            try
            {
                var existingService = await GetByAsync(x => x.ServiceName == entity.ServiceName && x.ServiceTypeId == entity.ServiceTypeId);

                if (existingService.Data is not null)
                {
                    return new Response(false, 409, "Service already exists");
                }

                var createdService = await context.Services.AddAsync(entity) ?? throw new Exception("Failed to create service");
                await context.SaveChangesAsync();

                var services = await context.Services.ToListAsync();
                _serviceMappingCache.UpdateCacheAsync(services);

                var (responseData, _) = ServiceConversion.FromEntity(entity, null);

                return new Response(true, 201, "Service created successfully")
                {
                    Data = new { data = responseData }
                };
            }
            catch (Exception ex)
            {
                if (ex.InnerException != null)
                {
                    LogException.LogError($"Inner Exception: {ex.InnerException.Message}");
                }
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


                // TODO: Before delete, check if service is in use by any other entity

                if (existingService.IsVisible == true)
                {
                    existingService.IsVisible = false;
                    await context.SaveChangesAsync();

                    var (responseData, _) = ServiceConversion.FromEntity(existingService, null!);

                    return new Response(true, 200, "Service was marked as hidden successfully")
                    {
                        Data = new { data = responseData }
                    };
                }

                var deletedService = context.Services.Remove(existingService) ?? throw new Exception("Fail to delete service");
                await context.SaveChangesAsync();

                var services = await context.Services.ToListAsync();
                _serviceMappingCache.UpdateCacheAsync(services);

                var (response, _) = ServiceConversion.FromEntity(existingService, null!);

                return new Response(false, 200, $"Service with id {id} was permanently deleted")
                {
                    Data = new { data = response }
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
                    .Include(s => s.ServiceVariants)
                    .Include(s => s.ServiceImages);

                var totalCount = await query.CountAsync();

                var services = await query
                    .Skip((pageIndex - 1) * pageSize)
                    .Take(pageSize)
                    .ToListAsync();

                if (services is null || services.Count == 0)
                {
                    return new Response(false, 404, "Can not find any s ervice");
                }

                var breedMapping = await _breedMappingClient.GetBreedMappingAsync();

                //var services = await context.Services.ToListAsync();
                //_serviceMappingCache.UpdateCacheAsync(services);

                var (_, responseData) = ServiceConversion.FromEntity(null, services, breedMapping);

                return new Response(true, 200, "Service retrieved successfully")
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

        public async Task<Response> GetAppropriateServices(Guid? breedId, string? petWeight)
        {
            try
            {
                var query = context.Services
                    .AsNoTracking()
                    .Include(s => s.ServiceType)
                    .Include(s => s.ServiceVariants)
                    .Include(s => s.ServiceImages)
                    .Include(s => s.WalkDogServiceVariants)
                    .Where(s => s.IsVisible);

                if (breedId.HasValue)
                {
                    query = query.Where(s =>
                        s.ServiceVariants!.Any(v => v.BreedId == breedId.Value) ||
                        s.WalkDogServiceVariants!.Any(w => w.BreedId == breedId.Value)
                    );
                }

                var services = await query.ToListAsync();

                if (!string.IsNullOrEmpty(petWeight))
                {
                    services = services.Where(s =>
                        s.ServiceVariants!.Any()
                            ? s.ServiceVariants!.Any(v => IsWeightInRange(v.PetWeightRange, petWeight))
                            : s.WalkDogServiceVariants!.Any()
                    ).ToList();
                }

                if (!services.Any())
                {
                    return new Response(false, 404, "No appropriate services found.");
                }

                var breedMapping = await _breedMappingClient.GetBreedMappingAsync();
                var (_, responseData) = ServiceConversion.FromEntity(null, services, breedMapping);

                return new Response(true, 200, "Services retrieved successfully")
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
                    return new Response(false, 404, "Không tìm thấy dịch vụ theo yêu cầu");
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

        public async Task<Response> GetByAsync(Expression<Func<Service, bool>> predicate, bool isReturnList = false)
        {
            try
            {
                var services = await context.Services
                    .Where(predicate)
                    .AsNoTracking()
                    .Include(s => s.ServiceType)
                    .Include(s => s.ServiceImages)
                    .Include(s => s.ServiceVariants)
                    .Include(s => s.WalkDogServiceVariants)
                    .ToListAsync();

                if (services is null || services.Count == 0)
                {
                    return new Response(false, 404, "Không tìm thấy dịch vụ theo yêu cầu");
                }

                var (_, response) = ServiceConversion.FromEntity(null, services);

                return new Response(true, 200, "Service retrived successfully")
                {
                    Data = new { data = response }
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
                var service = await FindServiceById(id, true, true);

                if (service is null || service.IsVisible == false)
                {
                    return new Response(false, 404, "Dịch vụ này đã bị xóa hoặc không tồn tại");
                }

                var breedMapping = await _breedMappingClient.GetBreedMappingAsync();

                var (singleService, _) = ServiceConversion.FromEntity(service, null, breedMapping);

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

                if (existingService.IsVisible.ToString() != entity.IsVisible.ToString())
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

                var services = await context.Services.ToListAsync();
                _serviceMappingCache.UpdateCacheAsync(services);

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

        public async Task<Service> FindServiceById(Guid id, bool noTracking = false, bool includeOthers = false)
        {
            var query = context.Services.Where(s => s.ServiceId == id);

            if (noTracking)
            {
                query = query.AsNoTracking();
            }

            if (includeOthers)
            {
                query = query.Include(s => s.ServiceType)
                    .Include(s => s.ServiceImages)
                    .Include(s => s.ServiceVariants)
                    .Include(s => s.WalkDogServiceVariants);
            }

            return await query.FirstOrDefaultAsync() ?? null!;
        }

        private bool IsWeightInRange(string? weightRange, string petWeight)
        {
            if (string.IsNullOrWhiteSpace(weightRange) || string.IsNullOrWhiteSpace(petWeight))
                return false;

            var rangeParts = weightRange.Split('-')
                .Select(p => p.Trim().Replace("kg", "").Replace(" ", ""))
                .ToList();

            if (rangeParts.Count != 2 ||
                !decimal.TryParse(rangeParts[0], NumberStyles.Any, CultureInfo.InvariantCulture, out var minWeight) ||
                !decimal.TryParse(rangeParts[1], NumberStyles.Any, CultureInfo.InvariantCulture, out var maxWeight))
            {
                return false;
            }

            if (!decimal.TryParse(petWeight.Replace("kg", "").Replace(" ", ""), NumberStyles.Any, CultureInfo.InvariantCulture, out var petWeightValue))
            {
                return false;
            }

            return petWeightValue >= minWeight && petWeightValue <= maxWeight;
        }

    }
}
