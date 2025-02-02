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
    public class PetRepository(PetDbContext _context) : IPet
    {
        public async Task<Response> CreateAsync(Pet entity)
        {
            try
            {
                var existingPet = await GetByAsync(x => x.PetName == entity.PetName && x.CustomerId == entity.CustomerId);

                if (existingPet.Data is not null)
                {
                    return new Response(false, 409, "Pet already exists");
                }

                await _context.Pets.AddAsync(entity);
                await _context.SaveChangesAsync();

                var (responseData, _) = PetConversion.FromEntity(entity, null);

                return new Response(true, 201, "Pet created successfully!")
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
                var existingPet = await FindById(id);

                if (existingPet is null)
                {
                    return new Response(false, 404, "Pet not found");
                }

                string message;

                if (existingPet.IsVisible)
                {
                    existingPet.IsVisible = false;
                    message = "Pet was made as hidden successfully";
                }
                else
                {
                    _context.Pets.Remove(existingPet);
                    message = "Pet was permanently deleted successfully";
                }

                await _context.SaveChangesAsync();

                return new Response(true, 200, message);
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
                var pets = await _context.Pets
                    .Skip((pageIndex - 1) * pageSize)
                    .Take(pageSize)
                    .Include(p => p.PetBreed)
                    .Include(p => p.PetImagePaths)
                    .Include(p => p.ParentPet)
                    .Include(p => p.ChildrenPets)
                    .Include(p => p.PetHealthBooks)
                    .ToListAsync();

                if (pets is null || pets.Count == 0)
                {
                    return new Response(false, 404, "No pets found");
                }

                var totalRecords = await _context.Pets.CountAsync();

                var (_, responseData) = PetConversion.FromEntity(null, pets);

                return new Response(true, 200, "Pets found")
                {
                    Data = new
                    {
                        data = responseData,
                        meta = new
                        {
                            currentPage = pageIndex,
                            totalPage = Math.Ceiling((double)totalRecords / pageSize),
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

        public async Task<Response> GetByAsync(Expression<Func<Pet, bool>> predicate)
        {
            try
            {
                var pet = await _context.Pets.FirstOrDefaultAsync(predicate);

                if (pet is null)
                {
                    return new Response(false, 404, "Pet not found");
                }

                var (responseData, _) = PetConversion.FromEntity(pet, null);

                return new Response(true, 200, "Pet found")
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
                var pet = await FindById(id, true, true);

                if (pet is null)
                {
                    return new Response(false, 404, "Pet not found");
                }

                var (responseData, _) = PetConversion.FromEntity(pet, null);

                return new Response(true, 200, "Pet found")
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

        public async Task<Response> UpdateAsync(Guid id, Pet entity)
        {
            try
            {
                var existingPet = await FindById(id);

                if (existingPet is null)
                {
                    return new Response(false, 404, "Pet not found");
                }

                bool hasChanges = entity.PetName != existingPet.PetName ||
                    entity.PetDateOfBirth != existingPet.PetDateOfBirth ||
                    entity.PetGender != existingPet.PetGender ||
                    entity.PetFurColor != existingPet.PetFurColor ||
                    entity.PetWeight != existingPet.PetWeight ||
                    entity.PetDesc != existingPet.PetDesc ||
                    entity.PetFamilyRole != existingPet.PetFamilyRole ||
                    entity.IsVisible != existingPet.IsVisible ||
                    entity.ParentPetId != existingPet.ParentPetId ||
                    entity.BreedId != existingPet.BreedId ||
                    entity.CustomerId != existingPet.CustomerId;

                if (!hasChanges)
                {
                    return new Response(false, 204, "No changes found");
                }

                existingPet.PetName = entity.PetName;
                existingPet.PetDateOfBirth = entity.PetDateOfBirth;
                existingPet.PetGender = entity.PetGender;
                existingPet.PetFurColor = entity.PetFurColor;
                existingPet.PetWeight = entity.PetWeight;
                existingPet.PetDesc = entity.PetDesc;
                existingPet.PetFamilyRole = entity.PetFamilyRole;
                existingPet.IsVisible = entity.IsVisible;
                existingPet.ParentPetId = entity.ParentPetId;
                existingPet.BreedId = entity.BreedId;
                existingPet.CustomerId = entity.CustomerId;

                await _context.SaveChangesAsync();

                var (responseData, _) = PetConversion.FromEntity(existingPet, null);

                return new Response(true, 200, "Pet updated successfully")
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

        public async Task<Pet> FindById(Guid id, bool noTracking = false, bool includeOthers = false)
        {
            var query = _context.Pets.Where(x => x.PetId == id);

            if (noTracking)
            {
                query = query.AsNoTracking();
            }

            if (includeOthers)
            {
                query = query
                    .Include(x => x.ParentPet)
                    .Include(x => x.PetBreed)
                    .Include(x => x.PetImagePaths)
                    .Include(x => x.ChildrenPets)
                    .Include(x => x.PetHealthBooks);
            }

            return await query.FirstOrDefaultAsync() ?? null!;
        }
    }
}
