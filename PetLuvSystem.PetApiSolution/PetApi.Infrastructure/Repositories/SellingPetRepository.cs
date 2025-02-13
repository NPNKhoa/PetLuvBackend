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
    public class SellingPetRepository(PetDbContext _context) : ISellingPet
    {
        public Task<Response> CreateAsync(SellingPet entity)
        {
            throw new NotImplementedException();
        }

        public Task<Response> DeleteAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public async Task<Response> GetAllAsync(int pageIndex = 1, int pageSize = 10)
        {
            try
            {

                var sellingPets = await _context.SellingPets
                    .Skip(pageSize * (pageIndex - 1))
                    .Take(pageSize)
                    .ToListAsync();

                if (sellingPets is null || sellingPets.Count == 0)
                {
                    return new Response(false, 404, "Không tìm thấy thú cưng nào");
                }

                var totalRecords = await _context.SellingPets.CountAsync();

                var (_, responseData) = SellingPetConversion.FromEntity(null, sellingPets);

                return new Response(true, 200, "Lấy danh sách thú cưng thành công")
                {
                    Data = new
                    {
                        data = responseData,
                        meta = new
                        {
                            currentPage = pageIndex,
                            totalPages = Math.Ceiling((double)totalRecords / pageSize),
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

        public async Task<Response> GetByAsync(Expression<Func<SellingPet, bool>> predicate)
        {
            try
            {
                var sellingPet = await _context.SellingPets
                    .Include(x => x.PetBreed)
                    .Include(x => x.PetImagePaths)
                    .FirstOrDefaultAsync(predicate);

                if (sellingPet is null)
                {
                    return new Response(false, 404, "Không tìm thấy thú cưng cần tìm");
                }

                var (responseData, _) = SellingPetConversion.FromEntity(sellingPet, null);

                return new Response(true, 200, "Lấy thông tin thú cưng thành công")
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
                var sellingPet = await FindById(id, true, true);

                if (sellingPet is null)
                {
                    return new Response(false, 404, "Không tìm thấy thu cưng cần tìm");
                }

                var (responseData, _) = SellingPetConversion.FromEntity(sellingPet, null);

                return new Response(true, 200, "Lấy thông tin thú cưng thành công")
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

        public async Task<Response> UpdateAsync(Guid id, SellingPet entity)
        {
            try
            {
                var exsitingPet = await FindById(id, false, true);

                if (exsitingPet is null)
                {
                    return new Response(false, 404, "Không tìm thấy thú cưng cần cập nhật");
                }

                bool hasChanges = exsitingPet.PetName != entity.PetName
                    || exsitingPet.PetDateOfBirth != entity.PetDateOfBirth
                    || exsitingPet.PetGender != entity.PetGender
                    || exsitingPet.PetFurColor != entity.PetFurColor
                    || exsitingPet.PetWeight != entity.PetWeight
                    || exsitingPet.PetDesc != entity.PetDesc
                    || exsitingPet.PetFamilyRole != entity.PetFamilyRole
                    || exsitingPet.IsVisible != entity.IsVisible
                    || exsitingPet.MotherId != entity.MotherId
                    || exsitingPet.FatherId != entity.FatherId
                    || exsitingPet.BreedId != entity.BreedId;

                if (!hasChanges)
                {
                    return new Response(true, 204, "Không có thay đổi nào được thực hiện");
                }

                exsitingPet.PetName = entity.PetName;
                exsitingPet.PetDateOfBirth = entity.PetDateOfBirth;
                exsitingPet.PetGender = entity.PetGender;
                exsitingPet.PetFurColor = entity.PetFurColor;
                exsitingPet.PetWeight = entity.PetWeight;
                exsitingPet.PetDesc = entity.PetDesc;
                exsitingPet.PetFamilyRole = entity.PetFamilyRole;
                exsitingPet.IsVisible = entity.IsVisible;
                exsitingPet.MotherId = entity.MotherId;
                exsitingPet.FatherId = entity.FatherId;
                exsitingPet.BreedId = entity.BreedId;

                await _context.SaveChangesAsync();

                var (responseData, _) = SellingPetConversion.FromEntity(exsitingPet, null);

                return new Response(true, 200, "Cập nhật thông tin thú cưng thành công")
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

        public async Task<SellingPet> FindById(Guid id, bool noTracking = false, bool includeRelatedData = false)
        {
            var query = _context.SellingPets.AsQueryable();

            if (noTracking)
            {
                query = query.AsNoTracking();
            }

            if (includeRelatedData)
            {
                query = query
                    .Include(x => x.PetBreed)
                    .Include(x => x.Mother)
                    .Include(x => x.Father)
                    .Include(x => x.PetImagePaths)
                    .Include(x => x.ChildrenFromMother)
                    .Include(x => x.ChildrenFromFather)
                    .Include(x => x.PetHealthBook);
            }

            return await query.FirstOrDefaultAsync(x => x.PetId == id) ?? null!;
        }
    }
}
