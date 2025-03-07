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
    public class PetRepository(PetDbContext _context, IPetCachingService _cacheService) : IPet
    {
        public async Task<Response> CreateAsync(Pet entity)
        {
            var strategy = _context.Database.CreateExecutionStrategy();

            return await strategy.ExecuteAsync(async () =>
            {
                using var transaction = await _context.Database.BeginTransactionAsync();
                try
                {
                    var existingPet = await GetByAsync(x => x.PetName == entity.PetName && x.CustomerId == entity.CustomerId);

                    if (existingPet.Data is not null)
                    {
                        return new Response(false, 409, "Pet already exists");
                    }

                    await _context.Pets.AddAsync(entity);
                    await _context.SaveChangesAsync();

                    var generatedPetId = entity.PetId;

                    var healthBook = new PetHealthBook
                    {
                        PetHealthBookId = generatedPetId,
                        Pet = entity,
                        PetHealthBookDetails = new List<PetHealthBookDetail>()
                    };

                    await _context.PetHealthBooks.AddAsync(healthBook);
                    await _context.SaveChangesAsync();

                    await transaction.CommitAsync();

                    var pets = await _context.Pets.ToListAsync();
                    await _cacheService.UpdateCache(pets);

                    var (responseData, _) = PetConversion.FromEntity(entity, null);

                    return new Response(true, 201, "Pet created successfully!")
                    {
                        Data = responseData
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

                var pets = await _context.Pets.ToListAsync();
                await _cacheService.UpdateCache(pets);

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

        public async Task<Response> GetByUserIdAsync(Guid id, int pageIndex = 1, int pageSize = 10)
        {
            try
            {
                var petCollection = await _context.Pets
                    .Where(x => x.CustomerId == id)
                    .Skip((pageIndex - 1) * pageSize)
                    .Take(pageSize)
                    .Include(p => p.PetImagePaths)
                    .Include(p => p.PetBreed)
                        .ThenInclude(b => b.PetType)
                    .ToListAsync();

                if (petCollection is null || petCollection.Count == 0)
                {
                    return new Response(false, 404, "Không tìm thấy thú cưng nào!");
                }

                var (_, response) = PetConversion.FromEntity(null, petCollection);

                return new Response(true, 200, "Found")
                {
                    Data = new
                    {
                        data = response,
                        meta = new
                        {
                            currentPage = pageIndex,
                            totalPage = Math.Ceiling((double)petCollection.Count / pageSize)
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

        public async Task<Response> GetByIdAsync(Guid id)
        {
            try
            {
                var pet = await FindById(id, true, true);

                if (pet is null || pet.IsVisible == false)
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
                if (entity.PetDateOfBirth > DateTime.UtcNow)
                {
                    return new Response(false, 400, "Ngày sinh không hợp lệ");
                }

                var existingBreed = await _context.PetBreeds.FindAsync(entity.BreedId);

                if (existingBreed is null || existingBreed.IsVisible == false)
                {
                    return new Response(false, 404, "Giống này đã bị xóa hoặc không tồn tại");
                }

                var existingMother = await _context.Pets.FindAsync(entity.MotherId);

                if (existingMother is null || existingMother.IsVisible == false)
                {
                    return new Response(false, 404, "Thú cưng này không tồn tại hoặc đã bị xóa");
                }

                var existingFather = await _context.Pets.FindAsync(entity.FatherId);

                if (existingFather is null || existingFather.IsVisible == false)
                {
                    return new Response(false, 404, "Thú cưng này không tồn tại hoặc đã bị xóa");
                }

                var existingPet = await FindById(id);

                if (existingPet is null)
                {
                    return new Response(false, 404, "Pet not found");
                }

                if
                (
                    existingPet.ChildrenFromMother is not null
                        && existingPet.ChildrenFromMother.Contains(existingMother) ||
                    existingPet.ChildrenFromFather is not null
                        && existingPet.ChildrenFromFather.Contains(existingFather)
                )
                {
                    return new Response(false, 400, "Cha hoặc mẹ của thú cưng này không hợp lệ");
                }

                bool hasChanges = entity.PetName != existingPet.PetName ||
                    entity.PetDateOfBirth != existingPet.PetDateOfBirth ||
                    entity.PetGender != existingPet.PetGender ||
                    entity.PetFurColor != existingPet.PetFurColor ||
                    entity.PetWeight != existingPet.PetWeight ||
                    entity.PetDesc != existingPet.PetDesc ||
                    entity.PetFamilyRole != existingPet.PetFamilyRole ||
                    entity.IsVisible != existingPet.IsVisible ||
                    entity.MotherId != existingPet.MotherId ||
                    entity.FatherId != existingPet.FatherId ||
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
                existingPet.MotherId = entity.MotherId;
                existingPet.FatherId = entity.FatherId;
                existingPet.BreedId = entity.BreedId;
                existingPet.CustomerId = entity.CustomerId;

                await _context.SaveChangesAsync();

                var pets = await _context.Pets.ToListAsync();
                await _cacheService.UpdateCache(pets);

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
                    .Include(p => p.PetBreed)
                    .Include(p => p.PetImagePaths)
                    .Include(p => p.Mother)
                    .Include(p => p.Father)
                    .Include(p => p.ChildrenFromMother)
                    .Include(p => p.ChildrenFromFather)
                    .Include(p => p.PetHealthBook);
            }

            return await query.FirstOrDefaultAsync() ?? null!;
        }
    }
}
