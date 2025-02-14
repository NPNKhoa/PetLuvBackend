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
        public async Task<Response> CreateAsync(SellingPet entity)
        {
            try
            {
                if (entity.PetDateOfBirth > DateTime.Now)
                {
                    return new Response(false, 400, "Ngày sinh không hợp lệ");
                }

                var exisitingBreed = await _context.PetBreeds.FirstOrDefaultAsync(x => x.BreedId == entity.BreedId);

                if (exisitingBreed is null)
                {
                    return new Response(false, 404, "Không tìm thấy giống thú cưng");
                }

                if (entity.MotherId is not null && entity.MotherId != Guid.Empty)
                {
                    var existingMother = await _context.SellingPets.FirstOrDefaultAsync(x => x.PetId == entity.MotherId);

                    if (existingMother is null)
                    {
                        return new Response(false, 404, "Không tìm thấy mẹ của thú cưng");
                    }
                }

                if (entity.FatherId is not null && entity.FatherId != Guid.Empty)
                {
                    var existingFather = await _context.SellingPets.FirstOrDefaultAsync(x => x.PetId == entity.FatherId);

                    if (existingFather is null)
                    {
                        return new Response(false, 404, "Không tìm thấy mẹ của thú cưng");
                    }
                }

                var existingPet = await GetByAsync(x => x.PetName == entity.PetName && x.BreedId == entity.BreedId);

                if (existingPet.Data is not null)
                {
                    return new Response(false, 409, "Thú cưng đã tồn tại");
                }

                await _context.SellingPets.AddAsync(entity);
                await _context.SaveChangesAsync();

                entity.PetBreed = exisitingBreed;

                var (responseData, _) = SellingPetConversion.FromEntity(entity, null);

                return new Response(false, 201, "Tạo thú cưng thành công")
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
                var existingPet = await FindById(id, false, true);

                if (existingPet is null)
                {
                    return new Response(false, 404, "Không tìm thấy thú cưng cần xóa");
                }

                if (existingPet.IsVisible)
                {
                    existingPet.IsVisible = false;
                    await _context.SaveChangesAsync();

                    return new Response(true, 200, "Ẩn thú cưng thành công");
                }

                if (existingPet.ChildrenFromFather is null
                    || existingPet.ChildrenFromMother is null
                    || existingPet.ChildrenFromFather.Count > 0
                    || existingPet.ChildrenFromMother.Count > 0)
                {
                    return new Response(false, 400, "Không thể xóa thú cưng này vì thú cưng này là cha hoặc mẹ của một số thú cưng khác");
                }

                // TODO: Check pet in order

                _context.SellingPets.Remove(existingPet);
                await _context.SaveChangesAsync();

                return new Response(true, 200, "Xóa thú cưng thành công");
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
