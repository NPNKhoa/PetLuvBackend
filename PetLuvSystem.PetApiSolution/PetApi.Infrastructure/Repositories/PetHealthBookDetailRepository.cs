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
    public class PetHealthBookDetailRepository(PetDbContext _context) : IPetHealthBookDetail
    {
        public async Task<Response> CreateAsync(PetHealthBookDetail entity)
        {
            try
            {
                var existingPetHealthBook = await _context.PetHealthBookDetails
                    .FirstOrDefaultAsync(x => x.HealthBookId == entity.HealthBookId
                                        && x.UpdatedDate == entity.UpdatedDate
                                        && x.TreatmentName.Trim().ToLower().Equals(entity.TreatmentName.ToLower()));

                if (existingPetHealthBook is not null)
                {
                    return new Response(false, 409, "Ngày không hợp lệ");
                }

                await _context.PetHealthBookDetails.AddAsync(entity);
                await _context.SaveChangesAsync();

                var (response, _) = PetHealthBookDetailConversion.FromEntity(entity, null!);

                return new Response(true, 201, "Success")
                {
                    Data = response
                };
            }
            catch (Exception ex)
            {
                LogException.LogExceptions(ex);
                var innerExceptionMessage = ex.InnerException?.Message ?? ex.Message;
                Console.WriteLine("Inner Exception: " + innerExceptionMessage);
                return new Response(false, 500, "Internal Server Error");
            }
        }

        public async Task<Response> DeleteAsync(Guid id)
        {
            try
            {
                var existingPetHealthBook = await FindById(id);

                if (existingPetHealthBook is null)
                {
                    return new Response(false, 404, "Không thấy sổ sức khỏe cần tìm");
                }

                _context.PetHealthBookDetails.Remove(existingPetHealthBook);
                await _context.SaveChangesAsync();

                return new Response(true, 200, "Xóa thành công");
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
                var petHealthBookDetails = await _context.PetHealthBookDetails
                    .Skip((pageIndex - 1) * pageSize)
                    .Take(pageSize)
                    .ToListAsync();

                if (petHealthBookDetails is null || petHealthBookDetails.Count == 0)
                {
                    return new Response(false, 404, "Không tìm thấy chi tiết sổ sức khỏe thú cưng");
                }

                var totalRecords = await _context.PetHealthBookDetails.CountAsync();

                var (_, responses) = PetHealthBookDetailConversion.FromEntity(null!, petHealthBookDetails);

                return new Response(true, 200, "Success")
                {
                    Data = new
                    {
                        data = responses,
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

        public async Task<Response> GetByHealthBook(Guid id, int pageIndex = 1, int pageSize = 10)
        {
            try
            {
                if (id != Guid.Empty)
                {
                    LogException.LogInformation("id ne: " + id.ToString());
                }
                else
                {
                    LogException.LogInformation("Id rong roi");
                }

                var petHealthBookDetails = await _context.PetHealthBookDetails
                    .Where(x => x.HealthBookId == id)
                    .OrderByDescending(x => x.UpdatedDate)
                    .Skip((pageIndex - 1) * pageSize)
                    .Take(pageSize)
                    .ToListAsync();

                if (petHealthBookDetails is null || petHealthBookDetails.Count == 0)
                {
                    return new Response(false, 404, "Không tìm thấy chi tiết sổ sức khỏe thú cưng");
                }

                var totalRecords = await _context.PetHealthBookDetails.Where(x => x.HealthBookId == id).CountAsync();

                var (_, responses) = PetHealthBookDetailConversion.FromEntity(null!, petHealthBookDetails);

                return new Response(true, 200, "Success")
                {
                    Data = new
                    {
                        data = responses,
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

        public async Task<Response> GetByAsync(Expression<Func<PetHealthBookDetail, bool>> predicate)
        {
            try
            {
                var petHealthBookDetail = await _context.PetHealthBookDetails.Where(predicate).FirstOrDefaultAsync();

                if (petHealthBookDetail is null)
                {
                    return new Response(false, 404, "Không tìm thấy chi tiết sổ sức khỏe thú cưng");
                }

                var (response, _) = PetHealthBookDetailConversion.FromEntity(petHealthBookDetail, null!);

                return new Response(true, 200, "Success")
                {
                    Data = response
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
                var petHealthBookDetail = await FindById(id, true, true);

                if (petHealthBookDetail is null)
                {
                    return new Response(false, 404, "Không tìm thấy chi tiết sổ sức khỏe thú cưng");
                }

                var (response, _) = PetHealthBookDetailConversion.FromEntity(petHealthBookDetail, null!);

                return new Response(true, 200, "Success")
                {
                    Data = response
                };
            }
            catch (Exception ex)
            {
                LogException.LogExceptions(ex);
                return new Response(false, 500, "Internal Server Error");
            }
        }

        public async Task<Response> UpdateAsync(Guid id, PetHealthBookDetail entity)
        {
            try
            {
                var petHealthBookDetail = await FindById(id, true, true);

                if (petHealthBookDetail is null)
                {
                    return new Response(false, 404, "Không tìm thấy chi tiết sổ sức khỏe thú cưng");
                }

                bool hasChanges = entity.PetHealthNote != petHealthBookDetail.PetHealthNote
                        || entity.TreatmentName != petHealthBookDetail.TreatmentName
                        || entity.TreatmentDesc != petHealthBookDetail.TreatmentDesc
                        || entity.TreatmentProof != petHealthBookDetail.TreatmentProof
                        || entity.VetName != petHealthBookDetail.VetName
                        || entity.VetDegree != petHealthBookDetail.VetDegree
                        || entity.UpdatedDate != petHealthBookDetail.UpdatedDate;

                if (!hasChanges)
                {
                    return new Response(true, 204, "No Change made");
                }

                petHealthBookDetail.PetHealthNote = entity.PetHealthNote;
                petHealthBookDetail.TreatmentName = entity.TreatmentName;
                petHealthBookDetail.TreatmentDesc = entity.TreatmentDesc;
                petHealthBookDetail.TreatmentProof = entity.TreatmentProof;
                petHealthBookDetail.VetName = entity.VetName;
                petHealthBookDetail.VetDegree = entity.VetDegree;
                petHealthBookDetail.UpdatedDate = entity.UpdatedDate;

                await _context.SaveChangesAsync();

                var (response, _) = PetHealthBookDetailConversion.FromEntity(petHealthBookDetail, null!);

                return new Response(true, 200, "Updated")
                {
                    Data = response
                };
            }
            catch (Exception ex)
            {
                LogException.LogExceptions(ex);
                return new Response(false, 500, "Internal Server Error");
            }
        }

        public async Task<PetHealthBookDetail> FindById(Guid id, bool noTracking = false, bool includeOthers = false)
        {
            var query = _context.PetHealthBookDetails.AsQueryable();

            if (noTracking)
            {
                query = query.AsNoTracking();
            }

            if (includeOthers)
            {
                query = query.Include(x => x.PetHealthBook);
            }

            return await query.FirstOrDefaultAsync(x => x.HealthBookDetailId == id) ?? null!;
        }
    }
}
