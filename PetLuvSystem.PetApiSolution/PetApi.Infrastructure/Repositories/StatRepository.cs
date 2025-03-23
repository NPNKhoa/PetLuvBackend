using Microsoft.EntityFrameworkCore;
using PetApi.Application.Interfaces;
using PetApi.Infrastructure.Data;
using PetLuvSystem.SharedLibrary.Logs;
using PetLuvSystem.SharedLibrary.Responses;

namespace PetApi.Infrastructure.Repositories
{
    public class StatRepository(PetDbContext _context) : IStatistic
    {
        public async Task<Response> GetPetCountByBreed(string? petTypeName)
        {
            try
            {
                var query = _context.Pets
                    .Where(p => p.IsVisible)
                    .Include(p => p.PetBreed)
                    .ThenInclude(b => b.PetType)
                    .AsQueryable();

                if (!string.IsNullOrEmpty(petTypeName))
                {
                    string normalizedPetTypeName = petTypeName.Trim().ToLower();
                    query = query.Where(p => p.PetBreed.PetType.PetTypeName.ToLower().Contains(normalizedPetTypeName));
                }

                var petData = await query
                    .GroupBy(p => new { p.PetBreed.PetType.PetTypeName, p.PetBreed.BreedName })
                    .Select(group => new
                    {
                        PetType = group.Key.PetTypeName,
                        Breed = group.Key.BreedName,
                        Count = group.Count()
                    })
                    .ToListAsync();

                if (!petData.Any())
                {
                    return new Response(false, 404, "Không tìm thấy dữ liệu!");
                }

                return new Response(true, 200, "OK") { Data = petData };
            }
            catch (Exception ex)
            {
                LogException.LogExceptions(ex);
                return new Response(false, 500, "Internal Server Error");
            }
        }

    }
}
