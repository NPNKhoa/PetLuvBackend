using PetLuvSystem.SharedLibrary.Responses;

namespace PetApi.Application.Interfaces
{
    public interface IStatistic
    {
        public Task<Response> GetPetCountByBreed(string? typeName);
    }
}
