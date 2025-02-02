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
    public class PetBreedRepository(PetDbContext _context) : IPetBreed
    {
        public async Task<Response> CreateAsync(PetBreed entity)
        {
            try
            {
                var existingPetBreed = await GetByAsync(x => x.BreedName == entity.BreedName && x.PetTypeId == entity.PetTypeId);

                if (existingPetBreed.Data is not null)
                {
                    return new Response(false, 409, "Pet Breed already exists");
                }

                await _context.PetBreeds.AddAsync(entity);
                await _context.SaveChangesAsync();

                var (responseData, _) = PetBreedConversion.FromEntity(entity, null);

                return new Response(true, 201, "Pet Breed created successfully!")
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
                var existingPetBreed = await FindById(id);

                if (existingPetBreed is null)
                {
                    return new Response(false, 409, "Pet Breed already exists");
                }

                string message;

                if (existingPetBreed.IsVisible)
                {
                    existingPetBreed.IsVisible = false;
                    message = "Pet Breed was made as hidden successfully";
                }
                else
                {
                    _context.PetBreeds.Remove(existingPetBreed);
                    message = "Pet Breed was permanently deleted successfully";
                }

                await _context.SaveChangesAsync();

                var (responseData, _) = PetBreedConversion.FromEntity(existingPetBreed, null);

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
                var petBreeds = await _context.PetBreeds
                    .Skip((pageIndex - 1) * pageSize)
                    .Take(pageSize)
                    .Include(pb => pb.PetType)
                    .Include(pb => pb.Pets)
                    .ToListAsync();

                if (petBreeds is null || petBreeds.Count == 0)
                {
                    return new Response(false, 404, "No Pet Breeds found");
                }

                var (_, responseData) = PetBreedConversion.FromEntity(null, petBreeds);

                var totalRecords = _context.PetBreeds.Count();

                return new Response(true, 200, "Pet Breeds retrieved successfully!")
                {
                    Data = new
                    {
                        data = responseData,
                        meta = new
                        {
                            currentPage = pageIndex,
                            pageSize,
                            totalPages = Math.Ceiling((double)totalRecords / pageSize),
                            totalRecords
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

        public async Task<Response> GetByAsync(Expression<Func<PetBreed, bool>> predicate)
        {
            try
            {
                var petBreed = await _context.PetBreeds.Where(predicate).FirstOrDefaultAsync();

                if (petBreed is null)
                {
                    return new Response(false, 404, "Pet Breed not found");
                }

                var (responseData, _) = PetBreedConversion.FromEntity(petBreed, null);

                return new Response(true, 200, "Pet Breed retrieved successfully!")
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
                var petBreed = await FindById(id, true, true);

                if (petBreed is null)
                {
                    return new Response(false, 404, "Pet Breed not found");
                }

                var (responseData, _) = PetBreedConversion.FromEntity(petBreed, null);

                return new Response(true, 200, "Pet Breed retrieved successfully!")
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

        public async Task<Response> UpdateAsync(Guid id, PetBreed entity)
        {
            try
            {
                var existingPetBreed = await FindById(id);

                if (existingPetBreed is null)
                {
                    return new Response(false, 409, "Pet Breed already exists");
                }

                bool hasChanges = entity.BreedName != existingPetBreed.BreedName
                    || entity.BreedDesc != existingPetBreed.BreedDesc
                    || entity.IllustrationImage != existingPetBreed.IllustrationImage
                    || entity.IsVisible != existingPetBreed.IsVisible;

                if (!hasChanges)
                {
                    return new Response(false, 204, "No changes were made");
                }

                existingPetBreed.BreedName = entity.BreedName;
                existingPetBreed.BreedDesc = entity.BreedDesc;
                existingPetBreed.IllustrationImage = entity.IllustrationImage;
                existingPetBreed.IsVisible = entity.IsVisible;

                await _context.SaveChangesAsync();

                var (responseData, _) = PetBreedConversion.FromEntity(existingPetBreed, null);

                return new Response(true, 200, "Pet Breed updated successfully!")
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

        public async Task<PetBreed> FindById(Guid id, bool noTracking = false, bool include = false)
        {
            var query = _context.PetBreeds.Where(pb => pb.BreedId == id);

            if (noTracking)
            {
                query = query.AsNoTracking();
            }

            if (include)
            {
                query = query.Include(pb => pb.Pets).Include(pb => pb.PetType);
            }

            return await query.FirstOrDefaultAsync() ?? null!;
        }
    }
}
