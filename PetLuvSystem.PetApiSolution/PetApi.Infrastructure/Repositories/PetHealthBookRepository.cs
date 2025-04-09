using Microsoft.EntityFrameworkCore;
using PetApi.Application.Interfaces;
using PetApi.Domain.Entities;
using PetApi.Infrastructure.Data;
using PetLuvSystem.SharedLibrary.Logs;
using PetLuvSystem.SharedLibrary.Responses;
using System.Linq.Expressions;

namespace PetApi.Infrastructure.Repositories
{
    public class PetHealthBookRepository(PetDbContext _context) : IPetHealthBook
    {
        public Task<Response> CreateAsync(PetHealthBook entity)
        {
            throw new NotImplementedException();
        }

        public Task<Response> DeleteAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<Response> GetAllAsync(int pageIndex = 1, int pageSize = 10)
        {
            throw new NotImplementedException();
        }

        public Task<Response> GetByAsync(Expression<Func<PetHealthBook, bool>> predicate)
        {
            throw new NotImplementedException();
        }

        public async Task<Response> GetByIdAsync(Guid id)
        {
            try
            {
                var healthBook = await _context.PetHealthBooks
                    .AsNoTracking()
                    .Include(p => p.Pet)
                    .Include(p => p.PetHealthBookDetails)
                    .FirstOrDefaultAsync(p => p.PetHealthBookId == id);

                if (healthBook is null)
                {
                    return new Response(false, 404, "Không tìm thấy sổ sức khỏe này");
                }

                return new Response(true, 200, "Found")
                {
                    Data = healthBook
                };
            }
            catch (Exception ex)
            {
                LogException.LogExceptions(ex);
                return new Response(false, 500, "Internal Server Error");
            }
        }

        public Task<Response> UpdateAsync(Guid id, PetHealthBook entity)
        {
            throw new NotImplementedException();
        }
    }
}
