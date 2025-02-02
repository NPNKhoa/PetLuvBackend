using Microsoft.EntityFrameworkCore;
using PetApi.Application.DTOs.Conversions;
using PetApi.Application.Interfaces;
using PetApi.Domain.Entities;
using PetApi.Infrastructure.Data;
using PetLuvSystem.SharedLibrary.Logs;
using PetLuvSystem.SharedLibrary.Responses;
using System.Linq.Expressions;

namespace PetApi.Infrastructure.Repositories
{
    public class PetTypeRepository(PetDbContext _context) : IPetType
    {
        public async Task<Response> CreateAsync(PetType entity)
        {
            try
            {
                var existingPetType = await _context.PetTypes
                    .FirstOrDefaultAsync(pt => pt.PetTypeName == entity.PetTypeName);

                if (existingPetType is not null)
                {
                    return new Response(false, 409, "Pet Type Already Exists");
                }

                var petType = await _context.PetTypes.AddAsync(entity) ?? throw new Exception("Fail to create Pet Type");
                await _context.SaveChangesAsync();

                var (responseData, _) = PetTypeConversion.FromEntity(entity, null);

                return new Response(true, 201, "Pet Type Created")
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
                var existingPetType = await FindById(id);

                if (existingPetType is null)
                {
                    return new Response(false, 404, "Pet Type Not Found");
                }

                string message;

                if (existingPetType.IsVisible)
                {
                    existingPetType.IsVisible = false;
                    message = "Pet Type was made as hidden successfully";
                }
                else
                {
                    _context.PetTypes.Remove(existingPetType);
                    message = "Pet Type was permanently deleted successfully";
                }

                await _context.SaveChangesAsync();

                var (responseData, _) = PetTypeConversion.FromEntity(existingPetType, null);

                return new Response(true, 200, message)
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


        public async Task<Response> GetAllAsync(int pageIndex = 1, int pageSize = 10)
        {
            try
            {
                var petTypes = await _context.PetTypes
                    .Skip((pageIndex - 1) * pageSize)
                    .Take(pageSize)
                    .ToListAsync();

                if (petTypes is null || petTypes.Count == 0)
                {
                    return new Response(false, 404, "No Pet Types Found");
                }

                var totalPages = Math.Ceiling((double)_context.PetTypes.Count() / pageSize);

                var (_, responseData) = PetTypeConversion.FromEntity(null, petTypes);

                return new Response(true, 200, "Pet Types Found")
                {
                    Data = new
                    {
                        data = responseData,
                        meta = new
                        {
                            pageIndex,
                            pageSize,
                            totalPages,
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

        public async Task<Response> GetByAsync(Expression<Func<PetType, bool>> predicate)
        {
            try
            {
                var petType = await _context.PetTypes.Where(predicate).FirstOrDefaultAsync();

                if (petType is null)
                {
                    return new Response(false, 404, "Pet Type Not Found");
                }

                var (responseData, _) = PetTypeConversion.FromEntity(petType, null);

                return new Response(true, 200, "Pet Type Found")
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

        public async Task<Response> GetByIdAsync(Guid id)
        {
            try
            {
                var petType = await FindById(id, true, true);

                if (petType is null)
                {
                    return new Response(false, 404, "Pet Type Not Found");
                }

                var (responseData, _) = PetTypeConversion.FromEntity(petType, null);

                return new Response(true, 200, "Pet Type Found")
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

        public async Task<Response> UpdateAsync(Guid id, PetType entity)
        {
            try
            {
                var existingPetType = await FindById(id);

                if (existingPetType is null)
                {
                    return new Response(false, 404, "Pet Type Not Found");
                }

                bool hasChanges =
                    existingPetType.PetTypeName != entity.PetTypeName ||
                    existingPetType.PetTypeDesc != entity.PetTypeDesc ||
                    existingPetType.IsVisible != entity.IsVisible;

                if (!hasChanges)
                {
                    return new Response(false, 204, "No changes were made");
                }

                existingPetType.PetTypeName = entity.PetTypeName;
                existingPetType.PetTypeDesc = entity.PetTypeDesc;
                existingPetType.IsVisible = entity.IsVisible;

                await _context.SaveChangesAsync();

                var (responseData, _) = PetTypeConversion.FromEntity(existingPetType, null);

                return new Response(true, 200, "Pet Type Updated")
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

        public async Task<PetType> FindById(Guid id, bool noTracking = false, bool includeBreed = false)
        {
            var query = _context.PetTypes.Where(pt => pt.PetTypeId == id);

            if (noTracking)
            {
                query = query.AsNoTracking();
            }

            if (includeBreed)
            {
                query = query.Include(pt => pt.PetBreeds);
            }

            return await query.FirstOrDefaultAsync() ?? null!;
        }
    }
}
